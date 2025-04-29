import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AddCompanyDto, MasterCompany } from '../../models/master-company';
import { Alert } from '../../utils/alert';
import { TYPE } from '../auth/login/values.constants';
import { CompanyMasterService } from '../../services/company-master.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-master-company-add-form',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './master-company-add-form.component.html',
  styleUrl: './master-company-add-form.component.css'
})
export class MasterCompanyAddFormComponent {
  mode:any;
  constructor(private  companyMasterService:CompanyMasterService,private route:Router) {}
  masterCompanyAddForm = new FormGroup({
    companyName : new FormControl('',[Validators.required]),
    pocName : new FormControl('',[Validators.required]),
    valueId : new FormControl(undefined),
    pocContactNumber : new FormControl('',[Validators.required,Validators.pattern('^[0-9]{10}$')]),
    companyStatus : new FormControl('',[Validators.required,Validators.pattern('^(0|1)$')]),
    pocEmailId : new FormControl('',[Validators.required,Validators.email]),
    companyAddressLine1 : new FormControl('',[Validators.required]),
    companyAddressLine2 : new FormControl(''),
    companyAddressLine3 : new FormControl(''),
    countryId : new FormControl('',[Validators.required]),
    stateId : new FormControl('',[Validators.required]),
    cityId : new FormControl('',[Validators.required]),
    zipcode : new FormControl('',[Validators.required]),
    companyContactNo : new FormControl('',[Validators.required,Validators.pattern('^[0-9]{10}$')]),
    companyEmailId : new FormControl('',[Validators.required,Validators.email]),
    companyWebsiteUrl : new FormControl('',[Validators.required]),
    companyBankName : new FormControl('',[Validators.required]),
    gSTno : new FormControl('',[Validators.required]),
    bankAccNo : new FormControl('',[Validators.required]),
    mSMERegistrationNo : new FormControl('',[Validators.required]),
    iFSCCode : new FormControl('',[Validators.required]),
    panNo : new FormControl('',[Validators.required])
  })
  errorMsg?: string 
  onAddFormSubmit(){
    if(this.masterCompanyAddForm.invalid){
      this.masterCompanyAddForm.markAllAsTouched();
      return;
    }
    else{
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
      if(pocContactNumber && Number(pocContactNumber) &&
        companyStatus && Number(companyStatus) &&
        countryId && Number(countryId) &&
        stateId && Number(stateId) &&
        cityId && Number(cityId) &&
        zipcode && Number(zipcode) &&
        companyContactNo && Number(companyContactNo) &&
        gSTno && Number(gSTno) &&
        bankAccNo && Number(bankAccNo) &&
        mSMERegistrationNo && Number(mSMERegistrationNo) 
      ){
        const addFormValues:AddCompanyDto = new AddCompanyDto();
        addFormValues.valueId = 0;
        addFormValues.companyName = this.masterCompanyAddForm.value.companyName;
        addFormValues.pocName = this.masterCompanyAddForm.value.pocName;
        addFormValues.pocContactNumber = Number(pocContactNumber);
        addFormValues.companyStatus = Number(companyStatus) == 1? true : false;
        addFormValues.pocEmailId =this.masterCompanyAddForm.value.pocEmailId;
        addFormValues.companyAddressLine1 =this.masterCompanyAddForm.value.companyAddressLine1;
        addFormValues.companyAddressLine2 =this.masterCompanyAddForm.value.companyAddressLine2;
        addFormValues.companyAddressLine3 =this.masterCompanyAddForm.value.companyAddressLine3;
        addFormValues.countryId = Number(countryId);
        addFormValues.stateId = Number(stateId);
        addFormValues.cityId = Number(cityId)
        addFormValues.zipcode = Number(zipcode)
        addFormValues.companyContactNo = Number(companyContactNo)
        addFormValues.companyEmailId =this.masterCompanyAddForm.value.companyEmailId;
        addFormValues.companyWebsiteUrl =this.masterCompanyAddForm.value.companyWebsiteUrl;
        addFormValues.companyBankName =this.masterCompanyAddForm.value.companyBankName;
        addFormValues.gSTno = Number(gSTno);
        addFormValues.bankAccNo = Number(bankAccNo);
        addFormValues.mSMERegistrationNo = Number(mSMERegistrationNo);
        addFormValues.iFSCCode =this.masterCompanyAddForm.value.iFSCCode;
        addFormValues.PanNo =this.masterCompanyAddForm.value.panNo;
        console.log(addFormValues);
        this.companyMasterService.addCompany(addFormValues).subscribe({
          next:(response:MasterCompany) => {
            if( response.valueId !== undefined && response.valueId > 0){
              Alert.toast(TYPE.SUCCESS,true,'Added successfully');
              this.route.navigate(['masters/companyMasters'])
            }
          }, 
          error:(error) => {
            console.error('Error :(', error);
            this.errorMsg = JSON.stringify((error.message !== undefined)?error.error.title: error.message);
            Alert.toast(TYPE.ERROR,true,this.errorMsg);
          }
        });
      }
      else{
        console.log("should not come here ", this.masterCompanyAddForm.value)
      }
    }
  }
  get companyName(){
    return this.masterCompanyAddForm.get('companyName');
  }
  get pocName(){
    return this.masterCompanyAddForm.get('pocName');
  }
  get pocContactNumber(){
    return this.masterCompanyAddForm.get('pocContactNumber');
  }
  get companyStatus(){
    return this.masterCompanyAddForm.get('companyStatus');
  }
  get pocEmailId(){
    return this.masterCompanyAddForm.get('pocEmailId');
  }
  get companyAddressLine1(){
    return this.masterCompanyAddForm.get('companyAddressLine1');
  }
  get countryId(){
    return this.masterCompanyAddForm.get('countryId');
  }
  get stateId(){
    return this.masterCompanyAddForm.get('stateId');
  }
  get cityId(){
    return this.masterCompanyAddForm.get('cityId');
  }
  get zipcode(){
    return this.masterCompanyAddForm.get('zipcode');
  }
  get companyContactNo(){
    return this.masterCompanyAddForm.get('companyContactNo');
  }
  get companyEmailId(){
    return this.masterCompanyAddForm.get('companyEmailId');
  }
  get companyWebsiteUrl(){
    return this.masterCompanyAddForm.get('companyWebsiteUrl');
  }
  get companyBankName(){
    return this.masterCompanyAddForm.get('companyBankName');
  }
  get gSTno(){
    return this.masterCompanyAddForm.get('gSTno');
  }
  get bankAccNo(){
    return this.masterCompanyAddForm.get('bankAccNo');
  }
  get mSMERegistrationNo(){
    return this.masterCompanyAddForm.get('mSMERegistrationNo');
  }
  get companiFSCCodeyName(){
    return this.masterCompanyAddForm.get('companiFSCCodeyName');
  }
  get iFSCCode(){
    return this.masterCompanyAddForm.get('iFSCCode');
  }
  get panNo(){
    return this.masterCompanyAddForm.get('panNo');
  }

  onClick(){
    this.route.navigate(['masters/companyMasters']);
  }
}
