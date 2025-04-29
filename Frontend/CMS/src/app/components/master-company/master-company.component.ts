import { Component, OnInit } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import {  ElementRef, Renderer2, ViewChild } from '@angular/core';
import { Alert } from '../../utils/alert';
import { CompanyMasterService } from '../../services/company-master.service';
import { AddCompanyDto, CompanyListResponse, CompanyMasterDto, MasterCompany } from '../../models/master-company';
import { Router, RouterModule } from '@angular/router';
import { TYPE } from '../auth/login/values.constants';
import { CommonModule } from '@angular/common';
import { LoaderComponent } from '../loader/loader.component';
import { Pagination } from '../../utils/pagination';

@Component({
  selector: 'app-master-company',
  standalone: true,
  imports: [CommonModule,LoaderComponent,FormsModule,RouterModule],
  templateUrl: './master-company.component.html',
  styleUrl: './master-company.component.css'
})
export class MasterCompanyComponent implements OnInit{
  loading: boolean = true;
  maxPage = 1;
  pageNumbers = [1, 1, 2, 3, 4, 5];
  masterCompany: CompanyListResponse[]=[];
  showCompanies: CompanyMasterDto[]=[];
  comp?:MasterCompany;
  errorMsg:string = '';
  @ViewChild('editCompanyName') editCompanyName!: ElementRef;

  company: AddCompanyDto = new AddCompanyDto();

  constructor(
    private companyService: CompanyMasterService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.getCompanies(1, 10);
  }

  getCompanies(pageNumber: number, pageSize: number): void {
    this.companyService.getCompany('t', pageNumber, pageSize).subscribe({
      next:(res:CompanyMasterDto[]) => {
        console.log(res);
        
      this.loading = false;
      this.showCompanies = res;
        if(this.showCompanies != undefined && this.showCompanies.length > 0){
                      let result = Pagination.paginator(pageNumber,this.showCompanies[0].TotalRecords,pageSize)
                      this.maxPage = result.maxPage;
                      this.pageNumbers = result.pageNumbers;
                    }
      }, error:(error) => {
        this.loading = false;
        console.error('Error :(', error);
        this.errorMsg = JSON.stringify((error.message !== undefined)?error.error.title: error.message);
        Alert.toast(TYPE.ERROR,true,this.errorMsg);
      }
      
      
    
    });
  }

  GetPage(pgNumber: number) {
    if (this.maxPage >= pgNumber && pgNumber >= 1) {
      this.getCompanies(pgNumber, 10);
    }
  }

  GetCompany(id:number){
    this.companyService.getCompanyById(id).subscribe({
      next:(res:MasterCompany)=>{
      this.comp=res;
      },
      error:(error)=>{
        console.error('Error :(', error);
        this.errorMsg = JSON.stringify((error.message !== undefined)?error.error.title: error.message);
        Alert.toast(TYPE.ERROR,true,this.errorMsg);
      }
        })
  }

  editCompany(id:number){
    let compName=this.editCompanyName.nativeElement.value;
    if (compName !=="") {
      console.log(compName);
      this.companyService.updateCompany(id,compName).subscribe({
        next:(res:boolean)=>{
          if (res) {
            Alert.toast(TYPE.SUCCESS,true,"Updated Successfully")
            this.getCompanies(1,10);
          }
        },
        error:(error)=>{
          console.error('Error :(', error);
              this.errorMsg = JSON.stringify((error.message !== undefined)?error.error.title: error.message);
              Alert.toast(TYPE.ERROR,true,this.errorMsg);
        }
      })
      
    }
  }

  addCompany(companyForm: NgForm) {
    this.company = companyForm.value;

    this.companyService.addCompany(this.company).subscribe({
      next: (response) => {
        Alert.bigToast('Success!', 'Company added successfully.', TYPE.SUCCESS, 'Ok');
        companyForm.resetForm();
        this.GetPage(this.maxPage);
      },
      error: (error) => {
        console.error('Error adding Company:', error);
        Alert.bigToast('Error!', 'There was an error adding the Company.', TYPE.ERROR, 'Try Again');
      }
    });
  }

  deleteCompany(id:number){
    // let askFirst:boolean = confirm("Are you sure you want to delete this department?");
    Alert.confirmToast("Are you sure you want to delete this Company?",
                       "You won't be able to revert this!", TYPE.WARNING,
                       "Yes, delete it!",
                       "Deleted successfully!",
                       "Company has been deleted.", TYPE.SUCCESS,() => {
                        this.companyService.deleteCompany(id).subscribe({
                          next:(response:boolean)=>{
                            if(response){
                              Alert.toast(TYPE.SUCCESS,true,"Deleted successfully");
                              this.getCompanies(1, 10);
                            }
                    
                          },
                          error:(error)=>{
                            console.error('Error :(', error);
                            this.errorMsg = JSON.stringify((error.message !== undefined)?error.error.title: error.message);
                            Alert.toast(TYPE.ERROR,true,this.errorMsg);
                          }
                        });
                       });
  }

}
