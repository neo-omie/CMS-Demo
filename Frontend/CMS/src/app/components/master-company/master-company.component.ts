declare var bootstrap: any;
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, FormsModule, NgForm, ReactiveFormsModule, Validators } from '@angular/forms';
import { ElementRef, Renderer2, ViewChild } from '@angular/core';
import { Alert } from '../../utils/alert';
import { CompanyMasterService } from '../../services/company-master.service';
import { AddCompanyDto, CompanyListResponse, CompanyMasterDto, MasterCompany } from '../../models/master-company';
import { Router, RouterModule } from '@angular/router';
import { TYPE } from '../auth/login/values.constants';
import { CommonModule } from '@angular/common';
import { Pagination } from '../../utils/pagination';
import { Cities, Countriess, States } from '../../models/company-cascade';
import { CompanyCascadeService } from '../../services/company-cascade.service';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { LoaderComponent } from '../UtilComponents/loader/loader.component';
import { PDFExport } from '../../utils/pdfExport';
import { DecodeToken } from '../../utils/decodeToken';

@Component({
  selector: 'app-master-company',
  standalone: true,
  imports: [CommonModule, LoaderComponent, FormsModule, RouterModule, ReactiveFormsModule,
    MatTableModule, MatSortModule, MatFormFieldModule, MatInputModule],
  templateUrl: './master-company.component.html',
  styleUrl: './master-company.component.css'
})
export class MasterCompanyComponent implements OnInit {
  loading: boolean = true;
  displayedColumns: string[] = ['companyName', 'companyLocation', 'status', 'action'];
  dataSource = new MatTableDataSource<CompanyMasterDto>();
  @ViewChild(MatSort) sort!: MatSort;
  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
  }
  maxPage = 1;
  pageNumbers = [1, 1, 2, 3, 4, 5];
  masterCompany: CompanyListResponse[] = [];
  showCompanies: CompanyMasterDto[] = [];
  comp?: MasterCompany;
  errorMsg: string = '';
  searchTerm: string = '';
  pageNumber: number = 1;
  pageSize: number = 10;
  @ViewChild('editCompanyName') editCompanyName!: ElementRef;

  countryList: Countriess[] = [];
  stateList: States[] = [];
  cityList: Cities[] = [];
  selectedCountryId: number = 0;
  selectedStateId: number = 0;
  company: AddCompanyDto = new AddCompanyDto();
  mode: any
  constructor(
    private companyService: CompanyMasterService,
    private router: Router,
    private companyCascadeService: CompanyCascadeService
  ) { }

  ngOnInit(): void {
    this.getCompanies();
    this.getCountryCascade();
  }
  getCountryCascade() {
    this.companyCascadeService.getCountries().subscribe({
      next: (response: Countriess[]) => {
        this.countryList = response;
        console.log(this.countryList);

      }, error: (error) => {
        console.error('Error :(', error);
        this.errorMsg = JSON.stringify((error.message !== undefined) ? error.error.title : error.message);
        Alert.toast(TYPE.ERROR, true, this.errorMsg);
      }
    });
  }
  getStateCascade(event: Event) {
    let input = event.target as HTMLInputElement
    this.companyCascadeService.getStates(+input.value).subscribe({
      next: (response: States[]) => {
        this.stateList = response;
      }, error: (error) => {
        console.error('Error :(', error);
        this.errorMsg = JSON.stringify((error.message !== undefined) ? error.error.title : error.message);
        Alert.toast(TYPE.ERROR, true, this.errorMsg);
      }
    })
  }
  getCityCascade(event: Event) {
    let input = event.target as HTMLInputElement
    this.companyCascadeService.getCities(+input.value).subscribe({
      next: (response: Cities[]) => {
        this.cityList = response;
      }, error: (error) => {
        console.error('Error :(', error);
        this.errorMsg = JSON.stringify((error.message !== undefined) ? error.error.title : error.message);
        Alert.toast(TYPE.ERROR, true, this.errorMsg);
      }
    })
  }


  getCompanies(): void {
    this.companyService.getCompany(this?.searchTerm, this.pageNumber, this.pageSize)
      .subscribe({
        next: (res: CompanyMasterDto[]) => {
          this.loading = false;
          this.dataSource.data = res;
          if (this.sort) {
            this.dataSource.sort = this.sort;
          }
          this.showCompanies = res;
          console.log(res);
          if (this.showCompanies != undefined && this.showCompanies.length > 0) {
            let result = Pagination.paginator(this.pageNumber, this.showCompanies[0].TotalRecords, this.pageSize)
            this.maxPage = result.maxPage;
            this.pageNumbers = result.pageNumbers;
          }
        }, error: (error) => {
          this.loading = false;
          console.error('Error :(', error);
          this.errorMsg = JSON.stringify((error.message !== undefined) ? error.error.title : error.message);
          Alert.toast(TYPE.ERROR, true, this.errorMsg);
        }



      });
  }

  onFilterChange() {
    this.pageNumber = 1;
    this.getCompanies();
  }

  GetPage(pgNumber: number) {
    if (this.maxPage >= pgNumber && pgNumber >= 1) {
      this.getCompanies();
    }
  }
  countryName?: string;
  stateName?: string;
  cityName?: string;
  GetCompany(id: number) {
    console.log("ftech id", id);
    this.companyService.getCompanyById(id).subscribe({
      next: (res: MasterCompany) => {
        this.comp = res;
        this.companyCascadeService.getCountryById(res.countryId).subscribe((resp) => {
          this.countryName = resp.countries;
        });
        this.companyCascadeService.getStateById(res.stateId).subscribe((resp) => {
          this.stateName = resp.state;
        });
        this.companyCascadeService.getCityById(res.cityId).subscribe((resp) => {
          this.cityName = resp.city;
        });
      },
      error: (error) => {
        console.error('Error :(', error);
        this.errorMsg = JSON.stringify((error.message !== undefined) ? error.error.title : error.message);
        Alert.toast(TYPE.ERROR, true, this.errorMsg);
      }
    })
  }


  // editCompany(id:number){
  //   let compName=this.editCompanyName.nativeElement.value;
  //   if (compName !=="") {
  //     console.log(compName);
  //     this.companyService.updateCompany(id,compName).subscribe({
  //       next:(res:boolean)=>{
  //         if (res) {
  //           Alert.toast(TYPE.SUCCESS,true,"Updated Successfully")
  //           this.getCompanies();
  //         }
  //       },
  //       error:(error)=>{
  //         console.error('Error :(', error);
  //             this.errorMsg = JSON.stringify((error.message !== undefined)?error.error.title: error.message);
  //             Alert.toast(TYPE.ERROR,true,this.errorMsg);
  //       }
  //     })

  //   }
  // }

  addCompany(companyForm: NgForm) {
    this.company = companyForm.value;
    let empCode = DecodeToken.ECode;

    this.companyService.addCompany(this.company, empCode).subscribe({
      next: (response) => {
        Alert.bigToast('Success!', 'Company added successfully.', TYPE.SUCCESS, 'Ok');
        companyForm.resetForm();
        this.GetPage(this.maxPage);
        const modalElement = document.getElementById('company-add');
        if (modalElement) {
          const modalInstance = bootstrap.Modal.getInstance(modalElement) || new bootstrap.Modal(modalElement);
          modalInstance.hide();
        }
      },

      error: (error) => {
        console.error('Error adding Company:', error);
        Alert.bigToast('Error!', 'There was an error adding the Company.', TYPE.ERROR, 'Try Again');
      }
    });
  }



  deleteCompany(id: number) {
    let empCode = DecodeToken.ECode;
    // let askFirst:boolean = confirm("Are you sure you want to delete this department?");
    Alert.confirmToast("Are you sure you want to delete this Company?",
      "You won't be able to revert this!", TYPE.WARNING,
      "Yes, delete it!",
      "Deleted successfully!",
      "Company has been deleted.", TYPE.SUCCESS, () => {
        this.companyService.deleteCompany(id, empCode).subscribe({
          next: (response: boolean) => {
            if (response) {
              // Alert.toast(TYPE.SUCCESS,true,"Deleted successfully");
              this.getCompanies();
            }

          },
          error: (error) => {
            console.error('Error :(', error);
            this.errorMsg = JSON.stringify((error.message !== undefined) ? error.error.title : error.message);
            Alert.toast(TYPE.ERROR, true, this.errorMsg);
          }
        });
      });
  }
  // editCompany(comp:CompanyMasterDto){
  //   console.log('Navigating to editContract with valueId:', comp.valueId);
  //   this.router.navigate(['masters/companyMasters/updateCompany', comp.valueId]);
  // }

  masterCompanyAddForm = new FormGroup({
    companyName: new FormControl('', [Validators.required, Validators.maxLength(30), Validators.pattern('^[A-Za-z0-9 ]+$')]),
    pocName: new FormControl('', [Validators.required, Validators.maxLength(40), Validators.pattern('^[A-Za-z ]+$')]),
    valueId: new FormControl(undefined),
    pocContactNumber: new FormControl('', [Validators.required, Validators.pattern('^[0-9]{10}$'), Validators.maxLength(10)]),
    companyStatus: new FormControl('1', [Validators.required, Validators.pattern('^(0|1)$')]),
    pocEmailId: new FormControl('', [Validators.required, Validators.email, Validators.maxLength(70)]),
    companyAddressLine1: new FormControl('', [Validators.required, Validators.maxLength(70)]),
    companyAddressLine2: new FormControl('', [Validators.maxLength(50)]),
    companyAddressLine3: new FormControl('', [Validators.maxLength(50)]),
    countryId: new FormControl('', [Validators.required]),
    stateId: new FormControl('', [Validators.required]),
    cityId: new FormControl('', [Validators.required]),
    zipcode: new FormControl('', [Validators.required, Validators.pattern('^[0-9]{5}$')]),
    companyContactNo: new FormControl('', [Validators.required, Validators.pattern('^[0-9]{10}$')]),
    companyEmailId: new FormControl('', [Validators.required, Validators.email, Validators.maxLength(50)]),
    companyWebsiteUrl: new FormControl('', [Validators.required, Validators.pattern('^(https?:\\/\\/)?(www\\.)?[a-zA-Z0-9-]+(\\.[a-zA-Z]{2,})+$')]),
    companyBankName: new FormControl('', [Validators.required, Validators.maxLength(40), Validators.pattern('^[A-Za-z]+$')]),
    gSTno: new FormControl('', [Validators.required, Validators.pattern('^[0-9]{15}$')]),
    bankAccNo: new FormControl('', [Validators.required, Validators.pattern('^[0-9]{12,16}$')]),
    mSMERegistrationNo: new FormControl('', [Validators.required, Validators.pattern('^[0-9]{16}$')]),
    iFSCCode: new FormControl('', [Validators.required, Validators.pattern('^[A-Z]{4}0[A-Z0-9]{6}$')]),
    panNo: new FormControl('', [Validators.required, Validators.pattern('^[A-Z]{5}[0-9]{4}[A-Z]$')])
  })
  onAddFormSubmit() {
    let empCode = DecodeToken.ECode;
    if (this.masterCompanyAddForm.invalid) {
      this.masterCompanyAddForm.markAllAsTouched();
      return;
    }
    else {
      const pocContactNumber = this.masterCompanyAddForm.value.pocContactNumber;
      const companyStatus = this.masterCompanyAddForm.value.companyStatus;
      const countryId = this.masterCompanyAddForm.value.countryId;
      const stateId = this.masterCompanyAddForm.value.stateId;
      const cityId = this.masterCompanyAddForm.value.cityId;
      const zipcode = this.masterCompanyAddForm.value.zipcode;
      const companyContactNo = this.masterCompanyAddForm.value.companyContactNo;
      const gSTno = this.masterCompanyAddForm.value.gSTno;
      const bankAccNo = this.masterCompanyAddForm.value.bankAccNo;
      const mSMERegistrationNo = this.masterCompanyAddForm.value.mSMERegistrationNo;
      if (pocContactNumber && Number(pocContactNumber) &&
        countryId && Number(countryId) &&
        stateId && Number(stateId) &&
        cityId && Number(cityId) &&
        zipcode && Number(zipcode) &&
        companyContactNo && Number(companyContactNo) &&
        gSTno && Number(gSTno) &&
        bankAccNo && Number(bankAccNo) &&
        mSMERegistrationNo && Number(mSMERegistrationNo)
      ) {
        const addFormValues: AddCompanyDto = new AddCompanyDto();
        addFormValues.valueId = 0;
        addFormValues.companyName = this.masterCompanyAddForm.value.companyName;
        addFormValues.pocName = this.masterCompanyAddForm.value.pocName;
        addFormValues.pocContactNumber = Number(pocContactNumber);
        addFormValues.companyStatus = Number(companyStatus) == 1 ? true : false;
        addFormValues.pocEmailId = this.masterCompanyAddForm.value.pocEmailId;
        addFormValues.companyAddressLine1 = this.masterCompanyAddForm.value.companyAddressLine1;
        addFormValues.companyAddressLine2 = this.masterCompanyAddForm.value.companyAddressLine2;
        addFormValues.companyAddressLine3 = this.masterCompanyAddForm.value.companyAddressLine3;
        addFormValues.countryId = Number(countryId);
        addFormValues.stateId = Number(stateId);
        addFormValues.cityId = Number(cityId)
        addFormValues.zipcode = Number(zipcode)
        addFormValues.companyContactNo = Number(companyContactNo)
        addFormValues.companyEmailId = this.masterCompanyAddForm.value.companyEmailId;
        addFormValues.companyWebsiteUrl = this.masterCompanyAddForm.value.companyWebsiteUrl;
        addFormValues.companyBankName = this.masterCompanyAddForm.value.companyBankName;
        addFormValues.gSTno = Number(gSTno);
        addFormValues.bankAccNo = Number(bankAccNo);
        addFormValues.mSMERegistrationNo = Number(mSMERegistrationNo);
        addFormValues.iFSCCode = this.masterCompanyAddForm.value.iFSCCode;
        addFormValues.PanNo = this.masterCompanyAddForm.value.panNo;
        console.log(addFormValues);
        this.companyService.addCompany(addFormValues, empCode).subscribe({
          next: (response: MasterCompany) => {
            if (response != undefined || response != null) {
              Alert.toast(TYPE.SUCCESS, true, 'Added successfully');
              this.masterCompanyAddForm.reset({
                stateId: '',
                countryId: '',
                cityId: '',
              });
              this.getCompanies();
              const modalElement = document.getElementById('company-add');
              if (modalElement) {
                const modalInstance = bootstrap.Modal.getInstance(modalElement) || new bootstrap.Modal(modalElement);
                modalInstance.hide();
              }
            }
          },
          error: (error) => {
            console.error('Error :(', error);
            this.errorMsg = JSON.stringify((error.message !== undefined) ? error.error.title : error.message);
            Alert.toast(TYPE.ERROR, true, this.errorMsg);
          }
        });
      }
      else {
        console.log("should not come here ", this.masterCompanyAddForm.value)
      }
    }
  }

  ResettingonCacelButton() {
    this.masterCompanyAddForm.reset({
      stateId: '',
      countryId: '',
      cityId: '',
    });
  }
  get companyName() {
    return this.masterCompanyAddForm.get('companyName');
  }
  get pocName() {
    return this.masterCompanyAddForm.get('pocName');
  }
  get pocContactNumber() {
    return this.masterCompanyAddForm.get('pocContactNumber');
  }
  get companyStatus() {
    return this.masterCompanyAddForm.get('companyStatus');
  }
  get pocEmailId() {
    return this.masterCompanyAddForm.get('pocEmailId');
  }
  get companyAddressLine1() {
    return this.masterCompanyAddForm.get('companyAddressLine1');
  }
  get companyAddressLine2() {
    return this.masterCompanyAddForm.get('companyAddressLine2')
  }
  get companyAddressLine3() {
    return this.masterCompanyAddForm.get('companyAddressLine3')
  }
  get countryId() {
    return this.masterCompanyAddForm.get('countryId');
  }
  get stateId() {
    return this.masterCompanyAddForm.get('stateId');
  }
  get cityId() {
    return this.masterCompanyAddForm.get('cityId');
  }
  get zipcode() {
    return this.masterCompanyAddForm.get('zipcode');
  }
  get companyContactNo() {
    return this.masterCompanyAddForm.get('companyContactNo');
  }
  get companyEmailId() {
    return this.masterCompanyAddForm.get('companyEmailId');
  }
  get companyWebsiteUrl() {
    return this.masterCompanyAddForm.get('companyWebsiteUrl');
  }
  get companyBankName() {
    return this.masterCompanyAddForm.get('companyBankName');
  }
  get gSTno() {
    return this.masterCompanyAddForm.get('gSTno');
  }
  get bankAccNo() {
    return this.masterCompanyAddForm.get('bankAccNo');
  }
  get mSMERegistrationNo() {
    return this.masterCompanyAddForm.get('mSMERegistrationNo');
  }
  get companiFSCCodeyName() {
    return this.masterCompanyAddForm.get('companiFSCCodeyName');
  }
  get iFSCCode() {
    return this.masterCompanyAddForm.get('iFSCCode');
  }
  get panNo() {
    return this.masterCompanyAddForm.get('panNo');
  }

  onClick() {
    // this.router.navigate(['masters/companyMasters']);
    this.masterCompanyAddForm.reset();
  }
  compID: number = 0
  fetchCompanyData(companyID: number) {
    this.compID = companyID;
    this.companyService.getCompanyById(companyID).subscribe({
      next: (res: MasterCompany) => {
        this.masterCompanyAddForm.patchValue({
          companyName: res.companyName,
          companyStatus: String(Number(res.companyStatus)),
          pocName: res.pocName,
          pocContactNumber: String(res.pocContactNumber),
          pocEmailId: res.pocEmailId,
          companyAddressLine1: res.companyAddressLine1,
          companyAddressLine2: res.companyAddressLine2,
          companyAddressLine3: res.companyAddressLine3,
          countryId: String(res.countryId),
          stateId: String(res.stateId),
          cityId: String(res.cityId),
          zipcode: String(res.zipcode),
          companyContactNo: String(res.companyContactNo),
          companyEmailId: res.companyEmailId,
          companyWebsiteUrl: res.companyWebsiteUrl,
          companyBankName: res.companyBankName,
          gSTno: String(res.gsTno),
          bankAccNo: String(res.bankAccNo),
          mSMERegistrationNo: String(res.msmeRegistrationNo),
          iFSCCode: String(res.ifscCode),
          panNo: String(res.panNo)
        });
      }
    })
  }
  onUpdateFormSubmit(compID: number) {
    let empCode = DecodeToken.ECode;
    if (this.masterCompanyAddForm.invalid) {
      this.masterCompanyAddForm.markAllAsTouched();
      return;
    }
    else {
      const pocContactNumber = this.masterCompanyAddForm.value.pocContactNumber;
      const companyStatus = this.masterCompanyAddForm.value.companyStatus;
      const countryId = this.masterCompanyAddForm.value.countryId;
      const stateId = this.masterCompanyAddForm.value.stateId;
      const cityId = this.masterCompanyAddForm.value.cityId;
      const zipcode = this.masterCompanyAddForm.value.zipcode;
      const companyContactNo = this.masterCompanyAddForm.value.companyContactNo;
      const gSTno = this.masterCompanyAddForm.value.gSTno;
      const bankAccNo = this.masterCompanyAddForm.value.bankAccNo;
      const mSMERegistrationNo = this.masterCompanyAddForm.value.mSMERegistrationNo;
      if (pocContactNumber && Number(pocContactNumber) &&
        countryId && Number(countryId) &&
        stateId && Number(stateId) &&
        cityId && Number(cityId) &&
        zipcode && Number(zipcode) &&
        companyContactNo && Number(companyContactNo) &&
        gSTno && Number(gSTno) &&
        bankAccNo && Number(bankAccNo) &&
        mSMERegistrationNo && Number(mSMERegistrationNo)
      ) {
        const addFormValues: AddCompanyDto = new AddCompanyDto();
        addFormValues.valueId = 0;
        addFormValues.companyName = this.masterCompanyAddForm.value.companyName;
        addFormValues.pocName = this.masterCompanyAddForm.value.pocName;
        addFormValues.pocContactNumber = Number(pocContactNumber);
        addFormValues.companyStatus = Number(companyStatus) == 1 ? true : false;
        addFormValues.pocEmailId = this.masterCompanyAddForm.value.pocEmailId;
        addFormValues.companyAddressLine1 = this.masterCompanyAddForm.value.companyAddressLine1;
        addFormValues.companyAddressLine2 = this.masterCompanyAddForm.value.companyAddressLine2;
        addFormValues.companyAddressLine3 = this.masterCompanyAddForm.value.companyAddressLine3;
        addFormValues.countryId = Number(countryId);
        addFormValues.stateId = Number(stateId);
        addFormValues.cityId = Number(cityId)
        addFormValues.zipcode = Number(zipcode)
        addFormValues.companyContactNo = Number(companyContactNo)
        addFormValues.companyEmailId = this.masterCompanyAddForm.value.companyEmailId;
        addFormValues.companyWebsiteUrl = this.masterCompanyAddForm.value.companyWebsiteUrl;
        addFormValues.companyBankName = this.masterCompanyAddForm.value.companyBankName;
        addFormValues.gSTno = Number(gSTno);
        addFormValues.bankAccNo = Number(bankAccNo);
        addFormValues.mSMERegistrationNo = Number(mSMERegistrationNo);
        addFormValues.iFSCCode = this.masterCompanyAddForm.value.iFSCCode;
        addFormValues.PanNo = this.masterCompanyAddForm.value.panNo;
        console.log('Check changes', addFormValues);
        this.companyService.updateCompany(compID, addFormValues, empCode).subscribe({
          next: (response: MasterCompany) => {
            if (response != undefined || response != null) {
              Alert.toast(TYPE.SUCCESS, true, 'Updated successfully');
              this.masterCompanyAddForm.reset();
              this.getCompanies();
              const modalElement = document.getElementById('company-edit');
              if (modalElement) {
                const modalInstance = bootstrap.Modal.getInstance(modalElement) || new bootstrap.Modal(modalElement);
                modalInstance.hide();
              }
              this.masterCompanyAddForm.reset();
            }
          },
          error: (error) => {
            console.error('Error :(', error);
            this.errorMsg = JSON.stringify((error.message !== undefined) ? error.error.title : error.message);
            Alert.toast(TYPE.ERROR, true, this.errorMsg);
          }
        });
      }
      else {
        console.log("should not come here ", this.masterCompanyAddForm.value)
      }
    }
  }
  printToPDF(tableID: string, fileName: string) {
    PDFExport.printToPDF(tableID, fileName);
  }
}

