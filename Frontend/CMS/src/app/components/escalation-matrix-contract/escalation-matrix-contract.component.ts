import { Component, ElementRef, OnInit, Renderer2, ViewChild } from '@angular/core';
import { EscalationMatrixContract, GetMasterEscalationMatrixContractByIdDto, MasterEscalationMatrixContractDto, UpdateMatrixContractDto } from '../../models/escalation-matrix-contract';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { LoaderComponent } from '../loader/loader.component';
import { Router, RouterModule } from '@angular/router';
import { EscalationMatrixContractService } from '../../services/escalation-matrix-contract.service';
import { MasterEmployee } from '../../models/master-employee';
import { Alert } from '../../utils/alert';
import { TYPE } from '../auth/login/values.constants';
import { ApproverMatrixContractService } from '../../services/approver-matrix-contract.service';
import { Title } from '@angular/platform-browser';
import { ApprovalMatrixContract } from '../../models/approval-matrix-contract';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-escalation-matrix-contract',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterModule, LoaderComponent, MatTableModule,MatSortModule,MatFormFieldModule,MatInputModule],
  templateUrl: './escalation-matrix-contract.component.html',
  styleUrl: './escalation-matrix-contract.component.css',
})
export class EscalationMatrixContractComponent implements OnInit {
  displayedColumns: string[] = ['departmentName', 'escalation1', 'escalation2', 'escalation3', 'action'];
    dataSource = new MatTableDataSource<EscalationMatrixContract>();
    @ViewChild(MatSort) sort!: MatSort;
    ngAfterViewInit() {
      this.dataSource.sort = this.sort;
    }
  loading: boolean = true;
  approvers1:MasterEmployee[] = [];
  approvers2:MasterEmployee[] = [];
  approvers3:MasterEmployee[] = [];
    @ViewChild('editApproverCollapse1') editApproverCollapse1!: ElementRef;
    @ViewChild('editApproverCollapse2') editApproverCollapse2!: ElementRef;
    @ViewChild('editApproverCollapse3') editApproverCollapse3!: ElementRef;
    @ViewChild('editApproverName1') editApproverName1!: ElementRef;
    @ViewChild('editApproverName2') editApproverName2!: ElementRef;
    @ViewChild('editApproverName3') editApproverName3!: ElementRef;
    @ViewChild('editApproverId1') editApproverId1!: ElementRef;
    @ViewChild('editApproverId2') editApproverId2!: ElementRef;
    @ViewChild('editApproverId3') editApproverId3!: ElementRef;
    @ViewChild('editNumberOfDays1') editNumberOfDays1!: ElementRef;
    @ViewChild('editNumberOfDays2') editNumberOfDays2!: ElementRef;
    @ViewChild('editNumberOfDays3') editNumberOfDays3!: ElementRef;
  maxPage = 1;
  pageNumbers = [1, 1, 2, 3, 4, 5];
  // matrixContracts : MasterEscalationMatrixContractDto []=;
  matrixContracts?: MasterEscalationMatrixContractDto;
  escalationMatrixContract? : GetMasterEscalationMatrixContractByIdDto;
  updateMatrixContract: UpdateMatrixContractDto = new UpdateMatrixContractDto('','','','','','',0,0,0);
  errorMsg ?: string

  ngOnInit(): void {
    this.getMatrixContracts(1, 10);
  }
  constructor(
    private escalationService: EscalationMatrixContractService,
    private router: Router,
    private renderer : Renderer2,
    private approverMatrixContractService : ApproverMatrixContractService,
    private title:Title
  ) {
    this.title.setTitle("Escalation Matrix (Contract) - CMS");
  }

  closeEditApproverCollapses() {
    this.renderer.removeClass(this.editApproverCollapse1.nativeElement,'show');
    this.renderer.removeClass(this.editApproverCollapse2.nativeElement,'show');
    this.renderer.removeClass(this.editApproverCollapse3.nativeElement,'show');
    this.escalationMatrixContract = undefined;
    this.approvers1.length = 0;
    this.approvers2.length = 0;
    this.approvers3.length = 0;
  }
  getMatrixContracts(pageNumber: number, pageSize: number) {
    this.escalationService
      .getAllMatrixContract(pageNumber, pageSize)
      .subscribe((res) => {
        this.loading = false;
        this.dataSource.data = res.getEscalationMatrixContractDto;
        if (this.sort) {
          this.dataSource.sort = this.sort;
        }
        this.matrixContracts = res;
        this.pageNumbers[0] = pageNumber;
        console.log(res.getEscalationMatrixContractDto);
        if (this.matrixContracts != undefined && this.matrixContracts.getEscalationMatrixContractDto != undefined && this.matrixContracts.getEscalationMatrixContractDto.length > 0) {
          if (pageNumber == 1) {
            this.maxPage = Math.ceil(this.matrixContracts.totalCount / 10);
          }
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

  GetPage(pgNumber:number){
    if(this.maxPage >= pgNumber && pgNumber >= 1){
      this.getMatrixContracts(pgNumber, 10);
    }
  }
  GetMatrixContractById(valueId:number){
    this.escalationService.getMatrixContractById(valueId).subscribe({
          next:(response:GetMasterEscalationMatrixContractByIdDto) => {
            this.escalationMatrixContract = response;
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
  textChangeApprover(departmentId:number, event:Event, approverNumber:number){
        let input = event.target as HTMLInputElement;
        this.approverMatrixContractService.GetApproversForInputText(departmentId,input.value).subscribe(
          {
            next:(response:MasterEmployee[]) => {
              if(approverNumber == 1){
                this.approvers1 = response;
              }
              else if(approverNumber == 2){
                this.approvers2 = response;
              }
              else if(approverNumber == 3){
                this.approvers3 = response;
              }
            },
            error:(error) => {
              console.error('Error :(', error);
              this.errorMsg = JSON.stringify((error.message !== undefined)?error.error.title: error.message);
              Alert.toast(TYPE.ERROR,true,this.errorMsg);
            }
          }
        )
      }
      fillApprover(approverId:string, approverName:string, inputNumber:number){
        if(inputNumber == 1){
          const input = this.editApproverCollapse1.nativeElement.querySelector('input');
          input.value = "";
          this.approvers1.length = 0;
          this.renderer.removeClass(this.editApproverCollapse1.nativeElement,'show');
          this.editApproverName1.nativeElement.value = approverName;
          this.editApproverId1.nativeElement.value = approverId;
        }
        else if(inputNumber == 2){
          const input = this.editApproverCollapse2.nativeElement.querySelector('input');
          input.value = "";
          this.approvers2.length = 0;
          this.renderer.removeClass(this.editApproverCollapse2.nativeElement,'show');
          this.editApproverName2.nativeElement.value = approverName;
          this.editApproverId2.nativeElement.value = approverId;
        }
        else if(inputNumber == 3){
          const input = this.editApproverCollapse3.nativeElement.querySelector('input');
          input.value = "";
          this.approvers3.length = 0;
          this.renderer.removeClass(this.editApproverCollapse3.nativeElement,'show');
          this.editApproverName3.nativeElement.value = approverName;
          this.editApproverId3.nativeElement.value = approverId;
        }
      }
      editApproverMatrixContractSubmit(id:number){
        let nod1 = this.editNumberOfDays1.nativeElement.value;
        let nod2 = this.editNumberOfDays2.nativeElement.value;
        let nod3 = this.editNumberOfDays3.nativeElement.value;
        if(this.updateMatrixContract != undefined &&
        nod1 !== "" && Number(nod1) > 0 &&
        nod2 !== "" && Number(nod2) > 0 &&
        nod3 !== "" && Number(nod3) > 0
      ){
          this.updateMatrixContract.escalationId1 = this.editApproverId1.nativeElement.value;
          this.updateMatrixContract.escalationId2 = this.editApproverId2.nativeElement.value;
          this.updateMatrixContract.escalationId3 = this.editApproverId3.nativeElement.value;
          this.updateMatrixContract.triggerDaysEscalation1 = nod1;
          this.updateMatrixContract.triggerDaysEscalation2 = nod2;
          this.updateMatrixContract.triggerDaysEscalation3 = nod3;
          this.escalationService.postMatrixContractById(id,this.updateMatrixContract).subscribe({
            next:(response:any)=>{
              Alert.toast(TYPE.SUCCESS,true,response.message);
              this.getMatrixContracts(1, 10);
            },
            error:(error)=>{
              console.error('Error :(', error);
              this.errorMsg = JSON.stringify((error.message !== undefined)?error.error.title: error.message);
              Alert.toast(TYPE.ERROR,true,this.errorMsg);
            }
          })
        }
        else{
          console.log(this.updateMatrixContract);
          Alert.toast(TYPE.ERROR,true,"Incorrect number of days");
        }
        this.closeEditApproverCollapses();
      }
  
}
