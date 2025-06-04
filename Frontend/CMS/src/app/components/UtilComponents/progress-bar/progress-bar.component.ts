import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { HelloComponent } from './hello.component';
import { CommonModule } from '@angular/common';
import { ContractsService } from '../../../services/contracts.service';
import { GetContractByIdDto } from '../../../models/contracts';
import { AddAddendumContractsService } from '../../../services/add-addendum-contracts.service';
import { AddAddendumContract } from '../../../models/add-addendum-contract';
import { HasApprovalStatuses } from '../../../models/shared/has-approval-statuses';
import { ClassifiedContractsService } from '../../../services/classified-contracts.service';
import { GetClassifiedContractByIdDto } from '../../../models/classified-contracts';

@Component({
  selector: 'app-progress-bar',
  standalone: true,
  imports: [FormsModule, MatButtonModule, MatIconModule, HelloComponent, CommonModule],
  templateUrl: './progress-bar.component.html',
  styleUrl: './progress-bar.component.css'
})
export class ProgressBarComponent implements OnInit, OnChanges{
  @Input() contractId!: number;
  @Input() phases: string[] = [];
  @Input() type: 'contract' | 'addendum' | 'classified'='contract';
  
  stepStatuses: number[] = [];

  contractDetails?: GetContractByIdDto;
  addAddendumContract?:AddAddendumContract;
  classifiedcontractDetails?:GetClassifiedContractByIdDto;

  ngOnInit(): void {
    if(this.contractId){
      this.loadProgress();
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    if(changes['contractId'] && changes['contractId'].currentValue){
      this.loadProgress();
    }
    if(changes['classifiedContractId'] && changes['classifiedContractId'].currentValue){
      this.loadProgress();
    }
  }

  constructor(private contractsService: ContractsService, private addAddendumContractsService: AddAddendumContractsService, private classifiedContractsService: ClassifiedContractsService){}

  loadProgress(){
    switch(this.type){
      case 'contract':
      this.fetchContractProgress(this.contractId);
      break;
      case 'addendum':
      this.fetchAddendumProgress(this.contractId); 
      break;
      case 'classified':
      this.fetchClassifiedProgress(this.contractId); 
      break;
    }
  }

  

  fetchContractProgress(contractId: number) {
  this.contractsService.getContractByID(contractId).subscribe({
    next: (response: GetContractByIdDto) => {
      this.contractDetails = response;
      this.setPhasesAndStep(response); 
    }
  });
  }

  fetchAddendumProgress(contractId: number){
  this.addAddendumContractsService.GetAddenduByAddendumId(contractId).subscribe({
    next:(addendumresponse:AddAddendumContract)=>{
      this.addAddendumContract= addendumresponse;
      this.setPhasesAndStep(addendumresponse);
    }
  })
  }

  fetchClassifiedProgress(contractId: number){
  this.classifiedContractsService.getContractByID(contractId).subscribe({
    next:(classifiedresponse:GetClassifiedContractByIdDto)=>{
      this.classifiedcontractDetails= classifiedresponse;
      this.setPhasesAndStep(classifiedresponse);
    }
  })
  }

  setPhasesAndStep(contract: HasApprovalStatuses) {

  if(contract!==undefined && contract.approver1Status!==undefined && contract.approver2Status!==undefined && contract.approver3Status!==undefined){
    this.stepStatuses=[
      2, 
      contract.approver1Status,
      contract.approver2Status,
      contract.approver3Status,
      this.calculateFinalStatus(contract),
    ];
  }else{
    this.stepStatuses=[
      2, 
      0,
      0,
      0,
      this.calculateFinalStatus(contract),
    ];
  }
}

calculateFinalStatus(contract:HasApprovalStatuses):number{
    if ([contract.approver1Status, contract.approver2Status, contract.approver3Status].includes(3)) {
    return 3; 
  }
  if (contract.approver3Status === 2) {
    return 2; 
  }
  return 0;
}

}
