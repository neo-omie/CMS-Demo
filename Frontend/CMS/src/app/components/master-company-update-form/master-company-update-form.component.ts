import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CompanyMasterService } from '../../services/company-master.service';
import { Router } from '@angular/router';
import { AddCompanyDto, MasterCompany } from '../../models/master-company';
import { Alert } from '../../utils/alert';
import { TYPE } from '../auth/login/values.constants';

@Component({
  selector: 'app-master-company-update-form',
  standalone: true,
  imports: [ReactiveFormsModule,CommonModule],
  templateUrl: './master-company-update-form.component.html',
  styleUrl: './master-company-update-form.component.css'
})
export class MasterCompanyUpdateFormComponent {
mode:any;
/**
 *
 */
constructor(private companyMasterService:CompanyMasterService,private route:Router) {}
 masterCompanyUpdateForm=new FormGroup({
  companyName:new FormControl('',[Validators.required]),
  pocName : new FormControl('',[Validators.required]),
    valueId : new FormControl(undefined),
    pocContactNumber : new FormControl('',[Validators.required,Validators.pattern('^[0-9]{10}$')]),
    companyStatus : new FormControl('1',[Validators.required,Validators.pattern('^(0|1)$')]),
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
 onUpdateFormSubmit(){
  if(this.masterCompanyUpdateForm.invalid){
    this.masterCompanyUpdateForm.markAllAsTouched();
    return;
  }
   else{
        const pocContactNumber = this.masterCompanyUpdateForm.value.pocContactNumber;
        const companyStatus = this.masterCompanyUpdateForm.value.companyStatus;
        const countryId = this.masterCompanyUpdateForm.value.countryId;
        const stateId = this.masterCompanyUpdateForm.value.stateId;
        const cityId = this.masterCompanyUpdateForm.value.cityId;
        const zipcode = this.masterCompanyUpdateForm.value.zipcode;
        const companyContactNo = this.masterCompanyUpdateForm.value.companyContactNo;
        const gSTno = this.masterCompanyUpdateForm.value.gSTno;
        const bankAccNo = this.masterCompanyUpdateForm.value.bankAccNo;
        const mSMERegistrationNo = this.masterCompanyUpdateForm.value.mSMERegistrationNo;
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
          const updateFormValues:AddCompanyDto = new AddCompanyDto();
          updateFormValues.valueId = 0;
          updateFormValues.companyName = this.masterCompanyUpdateForm.value.companyName;
          updateFormValues.pocName = this.masterCompanyUpdateForm.value.pocName;
          updateFormValues.pocContactNumber = Number(pocContactNumber);
          updateFormValues.companyStatus = Number(companyStatus) == 1? true : false;
          updateFormValues.pocEmailId =this.masterCompanyUpdateForm.value.pocEmailId;
          updateFormValues.companyAddressLine1 =this.masterCompanyUpdateForm.value.companyAddressLine1;
          updateFormValues.companyAddressLine2 =this.masterCompanyUpdateForm.value.companyAddressLine2;
          updateFormValues.companyAddressLine3 =this.masterCompanyUpdateForm.value.companyAddressLine3;
          updateFormValues.countryId = Number(countryId);
          updateFormValues.stateId = Number(stateId);
          updateFormValues.cityId = Number(cityId)
          updateFormValues.zipcode = Number(zipcode)
          updateFormValues.companyContactNo = Number(companyContactNo)
          updateFormValues.companyEmailId =this.masterCompanyUpdateForm.value.companyEmailId;
          updateFormValues.companyWebsiteUrl =this.masterCompanyUpdateForm.value.companyWebsiteUrl;
          updateFormValues.companyBankName =this.masterCompanyUpdateForm.value.companyBankName;
          updateFormValues.gSTno = Number(gSTno);
          updateFormValues.bankAccNo = Number(bankAccNo);
          updateFormValues.mSMERegistrationNo = Number(mSMERegistrationNo);
          updateFormValues.iFSCCode =this.masterCompanyUpdateForm.value.iFSCCode;
          updateFormValues.PanNo =this.masterCompanyUpdateForm.value.panNo;
          console.log(updateFormValues);
          this.companyMasterService.addCompany(updateFormValues).subscribe({
            next:(response:MasterCompany) => {
              if(response != undefined || response != null){
                Alert.toast(TYPE.SUCCESS,true,'Updated successfully');
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
          console.log("should not come here ", this.masterCompanyUpdateForm.value)
        }
      }
    }
    get companyName(){
      return this.masterCompanyUpdateForm.get('companyName');
    }
    get pocName(){
      return this.masterCompanyUpdateForm.get('pocName');
    }
    get pocContactNumber(){
      return this.masterCompanyUpdateForm.get('pocContactNumber');
    }
    get companyStatus(){
      return this.masterCompanyUpdateForm.get('companyStatus');
    }
    get pocEmailId(){
      return this.masterCompanyUpdateForm.get('pocEmailId');
    }
    get companyAddressLine1(){
      return this.masterCompanyUpdateForm.get('companyAddressLine1');
    }
    get countryId(){
      return this.masterCompanyUpdateForm.get('countryId');
    }
    get stateId(){
      return this.masterCompanyUpdateForm.get('stateId');
    }
    get cityId(){
      return this.masterCompanyUpdateForm.get('cityId');
    }
    get zipcode(){
      return this.masterCompanyUpdateForm.get('zipcode');
    }
    get companyContactNo(){
      return this.masterCompanyUpdateForm.get('companyContactNo');
    }
    get companyEmailId(){
      return this.masterCompanyUpdateForm.get('companyEmailId');
    }
    get companyWebsiteUrl(){
      return this.masterCompanyUpdateForm.get('companyWebsiteUrl');
    }
    get companyBankName(){
      return this.masterCompanyUpdateForm.get('companyBankName');
    }
    get gSTno(){
      return this.masterCompanyUpdateForm.get('gSTno');
    }
    get bankAccNo(){
      return this.masterCompanyUpdateForm.get('bankAccNo');
    }
    get mSMERegistrationNo(){
      return this.masterCompanyUpdateForm.get('mSMERegistrationNo');
    }
    get companiFSCCodeyName(){
      return this.masterCompanyUpdateForm.get('companiFSCCodeyName');
    }
    get iFSCCode(){
      return this.masterCompanyUpdateForm.get('iFSCCode');
    }
    get panNo(){
      return this.masterCompanyUpdateForm.get('panNo');
    }
  
    onClick(){
      this.route.navigate(['masters/companyMasters']);
    }
 }
 
  

