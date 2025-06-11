declare var bootstrap: any;
import {
  Component,
  ElementRef,
  OnInit,
  Renderer2,
  Type,
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
import { DocumentStatus } from '../../constants';
import { Alert } from '../../utils/alert';
import { TYPE } from '../auth/login/values.constants';
import { Pagination } from '../../utils/pagination';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { LoaderComponent } from '../UtilComponents/loader/loader.component';
import { DecodeToken } from '../../utils/decodeToken';
import { ErrorHandler } from '../../utils/errorHandler';

@Component({
  selector: 'app-master-document',
  standalone: true,
  imports: [
    FormsModule,
    CommonModule,
    RouterModule,
    LoaderComponent,
    MatTableModule,
    MatSortModule,
    MatFormFieldModule,
    MatInputModule,
  ],
  templateUrl: './master-document.component.html',
  styleUrl: './master-document.component.css',
})
export class MasterDocumentComponent implements OnInit {
  displayedColumns: string[] = ['displayDocumentName', 'status', 'action'];
  dataSource = new MatTableDataSource<MasterDocument>();
  @ViewChild(MatSort) sort!: MatSort;
  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
  }
  file: File | null = null;
  loading: boolean = true;
  maxPage = 1;
  pageNumbers = [1, 1, 2, 3, 4, 5];
  masterDocuments: MasterDocumentDto = new MasterDocumentDto([], 0);
  documentStatus = DocumentStatus;
  document: AddDocumentDto = new AddDocumentDto(null, 1);
  errorMsg?: string;
  getMasterDocumentById?: GetDocumentById;
  doc?: MasterDocument;
  existingFilePath: string | null = null;
  @ViewChild('editDocumentName') editDocumentName!: ElementRef;
  @ViewChild('editDocumentStatus') editDocumentStatus!: ElementRef;
  @ViewChild('addFile') addFile!: ElementRef;
  @ViewChild('editFile') editFile!: ElementRef;

  ngOnInit(): void {
    this.getDocuments(1, 10);
  }
  constructor(
    private documentService: MasterDocumentService,
    private router: Router,
    private renderer: Renderer2
  ) {}
  closeEditApproverCollapses() {
    // this.renderer.removeClass(this.editDocumentName.nativeElement, 'show');
    // this.renderer.removeClass(this.editDocumentStatus.nativeElement, 'show');
    this.getMasterDocumentById = undefined;
    this.doc = undefined;
    this.file = null;
  }

  getDocuments(pageNumber: number, pageSize: number) {
    this.documentService.getDocument(pageNumber, pageSize).subscribe({
      next: (res: MasterDocumentDto) => {
        this.loading = false;
        this.dataSource.data = res.documents;
        if (this.sort) {
          this.dataSource.sort = this.sort;
        }
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
  GetPage(pgNumber: number) {
    if (this.maxPage >= pgNumber && pgNumber >= 1) {
      this.getDocuments(pgNumber, 10);
    }
  }

  addDocument(documentForm: NgForm) {
    let empCode = DecodeToken.ECode;
    if (!this.file || !documentForm.valid) {
      this.addFile.nativeElement.value = '';
      this.document.file = null;
      this.document.status = 1;
      Alert.toast(
        TYPE.WARNING,
        true,
        'Please select a file and fill the form correctly.'
      );
      return;
    }

    const allowedExtensions = [
      '.pdf',
      '.doc',
      '.docx',
      '.jpg',
      '.jpeg',
      '.png',
    ];
    const fileExtension = this.file.name
      .substring(this.file.name.lastIndexOf('.'))
      .toLowerCase();

    if (!allowedExtensions.includes(fileExtension)) {
      this.addFile.nativeElement.value = '';
      this.document.file = null;
      this.document.status = 1;
      Alert.toast(
        TYPE.WARNING,
        true,
        'Unsupported file format. Allowed formats: .pdf, .doc, .docx, .jpg, .jpeg and .png.'
      );
      return;
    }

    if (this.file.size > 25 * 1048576) {
      this.addFile.nativeElement.value = '';
      this.document.file = null;
      this.document.status = 1;
      Alert.toast(TYPE.WARNING, true, 'File too large. Max 25MB allowed.');
      return;
    }

    const formData = new FormData();
    formData.append('File', this.file);
    formData.append('Status', String(this.document.status));
    this.documentService.checkDocumentExist(formData).subscribe({
      next: (res: boolean) => {
        if (res) {
          Alert.confirmToast(
            'Are you sure?',
            'File with this name already exist. Submitting will replace that file',
            TYPE.WARNING,
            'Add',
            'Added successfully',
            '',
            TYPE.SUCCESS,
            () => {
              this.documentService.addDocument(formData, empCode).subscribe({
                next: (response: any) => {
                  if (response) {
                    this.getMasterDocumentById = undefined;
                    this.doc = undefined;
                    this.file = null;

                    Alert.toast(
                      TYPE.SUCCESS,
                      true,
                      'Document Added Successfully'
                    );
                    this.getDocuments(1, 10);

                    const modalElement =
                      document.getElementById('document-add');
                    if (modalElement) {
                      const modalInstance =
                        bootstrap.Modal.getInstance(modalElement) ||
                        new bootstrap.Modal(modalElement);
                      modalInstance.hide();
                    }
                  }
                },
                error: (error) => {
                  this.getMasterDocumentById = undefined;
                  this.doc = undefined;
                  this.file = null;
                  console.error('Error :(', error);
                  this.errorMsg = JSON.stringify(
                    error.message !== undefined
                      ? error.error.title
                      : error.message
                  );
                  Alert.toast(TYPE.ERROR, true, this.errorMsg);
                },
              });
            }
          );
          this.editFile.nativeElement.value = '';
          this.getMasterDocumentById = undefined;
          this.doc = undefined;
          this.file = null;
        } else {
          this.documentService.addDocument(formData, empCode).subscribe({
            next: (response: any) => {
              if (response) {
                this.getMasterDocumentById = undefined;
                this.doc = undefined;
                this.file = null;
                Alert.toast(TYPE.SUCCESS, true, 'Document Added Successfully');
                this.GetPage(this.maxPage);

                const modalElement = document.getElementById('document-add');
                if (modalElement) {
                  const modalInstance =
                    bootstrap.Modal.getInstance(modalElement) ||
                    new bootstrap.Modal(modalElement);
                  modalInstance.hide();
                }
              }
            },
            error: (error) => {
              this.getMasterDocumentById = undefined;
              this.doc = undefined;
              this.file = null;
              console.error('Error :(', error);
              this.errorMsg = JSON.stringify(
                error.message !== undefined ? error.error.title : error.message
              );
              Alert.toast(TYPE.ERROR, true, this.errorMsg);
            },
          });
        }
      },
      error: (error) => {
        console.error(error.error);
      },
    });
    this.file = null;
    // documentForm.reset();
    this.addFile.nativeElement.value = '';
    this.document.file = null;
    this.document.status = 1;
  }
  editDocument(id: number) {
    let empCode = DecodeToken.ECode;
    if (this.file) {
      const formData = new FormData();
      formData.append('File', this.file);
      formData.append(
        'Status',
        String(this.editDocumentStatus.nativeElement.value)
      );
      this.documentService.checkDocumentExist(formData).subscribe({
        next: (res: boolean) => {
          if (res) {
            Alert.confirmToast(
              'Are you sure?',
              'File with this name already exist. Submitting will replace that file',
              TYPE.WARNING,
              'Add',
              'Updated successfully',
              '',
              TYPE.SUCCESS,
              () => {
                this.documentService
                  .updateDocument(id, formData, empCode)
                  .subscribe({
                    next: (response: any) => {
                      if (response) {
                        this.getMasterDocumentById = undefined;
                        this.doc = undefined;
                        this.file = null;

                        Alert.toast(
                          TYPE.SUCCESS,
                          true,
                          'Document Updated Successfully'
                        );
                        this.getDocuments(1, 10);
                      }
                    },
                    error: (error) => {
                      this.getMasterDocumentById = undefined;
                      this.doc = undefined;
                      this.file = null;
                      console.error('Error :(', error);
                      this.errorMsg = JSON.stringify(
                        error.message !== undefined
                          ? error.error.title
                          : error.message
                      );
                      Alert.toast(TYPE.ERROR, true, this.errorMsg);
                    },
                  });
              }
            );
            this.editFile.nativeElement.value = '';
            this.getMasterDocumentById = undefined;
            this.doc = undefined;
            this.file = null;
          } else {
            this.documentService
              .updateDocument(id, formData, empCode)
              .subscribe({
                next: (response: any) => {
                  if (response) {
                    this.getMasterDocumentById = undefined;
                    this.doc = undefined;
                    this.file = null;
                    Alert.toast(
                      TYPE.SUCCESS,
                      true,
                      'Document Updated Successfully'
                    );
                    this.getDocuments(1, 10);
                  }
                },
                error: (error) => {
                  this.getMasterDocumentById = undefined;
                  this.doc = undefined;
                  this.file = null;
                  console.error('Error :(', error);
                  this.errorMsg = JSON.stringify(
                    error.message !== undefined
                      ? error.error.title
                      : error.message
                  );
                  Alert.toast(TYPE.ERROR, true, this.errorMsg);
                },
              });
          }
        },
        error: (error) => {
          console.error(error.error);
        },
      });
    } else {
      this.documentService
        .updateDocumentWithoutFille(id, {
          status: this.editDocumentStatus.nativeElement.value,
        })
        .subscribe({
          next: (response: string) => {
            this.getMasterDocumentById = undefined;
            this.doc = undefined;
            this.file = null;
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
            this.getMasterDocumentById = undefined;
            this.doc = undefined;
            this.file = null;
          },
        });
    }
    this.getMasterDocumentById = undefined;
    this.doc = undefined;
    this.file = null;
  }

  deleteDocument(id?: number) {
    let empCode = DecodeToken.ECode;
    Alert.confirmToast(
  
      'Are you sure you want to delete this document?',
      "You won't be able to revert this!!",
      TYPE.WARNING,
      'Yes ,Delete it',
      'Deleted successfully',
      'Document has been Deleted',
      TYPE.SUCCESS,
      () => {
        if (id !== undefined) {
          this.documentService.deleteDocument(id, empCode).subscribe({
            next: () => {
              this.getDocuments(1, 10);
            },
            error: (err) => {ErrorHandler.handle(err);
            },
          });
        }
      }
    );
  }

  GetDocument(id: number) {
    this.documentService.getById(id).subscribe({
      next: (response: GetDocumentById) => {
        this.getMasterDocumentById = response;
        this.doc = response;
        // response.displayDocumentName
      },
      error: (error) => {
        ErrorHandler.handle(error);
        
      },
    });
  }

  uploadFile(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files?.length) {
      // TODO check file size and type
      this.file = input.files[0];
    }

    if (!this.file) {
      Alert.toast(
        TYPE.WARNING,
        true,
        'Please select a file and fill the form correctly.'
      );
      return;
    }

    const allowedExtensions = [
      '.pdf',
      '.doc',
      '.docx',
      '.jpg',
      '.jpeg',
      '.png',
    ];
    const fileExtension = this.file.name
      .substring(this.file.name.lastIndexOf('.'))
      .toLowerCase();

    if (!allowedExtensions.includes(fileExtension)) {
      Alert.toast(
        TYPE.WARNING,
        true,
        'Unsupported file format. Allowed formats: .pdf, .doc, .docx, .jpg, .jpeg and .png.'
      );
      return;
    }

    if (this.file.size > 25 * 1048576) {
      Alert.toast(TYPE.WARNING, true, 'File too large. Max 25MB allowed.');
      return;
    }
  }
}
