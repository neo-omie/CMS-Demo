import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { ClassifiedContractsService } from '../../../services/classified-contracts.service';
import { AddClasifiedContractDto, ClassifiedContracts } from '../../../models/classified-contracts';
import { Alert } from '../../../utils/alert';
import { TYPE } from '../../auth/login/values.constants';

@Component({
  selector: 'app-create-classified-contract',
  standalone: true,
  imports: [ReactiveFormsModule,CommonModule ,RouterModule],
  templateUrl: './create-classified-contract.component.html',
  styleUrl: './create-classified-contract.component.css'
})
export class CreateClassifiedContractComponent {

mode:any;
  constructor(private  contractsService:ClassifiedContractsService,private route:Router) {}
  masterContractAddForm = new FormGroup({
    classifiedContractName : new FormControl('',[Validators.required]),
    departmentId : new FormControl('',[Validators.required,Validators.pattern('^[0-9]$')]),
    contractWithCompanyId : new FormControl('',[Validators.required,Validators.pattern('^[0-9]$')]),
    contractTypeId : new FormControl('',[Validators.required,Validators.pattern('^[0-9]$')]),
    apostilleTypeId : new FormControl('',[Validators.required,Validators.pattern('^[0-9]$')]),
    actualDocRefNo : new FormControl('',[Validators.required]),
    retainerContract : new FormControl('',[Validators.required,Validators.pattern('^[0-9]$')]),
    termsAndConditions : new FormControl('',[Validators.required]),
    validFrom : new FormControl('',[Validators.required]),
    validTill : new FormControl('',[Validators.required]),
    renewalFrom : new FormControl('',[Validators.required]),
    renewalTill : new FormControl('',[Validators.required]),
    addendumDate : new FormControl('',[Validators.required]),
    empCustodianId : new FormControl('',[Validators.required,Validators.pattern('^[0-9]$')]),
    location : new FormControl('',[Validators.required]),
    approver1Status : new FormControl('',[Validators.required,Validators.pattern('^[0-9]$')]),
    approver2Status : new FormControl('',[Validators.required,Validators.pattern('^[0-9]$')]),
    approver3Status : new FormControl('',[Validators.required,Validators.pattern('^[0-9]$')])
  })
  errorMsg?: string 
  onAddFormSubmit(){
    if(this.masterContractAddForm.invalid){
      this.masterContractAddForm.markAllAsTouched();
      return;
    }
    else{
      const departmentId = this.masterContractAddForm.value.departmentId;
      const contractWithCompanyId = this.masterContractAddForm.value.contractWithCompanyId;
      const contractTypeId = this.masterContractAddForm.value.contractTypeId;
      const apostilleTypeId = this.masterContractAddForm.value.apostilleTypeId;
      const actualDocRefNo = this.masterContractAddForm.value.actualDocRefNo;
      const retainerContract = this.masterContractAddForm.value.retainerContract;
      const empCustodianId = this.masterContractAddForm.value.empCustodianId;
      const approver1Status = this.masterContractAddForm.value.approver1Status;
      const approver2Status = this.masterContractAddForm.value.approver2Status;
      const approver3Status = this.masterContractAddForm.value.approver3Status;
      if(departmentId && Number(departmentId) &&
      contractWithCompanyId && Number(contractWithCompanyId) &&
      contractTypeId && Number(contractTypeId) &&
      apostilleTypeId && Number(apostilleTypeId) &&
      actualDocRefNo && Number(actualDocRefNo) &&
      retainerContract && Number(retainerContract) &&
      empCustodianId && Number(empCustodianId) &&
      approver1Status && Number(approver1Status) &&
      approver2Status && Number(approver2Status) &&
      approver3Status && Number(approver3Status) 
      ){
        const addFormValues:AddClasifiedContractDto = new AddClasifiedContractDto();
        addFormValues.classifiedContractName = this.masterContractAddForm.value.classifiedContractName;
        addFormValues.departmentId = Number(departmentId);
        addFormValues.contractWithCompanyId = Number(contractWithCompanyId);
        addFormValues.contractTypeId = Number(contractTypeId);
        addFormValues.apostilleTypeId = Number(apostilleTypeId);
        addFormValues.actualDocRefNo = Number(actualDocRefNo);
        addFormValues.retainerContract = Number(retainerContract);
        addFormValues.termsAndConditions = this.masterContractAddForm.value.termsAndConditions;
        addFormValues.validFrom = this.masterContractAddForm.value.validFrom;
        addFormValues.validTill = this.masterContractAddForm.value.validTill;
        addFormValues.renewalFrom = this.masterContractAddForm.value.renewalFrom;
        addFormValues.renewalTill = this.masterContractAddForm.value.renewalTill;
        addFormValues.addendumDate = this.masterContractAddForm.value.renewalTill;
        addFormValues.empCustodianId = Number(empCustodianId);
        addFormValues.location = this.masterContractAddForm.value.location;
        addFormValues.approver1Status = Number(approver1Status);
        addFormValues.approver2Status = Number(approver2Status);
        addFormValues.approver3Status = Number(approver3Status);
        console.log(addFormValues);
        this.contractsService.addContract(addFormValues).subscribe({
          next:(response:ClassifiedContracts) => {
            if( response.classifiedContractID !== undefined && response.classifiedContractID > 0){
              Alert.toast(TYPE.SUCCESS,true,'Added successfully');
              this.route.navigate(['classifiedContracts/allContracts'])
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
        console.log("should not come here ", this.masterContractAddForm.value)
      }
    }
  }
  get classifiedContractName(){
    return this.masterContractAddForm.get('classifiedContractName');
  }
  get departmentId(){
    return this.masterContractAddForm.get('departmentId');
  }
  get contractWithCompanyId(){
    return this.masterContractAddForm.get('contractWithCompanyId');
  }
  get contractTypeId(){
    return this.masterContractAddForm.get('contractTypeId');
  }
  get apostilleTypeId(){
    return this.masterContractAddForm.get('apostilleTypeId');
  }
  get actualDocRefNo(){
    return this.masterContractAddForm.get('actualDocRefNo');
  }
  get retainerContract(){
    return this.masterContractAddForm.get('retainerContract');
  }
  get termsAndConditions(){
    return this.masterContractAddForm.get('termsAndConditions');
  }
  get validFrom(){
    return this.masterContractAddForm.get('validFrom');
  }
  get validTill(){
    return this.masterContractAddForm.get('validTill');
  }
  get renewalFrom(){
    return this.masterContractAddForm.get('renewalFrom');
  }
  get renewalTill(){
    return this.masterContractAddForm.get('renewalTill');
  }
  get addendumDate(){
    return this.masterContractAddForm.get('addendumDate');
  }
  get empCustodianId(){
    return this.masterContractAddForm.get('empCustodianId');
  }
  get location(){
    return this.masterContractAddForm.get('location');
  }
  get approver1Status(){
    return this.masterContractAddForm.get('approver1Status');
  }
  get approver2Status(){
    return this.masterContractAddForm.get('approver2Status');
  }
  get approver3Status(){
    return this.masterContractAddForm.get('approver3Status');
  }

  onClick(){
    this.route.navigate(['contracts/allContracts']);
  }
}

