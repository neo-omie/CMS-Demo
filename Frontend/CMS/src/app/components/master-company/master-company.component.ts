import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Alert } from '../../utils/alert';
import { CompanyMasterService } from '../../services/company-master.service';
import { AddCompanyDto, CompanyListResponse, CompanyMasterDto } from '../../models/master-company';
import { Router } from '@angular/router';
import { TYPE } from '../auth/login/values.constants';

@Component({
  selector: 'app-master-company',
  standalone: true,
  imports: [],
  templateUrl: './master-company.component.html',
  styleUrl: './master-company.component.css'
})
export class MasterCompanyComponent implements OnInit{
  loading: boolean = true;
  maxPage = 1;
  pageNumbers = [1, 1, 2, 3, 4, 5];
  masterCompany?: CompanyListResponse;
  company: AddCompanyDto = new AddCompanyDto();

  constructor(
    private companyService: CompanyMasterService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.getCompanies(1, 10);
  }

  getCompanies(pageNumber: number, pageSize: number): void {
    this.companyService.getCompany(pageNumber, pageSize).subscribe((res) => {
      this.loading = false;
      this.masterCompany = res;

      this.pageNumbers[0] = pageNumber;

      if (this.masterCompany) {
        this.maxPage = Math.ceil(this.masterCompany.totalCount / pageSize);

        let diff = this.maxPage - pageNumber;
        if (diff >= 0 && this.maxPage >= 5) {
          if (this.pageNumbers[0] == 1) {
            this.pageNumbers[1] = pageNumber;
            this.pageNumbers[2] = pageNumber + 1;
            this.pageNumbers[3] = pageNumber + 2;
            this.pageNumbers[4] = pageNumber + 3;
            this.pageNumbers[5] = pageNumber + 4;
          } else if (this.pageNumbers[0] == 2) {
            this.pageNumbers[1] = pageNumber - 1;
            this.pageNumbers[2] = pageNumber;
            this.pageNumbers[3] = pageNumber + 1;
            this.pageNumbers[4] = pageNumber + 2;
            this.pageNumbers[5] = pageNumber + 3;
          } else if (diff == 0) {
            this.pageNumbers[1] = pageNumber - 4;
            this.pageNumbers[2] = pageNumber - 3;
            this.pageNumbers[3] = pageNumber - 2;
            this.pageNumbers[4] = pageNumber - 1;
            this.pageNumbers[5] = pageNumber;
          } else if (diff == 1) {
            this.pageNumbers[1] = pageNumber - 3;
            this.pageNumbers[2] = pageNumber - 2;
            this.pageNumbers[3] = pageNumber - 1;
            this.pageNumbers[4] = pageNumber;
            this.pageNumbers[5] = pageNumber + 1;
          } else {
            this.pageNumbers[1] = pageNumber - 2;
            this.pageNumbers[2] = pageNumber - 1;
            this.pageNumbers[3] = pageNumber;
            this.pageNumbers[4] = pageNumber + 1;
            this.pageNumbers[5] = pageNumber + 2;
          }
        }
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
