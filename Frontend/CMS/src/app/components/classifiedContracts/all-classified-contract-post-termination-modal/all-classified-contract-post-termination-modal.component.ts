declare var bootstrap: any;
import { CommonModule } from '@angular/common';
import { Component, ElementRef, Input, ViewChild } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { Alert } from '../../../utils/alert';
import { TYPE } from '../../auth/login/values.constants';
import { PostTerminationNoticeUploadDTO } from '../../../models/post-termination-notice';
import { PostTerminationService } from '../../../services/post-termination.service';
import { ValidateFile } from '../../../utils/validateFile';

@Component({
  selector: 'app-all-classified-contract-post-termination-modal',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './all-classified-contract-post-termination-modal.component.html',
  styleUrl: './all-classified-contract-post-termination-modal.component.css'
})
export class AllClassifiedContractPostTerminationModalComponent {
  @Input() currentPgNumber: number = 1;
  @Input() GetPage !: (pgNumber: number) => void;
  @Input() contIdForPostTerm?: number = 0;
  file: File | null = null;
  @ViewChild('addFile') addFile!: ElementRef;
  postTerm: PostTerminationNoticeUploadDTO = new PostTerminationNoticeUploadDTO(null, 0, new Date(), '');

  constructor(private postTermService: PostTerminationService) { }

  uploadFile(event: Event) {
    this.file = ValidateFile.validateFile(event);
  }

  //uploading the Post Termination Notice 
  OnSavePostTermination(documentForm: NgForm) {

    if (!this.file || !documentForm.valid) {
      this.addFile.nativeElement.value = "";
      this.postTerm.file = null
      this.postTerm.notice_Duration = 1;
      this.postTerm.end_Date = new Date();
      this.postTerm.Remark = "";
      Alert.toast(TYPE.WARNING, true, "Please select a file and fill the Form Correctly");
      return;
    }
    const allowedExtensions = ['.pdf', '.doc', '.docx'];
    const fileExtension = this.file.name.substring(this.file.name.lastIndexOf('.')).toLowerCase();

    if (!allowedExtensions.includes(fileExtension)) {
      this.addFile.nativeElement.value = "";
      this.postTerm.file = null;
      this.postTerm.notice_Duration = 1;
      this.postTerm.end_Date = new Date();
      this.postTerm.Remark = "";
      Alert.toast(TYPE.WARNING, true, "Unsupported file format. Allowed formats: .pdf, .doc, .docx ");
      return;
    }
    if (this.file.size > 25 * 1048576) {
      this.addFile.nativeElement.value = "";
      this.postTerm.file = null;
      Alert.toast(TYPE.WARNING, true, "File too large. Max 25MB allowed.");
      return;
    }

    const formData = new FormData();
    formData.append('file', this.file)
    formData.append('contractId', String(this.contIdForPostTerm))
    formData.append('notice_Duration', String(this.postTerm.notice_Duration))
    formData.append('end_Date', String(this.postTerm.end_Date))
    formData.append('Remark', String(this.postTerm.Remark))
    this.postTermService.UploadClassifiedDoc(formData).subscribe({
      next: (res) => {
        this.file = null;
        documentForm.reset();

        Alert.bigToast(
          'Success!',
          'Posted Termination successfully.',
          TYPE.SUCCESS,
          'Ok'
        );
        this.GetPage(this.currentPgNumber);

        const modalElement = document.getElementById('Termination-Notice-Detail');
        if (modalElement) {
          const modalInstance = bootstrap.Modal.getInstance(modalElement) || new bootstrap.Modal(modalElement);
          modalInstance.hide();
        }

      },
      error: (error) => {
        console.error('Error in creating Notice:', error);
         if (error.status == 401) {
            let errmsg = error.error;
            Alert.toast(TYPE.ERROR, true, errmsg);
          }
          else{
            Alert.bigToast(
              'Error!',
              'There was an error posting termination notice. ' + error.error.message,
              TYPE.ERROR,
              'Try Again'
            );
          }
      },
    });
  }
}

