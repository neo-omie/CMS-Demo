import { Component, OnInit } from '@angular/core';
import { MasterDocumentService } from '../../services/master-document.service';
import { Router, RouterModule } from '@angular/router';
import { AddDocumentDto, MasterDocument, MasterDocumentDto } from '../../models/master-document';
import { FormsModule, NgForm } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { LoaderComponent } from '../loader/loader.component';
import { DocumentStatus } from '../../constants';
import { Alert } from '../../utils/alert';
import { TYPE } from '../auth/login/values.constants';

@Component({
  selector: 'app-master-document',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterModule,LoaderComponent],
  templateUrl: './master-document.component.html',
  styleUrl: './master-document.component.css'
})
export class MasterDocumentComponent implements OnInit{
  loading:boolean = true;
  maxPage=1;
  pageNumbers = [1,1,2,3,4,5];
  masterDocuments?: MasterDocumentDto 
  documentStatus = DocumentStatus;
  document:AddDocumentDto = new AddDocumentDto('',0);

  ngOnInit(): void {
    this.getDocuments(1,10);
  }

  constructor(private documentService:MasterDocumentService,private router:Router)
{}
getDocuments(pageNumber : number, pageSize : number):void{
  this.documentService.getDocument(pageNumber , pageSize ).subscribe((res)=>{
    this.loading = false;
    this.masterDocuments=res;
    this.pageNumbers[0] = pageNumber;
        if(this.masterDocuments.documents.length > 0){
          if(pageNumber == 1){
            this.maxPage = Math.ceil(this.masterDocuments.totalCount/10);
          }
          let diff = this.maxPage - pageNumber;
          if(diff >= 0 && this.maxPage >= 5){
            if(this.pageNumbers[0] == 1){
              this.pageNumbers[1] = pageNumber;
              this.pageNumbers[2] = pageNumber + 1;
              this.pageNumbers[3] = pageNumber + 2;
              this.pageNumbers[4] = pageNumber + 3;
              this.pageNumbers[5] = pageNumber + 4;
            }
            else if(this.pageNumbers[0] == 2){
              this.pageNumbers[1] = pageNumber - 1;
              this.pageNumbers[2] = pageNumber;
              this.pageNumbers[3] = pageNumber + 1;
              this.pageNumbers[4] = pageNumber + 2;
              this.pageNumbers[5] = pageNumber + 3;
            }
            else if(diff == 0){
              this.pageNumbers[1] = pageNumber - 4;
              this.pageNumbers[2] = pageNumber - 3;
              this.pageNumbers[3] = pageNumber - 2;
              this.pageNumbers[4] = pageNumber - 1;
              this.pageNumbers[5] = pageNumber;
            }
            else if(diff == 1){
              this.pageNumbers[1] = pageNumber - 3;
              this.pageNumbers[2] = pageNumber - 2;
              this.pageNumbers[3] = pageNumber - 1;
              this.pageNumbers[4] = pageNumber;
              this.pageNumbers[5] = pageNumber + 1;
            }
            else{
              this.pageNumbers[1] = pageNumber - 2;
              this.pageNumbers[2] = pageNumber - 1;
              this.pageNumbers[3] = pageNumber;
              this.pageNumbers[4] = pageNumber + 1;
              this.pageNumbers[5] = pageNumber + 2;
            }
          }
        }
      }
  );
}
GetPage(pgNumber:number){
  if(this.maxPage >= pgNumber && pgNumber >= 1){
    this.getDocuments(pgNumber, 10);
  }
}

 addDocument(documentForm :NgForm){
  this.document = documentForm.value;
  this.document.status = Number(this.document.status);
  console.log(documentForm);
    
    this.documentService.addDocument(this.document).subscribe({
      next: (response) => {
        Alert.bigToast('Success!','Document added successfully.',TYPE.SUCCESS,'Ok');
        documentForm.resetForm();
        this.GetPage(this.maxPage);
      },
      error: (error) => {
        console.error('Error adding Document:', error);
        Alert.bigToast('Error!','There was an error adding the Document.',TYPE.ERROR,'Try Again');
      }
    });
 }

 updateDocument(updateDocForm : NgForm){
  this.document = updateDocForm.value;
  this.document.status = Number(this.document.status);

  this.documentService.updateDocument(this.document).subscribe({
    next: (response) => {
      Alert.bigToast('Success!','Document updated successfully.',TYPE.SUCCESS,'Ok');
      updateDocForm.resetForm();
      this.GetPage(this.maxPage);
    },
    error: (error) => {
      console.error('Error updating Document:', error);
      Alert.bigToast('Error!','There was an error adding the Document.',TYPE.ERROR,'Try Again');
    }
  });
}
}
