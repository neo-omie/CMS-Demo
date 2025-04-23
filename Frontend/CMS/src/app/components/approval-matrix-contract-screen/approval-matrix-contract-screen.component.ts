import { Component, OnInit, QueryList, ElementRef, Renderer2, ViewChild } from '@angular/core';
import { ApproverMatrixContractService } from '../../services/approver-matrix-contract.service';
import { ApprovalMatrixContract } from '../../models/approval-matrix-contract';
import { CommonModule } from '@angular/common';
import { LoaderComponent } from '../loader/loader.component';

@Component({
  selector: 'app-approval-matrix-contract-screen',
  standalone: true,
  imports: [CommonModule,LoaderComponent],
  templateUrl: './approval-matrix-contract-screen.component.html',
  styleUrl: './approval-matrix-contract-screen.component.css'
})
export class ApprovalMatrixContractScreenComponent implements OnInit {
  approvers1:string[] = [];
  approvers2:string[] = [];
  approvers3:string[] = [];
  loading:boolean = true;
  pageNumbers:number[] = [1,1,2,3,4,5];
  maxPage:number = 1;
  approvalMatrixContracts:ApprovalMatrixContract[] = [];
  approvalMatrixContract?:ApprovalMatrixContract;
  errorMsg ?: string
  @ViewChild('editApproverCollapse1') editApproverCollapse1!: ElementRef;
  @ViewChild('editApproverCollapse2') editApproverCollapse2!: ElementRef;
  @ViewChild('editApproverCollapse3') editApproverCollapse3!: ElementRef;
  constructor(private approverMatrixContractService : ApproverMatrixContractService, private renderer : Renderer2){}
  ngOnInit(){
    this.GetApprovalMatrixContract(1 ,10);
  }
  closeEditApproverCollapses() {
      this.renderer.removeClass(this.editApproverCollapse1.nativeElement,'show');
      this.renderer.removeClass(this.editApproverCollapse2.nativeElement,'show');
      this.renderer.removeClass(this.editApproverCollapse3.nativeElement,'show');
  }
  GetApprovalMatrixContract(pageNumber : number, pageSize : number){
    this.approverMatrixContractService.GetApprovalMatrixContract(pageNumber, pageSize).subscribe({
      next:(response:ApprovalMatrixContract[]) => {
        this.loading = false;
        this.approvalMatrixContracts = response;
        this.pageNumbers[0] = pageNumber;
        if(this.approvalMatrixContracts.length > 0){
          if(pageNumber == 1){
            this.maxPage = Math.ceil(this.approvalMatrixContracts[0].totalRecords / 10);
          }
          let diff = this.maxPage - pageNumber;
          if(diff >= 0 && this.maxPage >= 5){
            if(this.pageNumbers[0] == 1){
              this.pageNumbers[1] = pageNumber;
              this.pageNumbers[2] = pageNumber + 1;
              this.pageNumbers[3] = pageNumber + 2;
              this.pageNumbers[4] = pageNumber + 3;
              this.pageNumbers[5] = pageNumber + 4;
            }
            else if(this.pageNumbers[0] == 2){
              this.pageNumbers[1] = pageNumber - 1;
              this.pageNumbers[2] = pageNumber;
              this.pageNumbers[3] = pageNumber + 1;
              this.pageNumbers[4] = pageNumber + 2;
              this.pageNumbers[5] = pageNumber + 3;
            }
            else if(diff == 0){
              this.pageNumbers[1] = pageNumber - 4;
              this.pageNumbers[2] = pageNumber - 3;
              this.pageNumbers[3] = pageNumber - 2;
              this.pageNumbers[4] = pageNumber - 1;
              this.pageNumbers[5] = pageNumber;
            }
            else if(diff == 1){
              this.pageNumbers[1] = pageNumber - 3;
              this.pageNumbers[2] = pageNumber - 2;
              this.pageNumbers[3] = pageNumber - 1;
              this.pageNumbers[4] = pageNumber;
              this.pageNumbers[5] = pageNumber + 1;
            }
            else{
              this.pageNumbers[1] = pageNumber - 2;
              this.pageNumbers[2] = pageNumber - 1;
              this.pageNumbers[3] = pageNumber;
              this.pageNumbers[4] = pageNumber + 1;
              this.pageNumbers[5] = pageNumber + 2;
            }
          }
        }
      }, 
      error:(error) => {
        this.loading = false;
        console.error('Error :(', error);
        if(error.message !== undefined){
          this.errorMsg = JSON.stringify(error.error.message);
          console.log(this.errorMsg);
        }
        else{
          this.errorMsg = JSON.stringify(error.message);
          console.log(this.errorMsg);
        }
      }
    });
  }
  GetPage(pgNumber:number){
    if(this.maxPage >= pgNumber && pgNumber >= 1){
      this.GetApprovalMatrixContract(pgNumber, 10);
    }
  }
  GetContract(id:number){
    this.approverMatrixContractService.GetApprovalMatrixContractById(id).subscribe({
      next:(response:ApprovalMatrixContract) => {
        this.approvalMatrixContract = response;
        console.log(response)
      }, 
      error:(error) => {
        console.error('Error :(', error);
        if(error.message !== undefined){
          this.errorMsg = JSON.stringify(error.error.message);
          console.log(this.errorMsg);
        }
        else{
          this.errorMsg = JSON.stringify(error.message);
          console.log(this.errorMsg);
        }
    }});
  }
  textChangeApprover1(departmentId:number, input:string){

  }
  textChangeApprover2(departmentId:number, input:string){

  }
  textChangeApprover3(departmentId:number, input:string){

  }
}
