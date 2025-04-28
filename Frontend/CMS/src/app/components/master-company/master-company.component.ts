import { Component, OnInit } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { Alert } from '../../utils/alert';
import { CompanyMasterService } from '../../services/company-master.service';
import { AddCompanyDto, CompanyListResponse, CompanyMasterDto } from '../../models/master-company';
import { Router } from '@angular/router';
import { TYPE } from '../auth/login/values.constants';
import { CommonModule } from '@angular/common';
import { LoaderComponent } from '../loader/loader.component';
import { Pagination } from '../../utils/pagination';

@Component({
  selector: 'app-master-company',
  standalone: true,
  imports: [CommonModule,LoaderComponent,FormsModule],
  templateUrl: './master-company.component.html',
  styleUrl: './master-company.component.css'
})
export class MasterCompanyComponent implements OnInit{
  loading: boolean = true;
  maxPage = 1;
  pageNumbers = [1, 1, 2, 3, 4, 5];
  masterCompany: CompanyListResponse[]=[];
  errorMsg:string = '';

  company: AddCompanyDto = new AddCompanyDto();

  constructor(
    private companyService: CompanyMasterService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.getCompanies(1, 10);
  }

  getCompanies(pageNumber: number, pageSize: number): void {
    this.companyService.getCompany(pageNumber, pageSize).subscribe({
      next:(res:CompanyListResponse[]) => {
      this.loading = false;
      this.masterCompany = res;
        if(this.masterCompany != undefined && this.masterCompany.length > 0){
                      let result = Pagination.paginator(pageNumber,this.masterCompany[0].totalCount,pageSize)
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

}
