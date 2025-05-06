import { Component,OnInit } from '@angular/core';
import { LoaderComponent } from '../loader/loader.component';
import { CommonModule } from '@angular/common';
import { ControlContainer, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { AddEmployeeDto, EditEmployeeDto, MasterEmployee, MasterEmployeeDto } from '../../models/master-employee';
import { MasterEmployeeService } from '../../services/master-employee.service';
import { HttpClient } from '@angular/common/http';
import { Pagination } from '../../utils/pagination';
import { Alert } from '../../utils/alert';
import { TYPE } from '../auth/login/values.constants';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { MasterDepartmentService } from '../../services/master-department.service';
import { MasterDepartment } from '../../models/master-department';

@Component({
  selector: 'app-master-employee',
  standalone: true,
  imports: [CommonModule, LoaderComponent, FormsModule,RouterModule,ReactiveFormsModule],
  templateUrl: './master-employee.component.html',
  styleUrl: './master-employee.component.css'
})

export class MasterEmployeeComponent implements OnInit{
loading=true;
employees: MasterEmployee[]=[];
totalEmployees:number=0;
totalPages:number=0;
currentPage:number=1;
pageSize:number=2;
maxPage:number=1; //used now 
pageNumbers:number[] = []; //used now 
selectedUnit: string = 'All';
searchTerm: string = '';
errorMsg ?: string;

// constructor(private router: Router){}

constructor(
  private employeeService: MasterEmployeeService,
  private route:ActivatedRoute,
  private router:Router, 
  private departmentService:MasterDepartmentService
){}

ngOnInit(): void {
  this.fetchEmployees();
  this.getDepartmentName();
    this.route.params.subscribe(params=>{
      console.log('Route Params:', params);
      const paramValueId=params['valueId']
      if(paramValueId){
         this.valueId=+paramValueId;
        console.log('Dynamic valueId:', this.valueId);
        this.mode=this.route.snapshot.url[2].path === 'editEmployee' ? 'edit' : 'view';
        console.log(this.mode);
        this.fetchEmployeeData(this.valueId);
    }
  })
}

fetchEmployees(){
  this.employeeService.getEmployees(this.currentPage, this.pageSize, this?.selectedUnit, this?.searchTerm)
  .subscribe({
    next:(response: MasterEmployeeDto) => {
      this.loading = false;
      console.log(response);
      this.employees = response.data;
      this.totalEmployees = response.totalCount;
      if(this.employees!= undefined && this.employees.length > 0){
        let result = Pagination.paginator(this.currentPage,this.totalEmployees,this.pageSize);
        this.maxPage = result.maxPage;
        console.log(this.maxPage);
        this.pageNumbers = result.pageNumbers;
      }
    },
    error:(error)=> {
      this.loading = false;
      console.error('Error :(', error);
      this.errorMsg = JSON.stringify((error.message !== undefined)?error.error.title: error.message);
      Alert.toast(TYPE.ERROR,true,this.errorMsg);
    }
  });
}

GetPage(pgNumber:number){
  if(this.maxPage >= pgNumber && pgNumber >= 1){
    this.currentPage=pgNumber;
    this.fetchEmployees();
  }
}

onFilterChange(){
  this.currentPage=1;
  this.fetchEmployees();
}


getPageNumbers():number[]{
  const pageNumbers=[];
  for(let i=1; i<=this.totalPages;i++){
    pageNumbers.push(i)
  }
  return pageNumbers
}

addEmployee(){
  this.router.navigate(['masters/employeeMasters/addEmployee']);
}

deleteEmployee(employee:MasterEmployee){
  Alert.confirmToast("Are you sure you want to delete this Employee?",
    "You won't be able to revert this!", TYPE.WARNING,
    "Yes, delete it!",
    "Deleted successfully!",
    "Company has been deleted.", TYPE.SUCCESS,() => {
     this.employeeService.deleteEmployee(employee.valueId).subscribe({
       next:(response:boolean)=>{
         if(response){
           Alert.toast(TYPE.SUCCESS,true,"Deleted successfully");
           this.fetchEmployees();
         }
       }
     });
  });
}

viewEmployee(employee:MasterEmployee){
  console.log('Navigating to viewEmployee with valueId:', employee.valueId);
  this.router.navigate(['masters/employeeMasters/viewEmployee', employee.valueId]);
}

editEmployee(employee:MasterEmployee){
  console.log('Navigating to editEmployee with valueId:', employee.valueId);
  this.router.navigate(['masters/employeeMasters/editEmployee', employee.valueId]);
}

// Adding Modal

addEmployeeForm: FormGroup= new FormGroup({
  employeeName:new FormControl('',[Validators.required]),
  password:new FormControl("",Validators.required),
  role:new FormControl("",Validators.required),
  employeeCode:new FormControl("",Validators.required),
  unit:new FormControl("",Validators.required),
  departmentId: new FormControl("", Validators.required),
  departmentName: new FormControl(""),
  employeeMobile:new FormControl("", Validators.required),
  email: new FormControl("", [Validators.required, Validators.email]),
  employeeExtension: new FormControl("", Validators.required)
})

formsValue:any;
departments:MasterDepartment[] = [];
mode:'add'|'edit'|'view'='add';
valueId?:number;

  
  getDepartmentName(){
    this.departmentService.getAllDepartments(1, 100).subscribe((res)=>{
      this.departments = res;
      console.log(this.departments);
    });
  }

  fetchEmployeeData(valueId:number){
    this.employeeService.getEmployeeById(valueId).subscribe({
      next:(employee)=>{
        this.addEmployeeForm.patchValue(employee);
        console.log(this.addEmployeeForm.value.departmentId);
        
        if(this.mode==='view'){
          this.addEmployeeForm.disable();
        }
      },
      error:(error)=>{
        console.error('Error fetching the data:',error);
        Alert.toast(TYPE.ERROR, true, 'Failed to load employee data.');
        this.router.navigate(['/masters/employeeMasters']);
      }
    })
  }

  onSubmit(){
    this.formsValue=this.addEmployeeForm.value;
    if(this.addEmployeeForm.invalid){
          this.addEmployeeForm.markAllAsTouched();
          return;
    }

    const formValues=this.addEmployeeForm.value;
    if(this.mode==='add'){
      const employeeName = this.addEmployeeForm.value.employeeName;
      const password = this.addEmployeeForm.value.password;
      const role = this.addEmployeeForm.value.role;
      const employeeCode = this.addEmployeeForm.value.employeeCode;
      const unit = this.addEmployeeForm.value.unit;
      const departmentId = this.addEmployeeForm.value.departmentId;
      const employeeMobile = this.addEmployeeForm.value.employeeMobile;
      const email = this.addEmployeeForm.value.email;
      const employeeExtension = this.addEmployeeForm.value.employeeExtension;
      if(employeeMobile && Number(employeeMobile) && Number(departmentId)
      ){
        const addFormValues:AddEmployeeDto = new AddEmployeeDto();
        addFormValues.employeeName = this.addEmployeeForm.value.employeeName;
        addFormValues.password = this.addEmployeeForm.value.password;
        addFormValues.role =this.addEmployeeForm.value.role;
        addFormValues.employeeCode =this.addEmployeeForm.value.employeeCode;
        addFormValues.unit =this.addEmployeeForm.value.unit;
        addFormValues.departmentId = Number(departmentId);
        addFormValues.employeeMobile = Number(employeeMobile);
  
        addFormValues.email =this.addEmployeeForm.value.email;
        addFormValues.employeeExtension =this.addEmployeeForm.value.employeeExtension;
        console.log(addFormValues);
        this.employeeService.addEmployee(addFormValues).subscribe({
          next:(response:AddEmployeeDto) => {
              Alert.toast(TYPE.SUCCESS,true,'Added successfully');
              this.router.navigate(['masters/employeeMasters']);
          }, 
          error:(error) => {
            console.error('Error :(', error);
            this.errorMsg = JSON.stringify((error.message !== undefined)?error.error.title: error.message);
            Alert.toast(TYPE.ERROR,true,this.errorMsg);
          }
        });
      }
      else{
        console.log("should not come here ", this.addEmployeeForm.value)
      }
    }
    else if(this.mode === 'edit'){
      const employeeName = this.addEmployeeForm.value.employeeName;
      const password = this.addEmployeeForm.value.password;
      const role = this.addEmployeeForm.value.role;
      const employeeCode = this.addEmployeeForm.value.employeeCode;
      const unit = this.addEmployeeForm.value.unit;
      const departmentId = this.addEmployeeForm.value.departmentId;
      const employeeMobile = this.addEmployeeForm.value.employeeMobile;
      const email = this.addEmployeeForm.value.email;
      const employeeExtension = this.addEmployeeForm.value.employeeExtension;
      if(employeeMobile && Number(employeeMobile) && Number(departmentId)
      ){
        const addFormValues:EditEmployeeDto = new EditEmployeeDto();
        addFormValues.employeeName = this.addEmployeeForm.value.employeeName;
        addFormValues.password = this.addEmployeeForm.value.password;
        addFormValues.role =this.addEmployeeForm.value.role;
        addFormValues.employeeCode =this.addEmployeeForm.value.employeeCode;
        addFormValues.unit =this.addEmployeeForm.value.unit;
        addFormValues.departmentId =Number(departmentId);
        addFormValues.employeeMobile = Number(employeeMobile);
  
        addFormValues.email =this.addEmployeeForm.value.email;
        addFormValues.employeeExtension =this.addEmployeeForm.value.employeeExtension;
        console.log(addFormValues);
        if (!this.valueId) {
          console.error('valueId is undefined. Cannot update employee.');
          Alert.toast(TYPE.ERROR, true, 'Invalid employee ID.');
          return;
        }
        this.employeeService.updateEmployee(this.valueId, addFormValues).subscribe({
          next:(response:EditEmployeeDto) => {
              Alert.toast(TYPE.SUCCESS,true,'Updated successfully');
              this.router.navigate(['masters/employeeMasters']);
          }, 
          error:(error) => {
            console.error('Error :(', error);
            this.errorMsg = JSON.stringify((error.message !== undefined)?error.error.title: error.message);
            Alert.toast(TYPE.ERROR,true,this.errorMsg);
          }
        });
      }
      else{
        console.log("should not come here ", this.addEmployeeForm.value)
      }
    }
  }

  // onCancel(){
  //   this.router.navigate(['masters/employeeMasters']);
  // }
  get employeeName(){
    return this.addEmployeeForm.get('employeeName');
  }
  get password(){
    return this.addEmployeeForm.get('password');
  }
  get role(){
    return this.addEmployeeForm.get('role');
  }
  get employeeCode(){
    return this.addEmployeeForm.get('employeeCode');
  }
  get unit(){
    return this.addEmployeeForm.get('unit');
  }
  get departmentId(){
    return this.addEmployeeForm.get('departmentId');
  }
  get departmentName(){
    return this.addEmployeeForm.get('departmentName');
  }
  get employeeMobile(){
    return this.addEmployeeForm.get('employeeMobile');
  }
  get email(){
    return this.addEmployeeForm.get('email');
  }
  get employeeExtension(){
    return this.addEmployeeForm.get('employeeExtension');
  }

}
