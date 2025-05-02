import { Component,OnInit } from '@angular/core';
import { LoaderComponent } from '../loader/loader.component';
import { CommonModule } from '@angular/common';
import { ControlContainer, FormsModule } from '@angular/forms';
import { MasterEmployee, MasterEmployeeDto } from '../../models/master-employee';
import { MasterEmployeeService } from '../../services/master-employee.service';
import { HttpClient } from '@angular/common/http';
import { Pagination } from '../../utils/pagination';
import { Alert } from '../../utils/alert';
import { TYPE } from '../auth/login/values.constants';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-master-employee',
  standalone: true,
  imports: [CommonModule, LoaderComponent, FormsModule,RouterModule],
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
errorMsg ?: string

constructor(private employeeService: MasterEmployeeService,private router: Router){}

ngOnInit(): void {
  this.fetchEmployees();
}

fetchEmployees(){
  this.employeeService.getEmployees(this.currentPage, this.pageSize, this?.selectedUnit, this?.searchTerm)
  .subscribe({
    next:(response: MasterEmployeeDto) => {
      this.loading = false;
      console.log(response);
      this.employees = response.data;
      this.totalEmployees = response.totalCount
      if(this.employees!= undefined && this.employees.length > 0){
        let result = Pagination.paginator(this.currentPage,this.totalEmployees,this.pageSize)
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

}
