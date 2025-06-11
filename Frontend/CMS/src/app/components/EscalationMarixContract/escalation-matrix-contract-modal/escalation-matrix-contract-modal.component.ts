declare var bootstrap: any;
import {
  Component,
  ElementRef,
  Input,
  Renderer2,
  ViewChild,
} from '@angular/core';
import {
  GetMasterEscalationMatrixContractByIdDto,
  UpdateMatrixContractDto,
} from '../../../models/escalation-matrix-contract';
import { MasterEmployee } from '../../../models/master-employee';
import { ApproverMatrixContractService } from '../../../services/approver-matrix-contract.service';
import { EscalationMatrixContractService } from '../../../services/escalation-matrix-contract.service';
import { Alert } from '../../../utils/alert';
import { TYPE } from '../../auth/login/values.constants';
import { CommonModule } from '@angular/common';
import { DecodeToken } from '../../../utils/decodeToken';
import { LoaderComponent } from '../../UtilComponents/loader/loader.component';

@Component({
  selector: 'app-escalation-matrix-contract-modal',
  standalone: true,
  imports: [CommonModule,LoaderComponent],
  templateUrl: './escalation-matrix-contract-modal.component.html',
  styleUrl: './escalation-matrix-contract-modal.component.css',
})
export class EscalationMatrixContractModalComponent {
  @Input() getMatrixMous!: (pageSize: number, pageNumber: number) => void;
  @Input() matrixContract?: GetMasterEscalationMatrixContractByIdDto;
  @Input() isEdit: boolean = false;
loading: boolean = true;
  errorMsg: string = '';
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
    private escalationService: EscalationMatrixContractService,
    private renderer: Renderer2,
    private approverMatrixContractService: ApproverMatrixContractService
  ) {}

  closeEditApproverCollapses() {
    if (
      this.editApproverCollapse1 &&
      this.editApproverCollapse2 &&
      this.editApproverCollapse3
    ) {
      this.renderer.removeClass(
        this.editApproverCollapse1.nativeElement,
        'show'
      );
      this.renderer.removeClass(
        this.editApproverCollapse2.nativeElement,
        'show'
      );
      this.renderer.removeClass(
        this.editApproverCollapse3.nativeElement,
        'show'
      );
      this.approvers1.length = 0;
      this.approvers2.length = 0;
      this.approvers3.length = 0;
    }
    if (this.matrixContract) {
      this.matrixContract.escalationId1 = '';
      this.matrixContract.escalationId2 = '';
      this.matrixContract.escalationId3 = '';
      this.matrixContract.escalation1 = '';
      this.matrixContract.escalation2 = '';
      this.matrixContract.escalation3 = '';
      this.matrixContract.departmentId = 0;
      this.matrixContract.departmentName = '';
      this.matrixContract.matrixContractId = 0;
      this.matrixContract.triggerDaysEscalation1 = 0;
      this.matrixContract.triggerDaysEscalation2 = 0;
      this.matrixContract.triggerDaysEscalation3 = 0;
    }
  }

  textChangeApprover(
    departmentId: number,
    event: Event,
    approverNumber: number
  ) {
    let input = event.target as HTMLInputElement;
    console.log('Department ID : ', departmentId);
    this.approverMatrixContractService
      .GetApproversForInputText(departmentId, input.value)
      .subscribe({
        next: (response: MasterEmployee[]) => {
          if (approverNumber == 1) {
            this.approvers1 = response;
          } else if (approverNumber == 2) {
            this.approvers2 = response;
          } else if (approverNumber == 3) {
            this.approvers3 = response;
          }
        },
        error: (error) => {
          this.loading =false;
          console.error('Error :(', error);
          if (error.status == 401) {
            let errmsg = error.error;
            Alert.toast(TYPE.ERROR, true, errmsg);
          } else {
            this.errorMsg = JSON.stringify(
              error.message !== undefined ? error.error.title : error.message
            );
            Alert.toast(TYPE.ERROR, true, this.errorMsg);
          }
        },
      });
  }

  fillApprover(approverId: string, approverName: string, inputNumber: number) {
    if (inputNumber == 1) {
      const input =
        this.editApproverCollapse1.nativeElement.querySelector('input');
      input.value = '';
      this.approvers1.length = 0;
      this.renderer.removeClass(
        this.editApproverCollapse1.nativeElement,
        'show'
      );
      this.editApproverName1.nativeElement.value = approverName;
      this.editApproverId1.nativeElement.value = approverId;
    } else if (inputNumber == 2) {
      const input =
        this.editApproverCollapse2.nativeElement.querySelector('input');
      input.value = '';
      this.approvers2.length = 0;
      this.renderer.removeClass(
        this.editApproverCollapse2.nativeElement,
        'show'
      );
      this.editApproverName2.nativeElement.value = approverName;
      this.editApproverId2.nativeElement.value = approverId;
    } else if (inputNumber == 3) {
      const input =
        this.editApproverCollapse3.nativeElement.querySelector('input');
      input.value = '';
      this.approvers3.length = 0;
      this.renderer.removeClass(
        this.editApproverCollapse3.nativeElement,
        'show'
      );
      this.editApproverName3.nativeElement.value = approverName;
      this.editApproverId3.nativeElement.value = approverId;
    }
  }

  editApproverMatrixContractSubmit(id: number) {
    let empCode = DecodeToken.ECode;
    let updateMatrixContractDto = new UpdateMatrixContractDto(
      '',
      '',
      '',
      '',
      '',
      '',
      0,
      0,
      0
    );
    let nod1 = this.editNumberOfDays1.nativeElement.value;
    let nod2 = this.editNumberOfDays2.nativeElement.value;
    let nod3 = this.editNumberOfDays3.nativeElement.value;
    let ap1 = this.editApproverId1.nativeElement.value;
    let ap2 = this.editApproverId2.nativeElement.value;
    let ap3 = this.editApproverId3.nativeElement.value;
    if (
      nod1 !== '' &&
      Number(nod1) > 0 &&
      nod2 !== '' &&
      Number(nod2) > 0 &&
      nod3 !== '' &&
      Number(nod3) > 0
    ) {
      updateMatrixContractDto.escalationId1 = ap1;
      updateMatrixContractDto.escalationId2 = ap2;
      updateMatrixContractDto.escalationId3 = ap3;
      updateMatrixContractDto.triggerDaysEscalation1 = nod1;
      updateMatrixContractDto.triggerDaysEscalation2 = nod2;
      updateMatrixContractDto.triggerDaysEscalation3 = nod3;
      this.escalationService
        .postMatrixContractById(id, updateMatrixContractDto, empCode)
        .subscribe({
          next: (response: any) => {
            Alert.toast(TYPE.SUCCESS, true, response.message);
            this.getMatrixMous(1, 10);
            const modalElement = document.getElementById(
              'escalation-matrix-contract-modal'
            );
            if (modalElement) {
              const modalInstance =
                bootstrap.Modal.getInstance(modalElement) ||
                new bootstrap.Modal(modalElement);
              modalInstance.hide();
            }
            this.closeEditApproverCollapses();
          },
          error: (error) => {
            console.error('Error :(', error);
            if (error.status == 401) {
              let errmsg = error.error;
              Alert.toast(TYPE.ERROR, true, errmsg);
            } else {
              this.errorMsg = JSON.stringify(
                error.message !== undefined
                  ? error.error.message
                  : error.error.title
              );
              Alert.toast(TYPE.ERROR, true, this.errorMsg);
            }
          },
        });
    } else {
      Alert.toast(TYPE.ERROR, true, 'Incorrect number of days');
    }
  }
}
