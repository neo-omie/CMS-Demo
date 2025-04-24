import { Component,OnInit } from '@angular/core';
import { LoaderComponent } from '../loader/loader.component';
import { CommonModule } from '@angular/common';
import { ControlContainer, FormsModule } from '@angular/forms';
import { MasterEmployee, MasterEmployeeDto } from '../../models/master-employee';
import { MasterEmployeeService } from '../../services/master-employee.service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-master-employee',
  standalone: true,
  imports: [CommonModule, LoaderComponent, FormsModule],
  templateUrl: './master-employee.component.html',
  styleUrl: './master-employee.component.css'
})
export class MasterEmployeeComponent implements OnInit{
loading=true;
employees: MasterEmployee[]=[];
paginatedEmployees:MasterEmployee[]=[];
totalEmployees:number=0;
totalPages:number=0;
currentPage:number=1;
pageSize:number=10;
selectedUnit: string = 'All';
searchTerm: string = '';


constructor(private employeeService: MasterEmployeeService){}

ngOnInit(): void {
  this.fetchEmployees();
}

fetchEmployees(){
  this.employeeService.getEmployees(this.currentPage, this.pageSize, this?.selectedUnit, this?.searchTerm)
  .subscribe(
    (response: MasterEmployeeDto) => {
      this.loading = false;
      console.log(response);
      this.employees = response.data;
      this.totalEmployees = response.totalCount;
      this.totalPages=Math.ceil(this.totalEmployees / this.pageSize);
      this.paginateEmployee();
    }
  );
}

paginateEmployee(){
  const startIndex=(this.currentPage-1)*this.pageSize;
  const endIndex=startIndex + this.pageSize;
  this.paginatedEmployees=this.employees;
}


onPageChange(page:number){
  if(page>=1 && page <= this.totalPages){
    this.currentPage=page;
    console.log(this.currentPage);
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

onPreviousPage(){
  if(this.currentPage>1){
    this.onPageChange(this.currentPage-1);
  }
}

onNextPage(){
  if(this.currentPage<this.totalPages){
    this.onPageChange(this.currentPage+1);
  }
}

addEmployee(){
  
}

deleteEmployee(employee:MasterEmployee){
  if(confirm(`Are you sure you want to Delete ${employee.employeeName}?`)){
    this.employeeService.deleteEmployee(employee.valueId).subscribe(()=>{
      this.fetchEmployees();
    })
  }
}

viewEmployee(employee:MasterEmployee){

}

editEmployee(employee:MasterEmployee){

}

}
