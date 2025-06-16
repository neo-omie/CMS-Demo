declare var bootstrap: any;
import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { ValidateFile } from '../../../utils/validateFile';
import { NoticeWithdrawalService } from '../../../services/notice-withdrawal.service';
import { Alert } from '../../../utils/alert';
import { TYPE } from '../../auth/login/values.constants';
import { ErrorHandler } from '../../../utils/errorHandler';

@Component({
  selector: 'app-termination-withdraw-notice',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './termination-withdraw-notice.component.html',
  styleUrl: './termination-withdraw-notice.component.css'
})
export class TerminationWithdrawNoticeComponent {
  @Output() loaderEmit = new EventEmitter<boolean>();
  @Input() GetPage !: (pgNumber: number) => void;
  @Input() currentPage?: number;
  @Input() contractId?: string;
  file: File | null = null;

  documentForm: FormGroup = new FormGroup({
    file: new FormControl(null, [Validators.required]),
    Remark: new FormControl('', [Validators.required]),
  })

  constructor(private noticeWithdrawalService: NoticeWithdrawalService) { }

  uploadFile(event: Event) {
    this.file = ValidateFile.validateFile(event);
    if (!this.file) {
      this.documentForm.get('file')?.setValue('');
    }
  }

  resetForm() {
    this.documentForm.reset({
      file: null,
      end_Date: '',
      Remark: '',
    })
  }

  OnSaveWithdrawalNotice() {
    if (!this.file || !this.documentForm.valid) {
      this.documentForm.markAllAsTouched();
      this.file = null;
      return;
    }

    const formData = new FormData();
    formData.append('file', this.file);
    formData.append('contractId', String(this.contractId));
    formData.append('postTermId', String(1));
    formData.append('Remark', String(this.documentForm.get('Remark')?.value));
    this.loaderEmit.emit(true);

    this.noticeWithdrawalService.AddWithdrawalNotice(formData).subscribe({
      next: (res) => {
        this.file = null;
        Alert.toast(TYPE.SUCCESS, true, 'Withdrawal Notice Added Successfully!');
        this.resetForm()
        this.loaderEmit.emit(false);
        const modalElement = document.getElementById('Termination-Notice-Detail');
        if (modalElement) {
          const modalInstance = bootstrap.Modal.getInstance(modalElement) || new bootstrap.Modal(modalElement);
          modalInstance.hide();
        }
        if (this.currentPage) {
          this.GetPage(this.currentPage);
        }
      },
      error: (err) => {
        this.loaderEmit.emit(false);
        ErrorHandler.handle(err)
      },
    });
  }
}
