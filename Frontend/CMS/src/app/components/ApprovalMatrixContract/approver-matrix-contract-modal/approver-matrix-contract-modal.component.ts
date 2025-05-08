import { Component, ElementRef, Input, Renderer2, ViewChild } from '@angular/core';
import { ApprovalMatrixContract, EditApprovalMatrixContractDto } from '../../../models/approval-matrix-contract';
import { MasterEmployee } from '../../../models/master-employee';
import { ApproverMatrixContractService } from '../../../services/approver-matrix-contract.service';
import { TYPE } from '../../auth/login/values.constants';
import { Alert } from '../../../utils/alert';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-approver-matrix-contract-modal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './approver-matrix-contract-modal.component.html',
  styleUrl: './approver-matrix-contract-modal.component.css'
})
export class ApproverMatrixContractModalComponent {
  @Input() GetApprovalMatrixContract!: (pageSize: number, pageNumber: number) => void;
  @Input() approvalMatrixContract?: ApprovalMatrixContract;
  @Input() isEdit: boolean = false;
  approvers1: MasterEmployee[] = [];
  approvers2: MasterEmployee[] = [];
  approvers3: MasterEmployee[] = [];

  errorMsg: string = "";
  editApprovalMatrixContractDto: EditApprovalMatrixContractDto = new EditApprovalMatrixContractDto("", "", "", 0);

  @ViewChild('editApproverCollapse1') editApproverCollapse1!: ElementRef;
  @ViewChild('editApproverCollapse2') editApproverCollapse2!: ElementRef;
  @ViewChild('editApproverCollapse3') editApproverCollapse3!: ElementRef;
  @ViewChild('editApproverName1') editApproverName1!: ElementRef;
  @ViewChild('editApproverName2') editApproverName2!: ElementRef;
  @ViewChild('editApproverName3') editApproverName3!: ElementRef;
  @ViewChild('editApproverId1') editApproverId1!: ElementRef;
  @ViewChild('editApproverId2') editApproverId2!: ElementRef;
  @ViewChild('editApproverId3') editApproverId3!: ElementRef;
  @ViewChild('editNumberOfDays') editNumberOfDays!: ElementRef;

  constructor(private approverMatrixContractService: ApproverMatrixContractService, private renderer: Renderer2) { }

  closeEditApproverCollapses() {
    if (this.editApproverCollapse1 && this.editApproverCollapse2 && this.editApproverCollapse3) {
      this.renderer.removeClass(this.editApproverCollapse1.nativeElement, 'show');
      this.renderer.removeClass(this.editApproverCollapse2.nativeElement, 'show');
      this.renderer.removeClass(this.editApproverCollapse3.nativeElement, 'show');
      this.approvers1.length = 0;
      this.approvers2.length = 0;
      this.approvers3.length = 0;
    }
    if(this.approvalMatrixContract){
      this.approvalMatrixContract.masterApprovalMatrixContractId = 0;
      this.approvalMatrixContract.approverId1 = '';
      this.approvalMatrixContract.approverId2 = '';
      this.approvalMatrixContract.approverId3 = '';
      this.approvalMatrixContract.approverName1 = '';
      this.approvalMatrixContract.approverName2 = '';
      this.approvalMatrixContract.approverName3 = '';
      this.approvalMatrixContract.departmentId = 0;
      this.approvalMatrixContract.departmentName = '';
      this.approvalMatrixContract.numberOfDays = 0;
      this.approvalMatrixContract.totalRecords = 0;
    }
  }

  textChangeApprover(departmentId: number, event: Event, approverNumber: number) {
    let input = event.target as HTMLInputElement;
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
      const input = this.editApproverCollapse1?.nativeElement.querySelector('input');
      input.value = "";
      this.approvers1.length = 0;
      this.renderer?.removeClass(this.editApproverCollapse1?.nativeElement, 'show');
      this.editApproverName1.nativeElement.value = approverName;
      this.editApproverId1.nativeElement.value = approverId;
    }
    else if (inputNumber == 2) {
      const input = this.editApproverCollapse2?.nativeElement.querySelector('input');
      input.value = "";
      this.approvers2.length = 0;
      this.renderer?.removeClass(this.editApproverCollapse2?.nativeElement, 'show');
      this.editApproverName2.nativeElement.value = approverName;
      this.editApproverId2.nativeElement.value = approverId;
    }
    else if (inputNumber == 3) {
      const input = this.editApproverCollapse3?.nativeElement.querySelector('input');
      input.value = "";
      this.approvers3.length = 0;
      this.renderer?.removeClass(this.editApproverCollapse3?.nativeElement, 'show');
      this.editApproverName3.nativeElement.value = approverName;
      this.editApproverId3.nativeElement.value = approverId;
    }
  }

  editApproverMatrixContractSubmit(id: number) {
    let nod = this.editNumberOfDays?.nativeElement.value;
    if (nod !== "" && Number(nod) > 0) {
      this.editApprovalMatrixContractDto.approverId1 = this.editApproverId1?.nativeElement.value;
      this.editApprovalMatrixContractDto.approverId2 = this.editApproverId2?.nativeElement.value;
      this.editApprovalMatrixContractDto.approverId3 = this.editApproverId3?.nativeElement.value;
      this.editApprovalMatrixContractDto.numberOfDays = nod;
      this.approverMatrixContractService.EditApproverMatrixContract(id, this.editApprovalMatrixContractDto).subscribe({
        next: (response: boolean) => {
          if (response) {
            Alert.toast(TYPE.SUCCESS, true, "Updated successfully");
            this.GetApprovalMatrixContract(1, 10);
          }
        },
        error: (error) => {
          console.error('Error :(', error);
          this.errorMsg = JSON.stringify((error.message !== undefined) ? error.error.message : error.error.title);
          Alert.toast(TYPE.ERROR, true, this.errorMsg);
        }
      })
    }
    else {
      Alert.toast(TYPE.ERROR, true, "Incorrect number of days");
    }
    this.closeEditApproverCollapses();
  }
}
