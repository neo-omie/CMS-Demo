import {
  Component,
  ElementRef,
  OnInit,
  Renderer2,
  ViewChild,
} from '@angular/core';
import { MasterDocumentService } from '../../services/master-document.service';
import { Router, RouterModule } from '@angular/router';
import {
  AddDocumentDto,
  GetDocumentById,
  MasterDocument,
  MasterDocumentDto,
} from '../../models/master-document';
import { FormsModule, NgForm } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { LoaderComponent } from '../loader/loader.component';
import { DocumentStatus } from '../../constants';
import { Alert } from '../../utils/alert';
import { TYPE } from '../auth/login/values.constants';
import { Pagination } from '../../utils/pagination';

@Component({
  selector: 'app-master-document',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterModule, LoaderComponent],
  templateUrl: './master-document.component.html',
  styleUrl: './master-document.component.css',
})
export class MasterDocumentComponent implements OnInit {
  file:File|null = null;
  loading: boolean = true;
  maxPage = 1;
  pageNumbers = [1, 1, 2, 3, 4, 5];
  masterDocuments: MasterDocumentDto = new MasterDocumentDto([], 0);
  documentStatus = DocumentStatus;
  document: AddDocumentDto = new AddDocumentDto(null, 0);
  errorMsg?: string;
  getMasterDocumentById?: GetDocumentById;
  doc?: MasterDocument;
  @ViewChild('editDocumentName') editDocumentName!: ElementRef;
  @ViewChild('editDocumentStatus') editDocumentStatus!: ElementRef;

  ngOnInit(): void {
    this.getDocuments(1, 10);
  }
  constructor(
    private documentService: MasterDocumentService,
    private router: Router,
    private renderer: Renderer2
  ) {}
  closeEditApproverCollapses() {
    this.renderer.removeClass(this.editDocumentName.nativeElement, 'show');
    this.renderer.removeClass(this.editDocumentStatus.nativeElement, 'show');
    this.doc = undefined;
  }

  getDocuments(pageNumber: number, pageSize: number) {
    this.documentService.getDocument(pageNumber, pageSize).subscribe({
      next: (res: MasterDocumentDto) => {
        this.loading = false;
        this.masterDocuments = res;
        if (this.masterDocuments.documents.length > 0) {
          let result = Pagination.paginator(
            pageNumber,
            this.masterDocuments.totalCount,
            pageSize
          );
          this.maxPage = result.maxPage;
          this.pageNumbers = result.pageNumbers;
        }
      },
      error: (error) => {
        this.loading = false;
        console.error('Error :(', error);
        this.errorMsg = JSON.stringify(
          error.message !== undefined ? error.error.title : error.message
        );
        Alert.toast(TYPE.ERROR, true, this.errorMsg);
      },
    });
  }
  GetPage(pgNumber: number) {
    if (this.maxPage >= pgNumber && pgNumber >= 1) {
      this.getDocuments(pgNumber, 10);
    }
  }

  addDocument(documentForm: NgForm) {
    if (!this.file || !documentForm.valid) {
      alert('Form is invalid or file is missing');
      return;
    }
    const formData = new FormData();
    formData.append('File',this.file)
    formData.append('Status',String(this.document.status))

    this.documentService.addDocument(formData).subscribe({
      next: (response) => {
        Alert.bigToast(
          'Success!',
          'Document added successfully.',
          TYPE.SUCCESS,
          'Ok'
        );
        documentForm.reset();
        this.GetPage(this.maxPage);
      },
      error: (error) => {
        console.error('Error adding Document:', error);
        Alert.bigToast(
          'Error!',
          'There was an error adding the Document.',
          TYPE.ERROR,
          'Try Again'
        );
        documentForm.resetForm();
      },
    });
  }

  editDocument(id: number) {
    let docName = this.editDocumentName.nativeElement.value;
    let status = Number(this.editDocumentStatus.nativeElement.value);
    this.document.file = docName;
    this.document.status = status;
    if (docName !== '') {
      this.documentService.updateDocument(id, this.document).subscribe({
        next: (response: string) => {
          if (response) {
            Alert.toast(TYPE.SUCCESS, true, 'Document Updated Successfully');
            this.getDocuments(1, 10);
          }
        },
        error: (error) => {
          console.error('Error :(', error);
          this.errorMsg = JSON.stringify(
            error.message !== undefined ? error.error.title : error.message
          );
          Alert.toast(TYPE.ERROR, true, this.errorMsg);
        },
      });
    } else {
      Alert.toast(TYPE.ERROR, true, 'Please enter the department name.');
    }
    this.closeEditApproverCollapses();
  }

  deleteDocument(id?: number) {
    Alert.confirmToast(
      'Are you sure you want to delete this document?',
      "You won't be able to revert this!!",
      TYPE.WARNING,
      'Yes ,Delete it',
      'Deleted Successfully',
      'Document has been Deleted',
      TYPE.SUCCESS,
      () => {
        if (id !== undefined) {
          this.documentService.deleteDocument(id).subscribe({
            next: () => {
              Alert.toast(TYPE.SUCCESS, true, 'Document Deleted successfully');
              this.getDocuments(1, 10);
            },
            error: () => {
              Alert.toast(TYPE.ERROR, true, this.errorMsg);
            },
          });
        }
      }
    );
  }

  GetDocument(id:number){
          this.documentService.getById(id).subscribe({
            next:(response:GetDocumentById) => {
              this.getMasterDocumentById = response;
              this.doc = response;
            }, 
            error:(error) => {
              console.error('Error :(', error);
              this.errorMsg = JSON.stringify((error.message !== undefined)?error.error.title: error.message);
              Alert.toast(TYPE.ERROR,true,this.errorMsg);
          }});
        }

        uploadFile(event: Event) {
          const input = event.target as HTMLInputElement;
          if (input.files?.length) {
            this.file = input.files[0];
          }
        }
}
