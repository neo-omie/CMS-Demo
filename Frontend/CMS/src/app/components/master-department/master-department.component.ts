import { CommonModule } from '@angular/common';
import { Component, ElementRef, Renderer2, ViewChild } from '@angular/core';
import { LoaderComponent } from '../loader/loader.component';
import { AddDepartmentDto, GetAllDepartmentsDto, MasterDepartment } from '../../models/master-department';
import { MasterDepartmentService } from '../../services/master-department.service';
import { Pagination } from '../../utils/pagination';
import { Alert } from '../../utils/alert';
import { TYPE } from '../auth/login/values.constants';
import { FormsModule, NgForm } from '@angular/forms';

@Component({
  selector: 'app-master-department',
  standalone: true,
  imports: [CommonModule, LoaderComponent, FormsModule],
  templateUrl: './master-department.component.html',
  styleUrl: './master-department.component.css'
})
export class MasterDepartmentComponent {
  loading:boolean = true
  pageNumbers:number[] = [];
  maxPage:number = 1;
  departments:GetAllDepartmentsDto[] = [];
  dept?:MasterDepartment;
  errorMsg ?: string;
  @ViewChild('editDepartmentName') editDepartmentName!: ElementRef;
  constructor(private departmentService : MasterDepartmentService, private renderer : Renderer2){}
      ngOnInit(){
        this.GetAllDepartments(1 ,10);
      }
      closeEditApproverCollapses() {
          this.renderer.removeClass(this.editDepartmentName.nativeElement,'show');
      }
      GetAllDepartments(pageNumber : number, pageSize : number){
        this.departmentService.getAllDepartments(pageNumber, pageSize).subscribe({
          next:(response:GetAllDepartmentsDto[]) => {
            this.loading = false;
            this.departments = response;
            if(this.departments != undefined && this.departments.length > 0){
              let result = Pagination.paginator(pageNumber,this.departments[0].totalRecords,pageSize)
              this.maxPage = result.maxPage;
              this.pageNumbers = result.pageNumbers;
            }
          }, 
          error:(error) => {
            this.loading = false;
            console.error('Error :(', error);
            this.errorMsg = JSON.stringify((error.message !== undefined)?error.error.title: error.message);
            Alert.toast(TYPE.ERROR,true,this.errorMsg);
          }
        });
      }
      GetPage(pgNumber:number){
        if(this.maxPage >= pgNumber && pgNumber >= 1){
          this.GetAllDepartments(pgNumber, 10);
        }
      }
      GetDepartment(id:number){
        this.departmentService.getDepartmentById(id).subscribe({
          next:(response:MasterDepartment) => {
            this.dept = response;
            console.log(this.dept.departmentId);
            console.log(this.dept.departmentName);
            
          }, 
          error:(error) => {
            console.error('Error :(', error);
            this.errorMsg = JSON.stringify((error.message !== undefined)?error.error.title: error.message);
            Alert.toast(TYPE.ERROR,true,this.errorMsg);
        }});
      }
      editDepartment(id:number){
        let deptName = this.editDepartmentName.nativeElement.value;
        if(deptName !== ""){
          console.log(deptName);
          this.departmentService.updateDepartment(id,deptName).subscribe({
            next:(response:boolean)=>{
              if(response){
                Alert.toast(TYPE.SUCCESS,true,"Updated successfully");
                this.GetAllDepartments(1, 10);
              }
      
            },
            error:(error)=>{
              console.error('Error :(', error);
              this.errorMsg = JSON.stringify((error.message !== undefined)?error.error.title: error.message);
              Alert.toast(TYPE.ERROR,true,this.errorMsg);
            }
          });
        }
        else{
          Alert.toast(TYPE.ERROR,true,"Please enter the department name.");
        }
        this.closeEditApproverCollapses();
      }
      deleteDepartment(id:number){
        let askFirst:boolean = confirm("Are you sure you want to delete this department?");
        if(askFirst) {
          this.departmentService.deleteDepartment(id).subscribe({
            next:(response:boolean)=>{
              if(response){
                Alert.toast(TYPE.SUCCESS,true,"Deleted successfully");
                this.GetAllDepartments(1, 10);
              }
      
            },
            error:(error)=>{
              console.error('Error :(', error);
              this.errorMsg = JSON.stringify((error.message !== undefined)?error.error.title: error.message);
              Alert.toast(TYPE.ERROR,true,this.errorMsg);
            }
          });
        }
      }
      addDept:AddDepartmentDto = new AddDepartmentDto('');
      addDepartment(departmentForm:NgForm){
        this.addDept = departmentForm.value;
        console.log(departmentForm);
          
          this.departmentService.addDepartment(this.addDept.departmentName).subscribe({
            next: (response) => {
              Alert.bigToast('Success!','Department added successfully.',TYPE.SUCCESS,'Ok');
              departmentForm.resetForm();
              this.GetPage(this.maxPage);
            },
            error: (error) => {
              console.error('Error adding Department:', error);
              Alert.bigToast('Error!','There was an error adding the Department.', TYPE.ERROR, 'Try Again.');
            }
          });
       }
}
