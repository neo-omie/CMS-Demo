declare var bootstrap: any
import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormControl, FormGroup, FormsModule, NgForm, ReactiveFormsModule, Validators } from '@angular/forms';
import { Alert } from '../../../utils/alert';
import { TYPE } from '../../auth/login/values.constants';
import { PostTerminationService } from '../../../services/post-termination.service';
import { ValidateFile } from '../../../utils/validateFile';
import { dateValidator } from '../../../utils/dateValidator';
import { ErrorHandler } from '../../../utils/errorHandler';

@Component({
  selector: 'app-post-termination-notice-modal',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  
  templateUrl: './post-termination-notice-modal.component.html',
  styleUrl: './post-termination-notice-modal.component.css'
})
export class PostTerminationNoticeModalComponent {
  @Output() loaderEmit = new EventEmitter<boolean>();
  @Input() GetPage !: (pgNumber: number) => void;
  @Input() currentPage?: number;
  @Input() contractId?: string;
  file: File | null = null;

  constructor(private postTermService: PostTerminationService) { }

  documentForm: FormGroup = new FormGroup({
    file: new FormControl(null, [Validators.required]),
    end_Date: new FormControl('', [Validators.required, dateValidator()]),
    Remark: new FormControl('', [Validators.required]),
  })

  resetForm() {
    this.documentForm.reset({
      file: null,
      end_Date: '',
      Remark: '',
    })
  }

  uploadFile(event: Event) {
    this.file = ValidateFile.validateFile(event);
    if (!this.file) {
      this.documentForm.get('file')?.setValue('');
    }
  }

  onSavePostTermination() {
    if (!this.file || !this.documentForm.valid) {
      this.documentForm.markAllAsTouched();
      this.file = null;
      return;
    }

    const formData = new FormData();
    formData.append('file', this.file);
    formData.append('contractId', String(this.contractId));
    formData.append('notice_Duration', String(0));
    formData.append('end_Date', String(this.documentForm.get('end_Date')?.value));
    formData.append('Remark', String(this.documentForm.get('Remark')?.value));
    this.loaderEmit.emit(true)
    this.postTermService.UploadDoc(formData).subscribe({
      next: (res) => {
        this.file = null;
        this.loaderEmit.emit(false);
        this.resetForm();
        Alert.toast(TYPE.SUCCESS, true, ' Notice added successfully');
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
        ErrorHandler.handle(err);
      },
    });
  }
}
