declare var bootstrap: any;
import { Component, ElementRef, Input, Renderer2, ViewChild } from '@angular/core';
import { ApprovalMatrixMouService } from '../../../services/approval-matrix-mou.service';
import { MasterEmployee } from '../../../models/master-employee';
import { Alert } from '../../../utils/alert';
import { TYPE } from '../../auth/login/values.constants';
import { ApprovalMatrixMou, EditApprovalMatrixMOUDto } from '../../../models/approval-matrix-mou';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-approval-matrix-mou-modal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './approval-matrix-mou-modal.component.html',
  styleUrl: './approval-matrix-mou-modal.component.css'
})
export class ApprovalMatrixMouModalComponent {
  @Input() GetApprovalMatrixMou!: (pageSize: number, pageNumber: number) => void;
  @Input() approvalMatrixMOU?: ApprovalMatrixMou;
  @Input() isEdit: boolean = false;
  approvers1: MasterEmployee[] = [];
  approvers2: MasterEmployee[] = [];
  approvers3: MasterEmployee[] = [];

  errorMsg: string = "";
  editApprovalMatrixMOUDto: EditApprovalMatrixMOUDto = new EditApprovalMatrixMOUDto("", "", "", 0);

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

  constructor(private approverMatrixMouService: ApprovalMatrixMouService, private renderer: Renderer2) { }

  closeEditApproverCollapses() {
    if (this.editApproverCollapse1 && this.editApproverCollapse2 && this.editApproverCollapse3) {
      this.renderer.removeClass(this.editApproverCollapse1.nativeElement, 'show');
      this.renderer.removeClass(this.editApproverCollapse2.nativeElement, 'show');
      this.renderer.removeClass(this.editApproverCollapse3.nativeElement, 'show');
      this.approvers1.length = 0;
      this.approvers2.length = 0;
      this.approvers3.length = 0;
    }
    if (this.approvalMatrixMOU) {
      this.approvalMatrixMOU.masterApprovalMatrixMOUId = 0;
      this.approvalMatrixMOU.approverId1 = '';
      this.approvalMatrixMOU.approverId2 = '';
      this.approvalMatrixMOU.approverId3 = '';
      this.approvalMatrixMOU.approverName1 = '';
      this.approvalMatrixMOU.approverName2 = '';
      this.approvalMatrixMOU.approverName3 = '';
      this.approvalMatrixMOU.departmentId = 0;
      this.approvalMatrixMOU.departmentName = '';
      this.approvalMatrixMOU.numberOfDays = 0;
      this.approvalMatrixMOU.totalRecords = 0;
    }
  }

  textChangeApprover(departmentId: number, event: Event, approverNumber: number) {
    let input = event.target as HTMLInputElement;
    this.approverMatrixMouService.GetApproversForInputText(departmentId, input.value).subscribe(
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
      this.editApprovalMatrixMOUDto.approverId1 = this.editApproverId1?.nativeElement.value;
      this.editApprovalMatrixMOUDto.approverId2 = this.editApproverId2?.nativeElement.value;
      this.editApprovalMatrixMOUDto.approverId3 = this.editApproverId3?.nativeElement.value;
      this.editApprovalMatrixMOUDto.numberOfDays = nod;
      this.approverMatrixMouService.EditApproverMatrixMOU(id, this.editApprovalMatrixMOUDto).subscribe({
        next: (response: boolean) => {
          if (response) {
            Alert.toast(TYPE.SUCCESS, true, "Updated successfully");
            this.GetApprovalMatrixMou(1, 10);
          }
          const modalElement = document.getElementById('approval-matrix-mou-modal');
          if (modalElement) {
            const modalInstance = bootstrap.Modal.getInstance(modalElement) || new bootstrap.Modal(modalElement);
            modalInstance.hide();
          }
          this.closeEditApproverCollapses();
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
  }
}
