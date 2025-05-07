import { Component,OnInit, ViewChild } from '@angular/core';
import { LoaderComponent } from '../loader/loader.component';
import { CommonModule } from '@angular/common';
import { ControlContainer, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { AddEmployeeDto, EditEmployeeDto, MasterEmployee, MasterEmployeeDto } from '../../models/master-employee';
import { MasterEmployeeService } from '../../services/master-employee.service';
import { Pagination } from '../../utils/pagination';
import { Alert } from '../../utils/alert';
import { TYPE } from '../auth/login/values.constants';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { MasterDepartmentService } from '../../services/master-department.service';
import { MasterDepartment } from '../../models/master-department';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-master-employee',
  standalone: true,
  imports: [CommonModule, LoaderComponent, FormsModule,RouterModule,ReactiveFormsModule,MatTableModule,MatSortModule,MatFormFieldModule,MatInputModule],
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
formsValue:any;
departments:MasterDepartment[] = [];
mode?:string;
valueId?:number;

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
        console.log(this.mode);
    }
  })
}

resetForm() {
  this.addEmployeeForm.reset();
  console.log(this.mode);
  this.mode='';
}

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

fetchEmployees(){
  this.employeeService.getEmployees(this.currentPage, this.pageSize, this?.selectedUnit, this?.searchTerm)
  .subscribe({
    next:(response: MasterEmployeeDto) => {
      this.loading = false;
      console.log(response);
      this.dataSource.data = response.data;
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
      this.errorMsg = JSON.stringify((error.message !== undefined)?error.error: error.error.message);
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

addEmployee(){
  const formValues=this.addEmployeeForm.value;
  this.mode='add';
}

viewEmployee(valueId?:number){
  if (valueId !== undefined) {
    this.employeeService.getEmployeeById(valueId).subscribe({
      next: (employee) => {
        this.addEmployeeForm.patchValue(employee); 
        console.log('Fetched Employee:', employee);
        this.fetchEmployees();
      },
      error: (error) => {
        console.error('Error fetching employee data:', error);
        Alert.toast(TYPE.ERROR, true, 'Failed to load employee data.');
        this.router.navigate(['/masters/employeeMasters']);
      }
    });
  } else {
    console.error('Invalid valueId:', valueId);
    Alert.toast(TYPE.ERROR, true, 'Invalid employee ID.');
  }
}


empId:number = 0;
editEmployee(employee:MasterEmployee){
  this.empId = employee.valueId;
  console.log(this.empId);
  const formValues=this.addEmployeeForm.value;
  this.mode='edit';
  if(employee.valueId){
    this.employeeService.getEmployeeById(employee.valueId).subscribe({
      next: (employeeData) => {
        this.addEmployeeForm.patchValue(employeeData);
        console.log('Fetched Employee for Edit:', employeeData);
      },
      error: (error) => {
        console.error('Error fetching employee data:', error);
        Alert.toast(TYPE.ERROR, true, 'Failed to load employee data.');
        this.router.navigate(['/masters/employeeMasters']);
      }
    });
  } else {
    console.error('Invalid valueId:', employee.valueId);
    Alert.toast(TYPE.ERROR, true, 'Invalid employee ID.');
  }
}

getDepartmentName(){
  this.departmentService.getAllDepartments(1, 100).subscribe((res)=>{
    this.departments = res;
    console.log(this.departments);
  });
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
      if (!this.empId) {
        console.error('valueId is undefined. Cannot update employee.');
        Alert.toast(TYPE.ERROR, true, 'Invalid employee ID.');
        return;
      }
      this.employeeService.updateEmployee(this.empId, addFormValues).subscribe({
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

// sakthish table ts

displayedColumns: string[] = ['employeeName', 'unit', 'role', 'action'];
  dataSource = new MatTableDataSource<MasterEmployee>();
  @ViewChild(MatSort) sort!: MatSort;
  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
  }
}
