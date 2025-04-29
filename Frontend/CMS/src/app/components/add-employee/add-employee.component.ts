import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { LoaderComponent } from "../loader/loader.component";
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { MasterDepartment } from '../../models/master-department';
import { MasterDepartmentService } from '../../services/master-department.service';
import { AddEmployeeDto, EditEmployeeDto } from '../../models/master-employee';
import { MasterEmployeeService } from '../../services/master-employee.service';
import { Alert } from '../../utils/alert';
import { TYPE } from '../auth/login/values.constants';

@Component({
  selector: 'app-add-employee',
  standalone: true,
  imports: [ReactiveFormsModule,CommonModule,RouterModule],
  templateUrl: './add-employee.component.html',
  styleUrl: './add-employee.component.css'
})
export class AddEmployeeComponent implements OnInit{
  errorMsg:string = '';
  addEmployeeForm: FormGroup= new FormGroup({
    employeeName:new FormControl('',[Validators.required]),
    password:new FormControl("",Validators.required),
    role:new FormControl("",Validators.required),
    employeeCode:new FormControl("",Validators.required),
    unit:new FormControl("All",Validators.required),
    department: new FormGroup("", Validators.required),
    employeeMobile:new FormControl("", Validators.required),
    email: new FormControl("", [Validators.required, Validators.email]),
    employeeExtension: new FormControl("", Validators.required)
  })

  formsValue:any;
  departments:MasterDepartment[] = [];
  mode:'add'|'edit'|'view'='add';
  valueId:number=0;

  ngOnInit(){
    this.getDepartmentName();

    const idParam=this.route.snapshot.paramMap.get('id');
    if(idParam){
      const valueId=+idParam;
      this.mode=this.route.snapshot.url[0].path === 'view' ? 'view' : 'edit';
      this.fetchEmployeeData(this.valueId);
    }
  }

  constructor(
    private route:ActivatedRoute,
    private router:Router, 
    private departmentService:MasterDepartmentService, 
    private employeeService: MasterEmployeeService
  ){}


  getDepartmentName(){
    this.departmentService.getAllDepartments(1, 100).subscribe((res)=>{
      this.departments = res;
      console.log(this.departments);
    });
  }

  fetchEmployeeData(id:number){
    this.employeeService.getEmployeeById(id).subscribe({
      next:(employee)=>{
        this.addEmployeeForm.patchValue(employee);
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
      const department = this.addEmployeeForm.value.department;
      const employeeMobile = this.addEmployeeForm.value.employeeMobile;
      const email = this.addEmployeeForm.value.email;
      const employeeExtension = this.addEmployeeForm.value.employeeExtension;
      if(employeeMobile && Number(employeeMobile)
      ){
        const addFormValues:AddEmployeeDto = new AddEmployeeDto();
        addFormValues.employeeName = this.addEmployeeForm.value.employeeName;
        addFormValues.password = this.addEmployeeForm.value.password;
        addFormValues.role =this.addEmployeeForm.value.role;
        addFormValues.employeeCode =this.addEmployeeForm.value.employeeCode;
        addFormValues.unit =this.addEmployeeForm.value.unit;
        addFormValues.department =this.addEmployeeForm.value.department;
        addFormValues.employeeMobile = Number(employeeMobile);
  
        addFormValues.email =this.addEmployeeForm.value.email;
        addFormValues.employeeExtension =this.addEmployeeForm.value.employeeExtension;
        console.log(addFormValues);
        this.employeeService.addEmployee(addFormValues).subscribe({
          next:(response:AddEmployeeDto) => {
              Alert.toast(TYPE.SUCCESS,true,'Added successfully');
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
      const department = this.addEmployeeForm.value.department;
      const employeeMobile = this.addEmployeeForm.value.employeeMobile;
      const email = this.addEmployeeForm.value.email;
      const employeeExtension = this.addEmployeeForm.value.employeeExtension;
      if(employeeMobile && Number(employeeMobile)
      ){
        const addFormValues:EditEmployeeDto = new EditEmployeeDto();
        addFormValues.employeeName = this.addEmployeeForm.value.employeeName;
        addFormValues.password = this.addEmployeeForm.value.password;
        addFormValues.role =this.addEmployeeForm.value.role;
        addFormValues.employeeCode =this.addEmployeeForm.value.employeeCode;
        addFormValues.unit =this.addEmployeeForm.value.unit;
        addFormValues.department =this.addEmployeeForm.value.department;
        addFormValues.employeeMobile = Number(employeeMobile);
  
        addFormValues.email =this.addEmployeeForm.value.email;
        addFormValues.employeeExtension =this.addEmployeeForm.value.employeeExtension;
        console.log(addFormValues);
        this.employeeService.updateEmployee(this.valueId, addFormValues).subscribe({
          next:(response:EditEmployeeDto) => {
              Alert.toast(TYPE.SUCCESS,true,'Updated successfully');
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

  onCancel(){
    this.router.navigate(['masters/employeeMasters']);
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
  get department(){
    return this.addEmployeeForm.get('department');
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
