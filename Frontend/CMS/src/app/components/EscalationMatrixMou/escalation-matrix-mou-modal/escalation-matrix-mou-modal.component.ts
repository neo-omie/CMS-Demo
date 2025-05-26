declare var bootstrap: any;
import { CommonModule } from '@angular/common';
import { Component, ElementRef, Input, Renderer2, ViewChild } from '@angular/core';
import { MasterEmployee } from '../../../models/master-employee';
import { ApproverMatrixContractService } from '../../../services/approver-matrix-contract.service';
import { Alert } from '../../../utils/alert';
import { TYPE } from '../../auth/login/values.constants';
import { MasterEscalationMatrixMouDto, UpdateMatrixMouDto } from '../../../models/master-escalation-matrix-mou-dto';
import { EscalationMatrixMouService } from '../../../services/escalation-matrix-mou.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-escalation-matrix-mou-modal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './escalation-matrix-mou-modal.component.html',
  styleUrl: './escalation-matrix-mou-modal.component.css'
})
export class EscalationMatrixMouModalComponent {
  @Input() getMatrixMous!: (pageSize: number, pageNumber: number) => void;
  @Input() escalationMatrixMou?: MasterEscalationMatrixMouDto;
  @Input() isEdit: boolean = false;

  errorMsg: string = "";
  approvers1: MasterEmployee[] = [];
  approvers2: MasterEmployee[] = [];
  approvers3: MasterEmployee[] = [];

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

  constructor(
    private escalationService: EscalationMatrixMouService,
    private renderer: Renderer2, 
    private approverMatrixContractService: ApproverMatrixContractService,
    private route : Router
  ) { }

  closeEditApproverCollapses() {
    if(this.editApproverCollapse1 && this.editApproverCollapse2 && this.editApproverCollapse3){
      this.renderer.removeClass(this.editApproverCollapse1.nativeElement, 'show');
      this.renderer.removeClass(this.editApproverCollapse2.nativeElement, 'show');
      this.renderer.removeClass(this.editApproverCollapse3.nativeElement, 'show');
      this.approvers1.length = 0;
      this.approvers2.length = 0;
      this.approvers3.length = 0;
    }
    if(this.escalationMatrixMou){
      this.escalationMatrixMou.escalationId1 = "";
      this.escalationMatrixMou.escalationId2 = "";
      this.escalationMatrixMou.escalationId3 = "";
      this.escalationMatrixMou.escalation1 = "";
      this.escalationMatrixMou.escalation2 = "";
      this.escalationMatrixMou.escalation3 = "";
      this.escalationMatrixMou.departmentId = 0;
      this.escalationMatrixMou.departmentName = "";
      this.escalationMatrixMou.matrixMouId = 0;
      this.escalationMatrixMou.triggerDaysEscalation1 = 0;
      this.escalationMatrixMou.triggerDaysEscalation2 = 0;
      this.escalationMatrixMou.triggerDaysEscalation3 = 0;
      this.escalationMatrixMou.totalRecords = 0;
    }
  }

  textChangeApprover(departmentId: number, event: Event, approverNumber: number) {
    let input = event.target as HTMLInputElement;
    console.log("Department ID : ", departmentId)
    this.approverMatrixContractService.GetApproversForInputText(departmentId, input.value).subscribe(
      {
        next: (response: MasterEmployee[]) => {
          if (approverNumber == 1) {
            this.approvers1 = response;
          }
          else if (approverNumber == 2) {
            this.approvers2 = response;
          }
          else if (approverNumber == 3) {
            this.approvers3 = response;
          }
        },
        error: (error) => {
          console.error('Error :(', error);
          this.errorMsg = JSON.stringify((error.message !== undefined) ? error.error.title : error.message);
          Alert.toast(TYPE.ERROR, true, this.errorMsg);
        }
      }
    )
  }

  fillApprover(approverId: string, approverName: string, inputNumber: number) {
    if (inputNumber == 1) {
      const input = this.editApproverCollapse1.nativeElement.querySelector('input');
      input.value = "";
      this.approvers1.length = 0;
      this.renderer.removeClass(this.editApproverCollapse1.nativeElement, 'show');
      this.editApproverName1.nativeElement.value = approverName;
      this.editApproverId1.nativeElement.value = approverId;
    }
    else if (inputNumber == 2) {
      const input = this.editApproverCollapse2.nativeElement.querySelector('input');
      input.value = "";
      this.approvers2.length = 0;
      this.renderer.removeClass(this.editApproverCollapse2.nativeElement, 'show');
      this.editApproverName2.nativeElement.value = approverName;
      this.editApproverId2.nativeElement.value = approverId;
    }
    else if (inputNumber == 3) {
      const input = this.editApproverCollapse3.nativeElement.querySelector('input');
      input.value = "";
      this.approvers3.length = 0;
      this.renderer.removeClass(this.editApproverCollapse3.nativeElement, 'show');
      this.editApproverName3.nativeElement.value = approverName;
      this.editApproverId3.nativeElement.value = approverId;
    }
  }

  editApproverMatrixContractSubmit(id:number){
    let empCode = localStorage.getItem("empCode");
    if(empCode){
      let updateMatrixMouDto = new UpdateMatrixMouDto(0,'','','','',0,0,0);
      let nod1 = this.editNumberOfDays1.nativeElement.value;
      let nod2 = this.editNumberOfDays2.nativeElement.value;
      let nod3 = this.editNumberOfDays3.nativeElement.value;
      let ap1 = this.editApproverId1.nativeElement.value;
      let ap2 = this.editApproverId2.nativeElement.value;
      let ap3 = this.editApproverId3.nativeElement.value;
      if(nod1 !== "" && Number(nod1) > 0 &&
      nod2 !== "" && Number(nod2) > 0 &&
      nod3 !== "" && Number(nod3) > 0){
        updateMatrixMouDto.escalationId1 = ap1;
        updateMatrixMouDto.escalationId2 = ap2;
        updateMatrixMouDto.escalationId3 = ap3;
        updateMatrixMouDto.triggerDaysEscalation1 = nod1;
        updateMatrixMouDto.triggerDaysEscalation2 = nod2;
        updateMatrixMouDto.triggerDaysEscalation3 = nod3;
        this.escalationService.postMatrixMouById(id,updateMatrixMouDto,empCode).subscribe({
          next:(response:any)=>{
            Alert.toast(TYPE.SUCCESS,true,response.message);
            this.getMatrixMous(1, 10);
            const modalElement = document.getElementById('escalation-matrix-mou-modal');
            if (modalElement) {
              const modalInstance = bootstrap.Modal.getInstance(modalElement) || new bootstrap.Modal(modalElement);
              modalInstance.hide();
            }
            this.closeEditApproverCollapses();
          },
          error:(error)=>{
            console.error('Error :(', error);
            this.errorMsg = JSON.stringify((error.message !== undefined)?error.error.message: error.error.title);
            Alert.toast(TYPE.ERROR,true,this.errorMsg);
          }
        })
      }
      else{
        Alert.toast(TYPE.ERROR,true,"Incorrect number of days");
      }
    }
    else{
      localStorage.clear();
      this.route.navigate(["/"]);
    }
  }
}
