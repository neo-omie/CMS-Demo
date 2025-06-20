declare var bootstrap :any
import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Alert } from '../../../utils/alert';
import { TYPE } from '../../auth/login/values.constants';
import { DecodeToken } from '../../../utils/decodeToken';
import { PostTermination } from '../../../models/post-termination';
import { PostTerminationService } from '../../../services/post-termination.service';
import { ErrorHandler } from '../../../utils/errorHandler';
import { Router } from '@angular/router';
import { ApproveRejectWithdrawalDTO } from '../../../models/notice-withdrawal';
import { NoticeWithdrawalService } from '../../../services/notice-withdrawal.service';

@Component({
  selector: 'app-withdrawal-contract-mail-body-modal',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './withdrawal-contract-mail-body-modal.component.html',
  styleUrl: './withdrawal-contract-mail-body-modal.component.css'
})
export class WithdrawalContractMailBodyModalComponent {
  @Output() loaderEmit = new EventEmitter<boolean>();
  @Input() GetPage !: (pgNumber: number) => void;
  @Input() mailType ?: string
  @Input() statusTermOrReject?: number; 
  @Input() currentPage: number = 1;
  @Input() contractId?: string = '0';

  constructor(
    private postTermService: PostTerminationService,
    private noticeWithdrawalService: NoticeWithdrawalService,
    private router: Router,
  ) { }

  emailForm = new FormGroup({
    emailSubject: new FormControl('', [Validators.required]),
    emailBody: new FormControl('', [Validators.required]),
  });

  restForm(){
    this.emailForm.reset({
      emailSubject: '',
      emailBody: ''
    })
  }

  approveTerminateContract(contractId: string) {
    if (this.emailForm.invalid) {
      this.emailForm.markAllAsTouched();
      return;
    } else {
      const emailSubject = this.emailForm.value.emailSubject;
      const emailBody = this.emailForm.value.emailBody;
      let email = DecodeToken.email;
      if (email && this.mailType && this.mailType != '') {
        if(this.mailType == 'postTerm'){
          this.loaderEmit.emit(true);
          const postTermination: PostTermination = new PostTermination();
          postTermination.contractId = Number(contractId);
          postTermination.changeToStatus = this.statusTermOrReject;
          postTermination.emailSubject = emailSubject;
          postTermination.emailBody = emailBody;
          postTermination.employeeEmail = email;
  
          this.postTermService
            .ApproveTerminationContract(postTermination)
            .subscribe({
              next: (res) => {
                if (res) {
                  this.loaderEmit.emit(false);
                  Alert.toast(TYPE.SUCCESS, true, 'Updated successfully');
                  this.GetPage(this.currentPage);
                }
                const modalElement = document.getElementById('postTerm-mail');
                if (modalElement) {
                  const modalInstance =
                    bootstrap.Modal.getInstance(modalElement) ||
                    new bootstrap.Modal(modalElement);
                  modalInstance.hide();
                }
              },
              error: (err) => {
                this.loaderEmit.emit(false);
                ErrorHandler.handle(err);
              },
            });
        }
        else if(this.mailType == 'withdrawal'){
          this.loaderEmit.emit(true);
          const withdrawNoticeSend: ApproveRejectWithdrawalDTO = new ApproveRejectWithdrawalDTO();
          withdrawNoticeSend.contractId = Number(contractId);
          withdrawNoticeSend.changeToStatus = this.statusTermOrReject;
          withdrawNoticeSend.emailSubject = emailSubject;
          withdrawNoticeSend.emailBody = emailBody;
          withdrawNoticeSend.employeeEmail = email;
          this.noticeWithdrawalService.ApproveWithdrawalTermination(withdrawNoticeSend).subscribe({
            next: (res) => {
              if (res) {
                this.loaderEmit.emit(false)
                Alert.toast(TYPE.SUCCESS, true, 'Updated successfully');
                this.GetPage(this.currentPage);

              }
              const modalElement = document.getElementById('postTerm-mail');
            if (modalElement) {
              const modalInstance =
                bootstrap.Modal.getInstance(modalElement) ||
                new bootstrap.Modal(modalElement);
              modalInstance.hide();
            }
            },
            error: (err) => {
              this.loaderEmit.emit(false)
              ErrorHandler.handle(err)
            }
          })
        }
        else{
          Alert.toast(TYPE.ERROR, true, 'Invalid mail type');
        }
      } else {
        DecodeToken.clearUserCredentials();
        sessionStorage.clear()
        this.router.navigate(['/']);
      }
    }
  }
}
