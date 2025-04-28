import { Component } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { LoaderComponent } from "../loader/loader.component";
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-add-employee',
  standalone: true,
  imports: [ReactiveFormsModule,CommonModule,RouterModule],
  templateUrl: './add-employee.component.html',
  styleUrl: './add-employee.component.css'
})
export class AddEmployeeComponent {
  addEmployeeForm: FormGroup= new FormGroup({
    employeeName:new FormControl("",Validators.required),
    employeeCode:new FormControl("",Validators.required),
    unit:new FormControl("All",Validators.required),
    department: new FormGroup("", Validators.required),
    employeeMobile:new FormGroup("", Validators.required),
    employeeEmail: new FormGroup("", Validators.required),
    employeeExtension: new FormGroup("", Validators.required),
    role:new FormGroup("",Validators.required)
  })

  constructor(private router:Router){}

  formsValue:any;

  onSubmit(){
    this.formsValue=this.addEmployeeForm.value;
  }

  onCancel(){
    this.router.navigate(['masters/employeeMasters']);
  }

}
