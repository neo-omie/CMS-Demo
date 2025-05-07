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
  MasterDocumentDto
} from '../../models/master-document';
import { FormsModule, NgForm } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { LoaderComponent } from '../loader/loader.component';
import { DocumentStatus } from '../../constants';
import { Alert } from '../../utils/alert';
import { TYPE } from '../auth/login/values.constants';
import { Pagination } from '../../utils/pagination';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';

@Component({
  selector: 'app-master-document',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterModule, LoaderComponent,MatTableModule, MatSortModule, MatFormFieldModule, MatInputModule],
  templateUrl: './master-document.component.html',
  styleUrl: './master-document.component.css',
})
export class MasterDocumentComponent implements OnInit {
   displayedColumns: string[] = ['displayDocumentName', 'status','action'];
      dataSource = new MatTableDataSource<MasterDocument>();
      @ViewChild(MatSort) sort!: MatSort;
      ngAfterViewInit() {
        this.dataSource.sort = this.sort;
      }
  file:File|null = null;
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
        this.dataSource.data= res.documents;
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
      this.addFile.nativeElement.value = "";
      this.document.file = null;
      this.document.status = 1;
      Alert.toast(TYPE.WARNING, true, "Please select a file and fill the form correctly.");
      return;
    }
    
    const allowedExtensions = ['.pdf', '.doc', '.docx', '.jpg', '.jpeg', '.png'];
    const fileExtension = this.file.name.substring(this.file.name.lastIndexOf('.')).toLowerCase();
  
    if (!allowedExtensions.includes(fileExtension)) {
      this.addFile.nativeElement.value = "";
      this.document.file = null;
      this.document.status = 1;
      Alert.toast(TYPE.WARNING, true, "Unsupported file format. Allowed formats: .pdf, .doc, .docx, .jpg, .jpeg and .png.");
      return;
    }
    
    if (this.file.size > 25 * 1048576) {
      this.addFile.nativeElement.value = "";
      this.document.file = null;
      this.document.status = 1;
      Alert.toast(TYPE.WARNING, true, "File too large. Max 25MB allowed.");
      return;
    }
    
    const formData = new FormData();
    formData.append('File',this.file)
    formData.append('Status',String(this.document.status))
    this.documentService.addDocument(formData).subscribe({
      next: (res) => {
        this.file = null;
        // documentForm.reset();
        this.addFile.nativeElement.value = "";
        this.document.file = null;
        this.document.status = 1;
        Alert.bigToast(
          'Success!',
          'Document added successfully.',
          TYPE.SUCCESS,
          'Ok'
        );
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
        this.file = null;
        // documentForm.reset();
        this.addFile.nativeElement.value = "";
        this.document.file = null;
        this.document.status = 1;
      },
    });
    this.file = null;
    // documentForm.reset();
    this.addFile.nativeElement.value = "";
    this.document.file = null;
    this.document.status = 1;
  }



  
    editDocument(id: number) {
      if(this.file){
        const formData = new FormData();
        formData.append('File',this.file)
        formData.append('Status',String(this.editDocumentStatus.nativeElement.value))
        this.documentService.checkDocumentExist(formData).subscribe({
          next: (res:boolean) => {
            if(res){
              Alert.confirmToast("Are you sure?",
                "File with this name already exist. Submitting will replace that file",
                TYPE.WARNING,
                "Add",
                "Updated successfully",
                "",
                TYPE.SUCCESS,
                () =>{
                  this.documentService.updateDocument(id, formData).subscribe({
                    next: (response: any) => {
                      if (response) {
                        this.getMasterDocumentById = undefined;
                        this.doc = undefined;
                        this.file = null;
                        Alert.toast(TYPE.SUCCESS, true, 'Document Updated Successfully');
                        this.getDocuments(1, 10);
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
              ) 
              this.editFile.nativeElement.value = "";
              this.getMasterDocumentById = undefined;
              this.doc = undefined;
              this.file = null;
            }
            else{
              this.documentService.updateDocument(id, formData).subscribe({
                next: (response: any) => {
                  if (response) {
                    this.getMasterDocumentById = undefined;
                    this.doc = undefined;
                    this.file = null;
                    Alert.toast(TYPE.SUCCESS, true, 'Document Updated Successfully');
                    this.getDocuments(1, 10);
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
          }, error: (error) => {
            console.error(error.error);
          }
        })
      }
      else{
        this.documentService.updateDocumentWithoutFille(id, {"status":this.editDocumentStatus.nativeElement.value}).subscribe({
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
          }
        });
      }
      this.getMasterDocumentById = undefined;
      this.doc = undefined;
      this.file = null;
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
              //Alert.toast(TYPE.SUCCESS, true, 'Document Deleted successfully');
              this.getDocuments(1, 10);
            },
            error: (err) => {
              Alert.toast(TYPE.ERROR, true, this.errorMsg);
              console.error(err)
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
              // response.displayDocumentName
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
            // TODO check file size and type
            this.file = input.files[0];
          }
          
          if (!this.file) {
            Alert.toast(TYPE.WARNING, true, "Please select a file and fill the form correctly.");
            return;
          }
          
          const allowedExtensions = ['.pdf', '.doc', '.docx', '.jpg', '.jpeg', '.png'];
          const fileExtension = this.file.name.substring(this.file.name.lastIndexOf('.')).toLowerCase();
        
          if (!allowedExtensions.includes(fileExtension)) {
            Alert.toast(TYPE.WARNING, true, "Unsupported file format. Allowed formats: .pdf, .doc, .docx, .jpg, .jpeg and .png.");
            return;
          }
          
          if (this.file.size > 25 * 1048576) {
            Alert.toast(TYPE.WARNING, true, "File too large. Max 25MB allowed.");
            return;
          }
        }
}
