import { Component, OnInit } from '@angular/core';
import { ApprovalMatrixMou } from '../../models/approval-matrix-mou';
import { ApprovalMatrixMouService } from '../../services/approval-matrix-mou.service';
import { CommonModule } from '@angular/common';
import { LoaderComponent } from '../loader/loader.component';

@Component({
  selector: 'app-approval-matrix-mou-screen',
  standalone: true,
  imports: [CommonModule, LoaderComponent],
  templateUrl: './approval-matrix-mou-screen.component.html',
  styleUrl: './approval-matrix-mou-screen.component.css'
})
export class ApprovalMatrixMouScreenComponent implements OnInit {
  loading:boolean = true
    pageNumbers:number[] = [1,1,2,3,4,5];
    maxPage:number = 1;
    approvalMatrixMOUs:ApprovalMatrixMou[] = [];
    approvalMatrixMou?:ApprovalMatrixMou; // = new ApprovalMatrixContract(-1,'',-1,'',-1,'',-1,'',-1,-1,0);
    errorMsg ?: string
    constructor(private approverMatrixMouService : ApprovalMatrixMouService){}
    ngOnInit(){
      this.GetApprovalMatrixMOU(1 ,10);
    }
    GetApprovalMatrixMOU(pageNumber : number, pageSize : number){
      this.approverMatrixMouService.GetApprovalMatrixMOU(pageNumber, pageSize).subscribe({
        next:(response:ApprovalMatrixMou[]) => {
          this.loading = false;
          this.approvalMatrixMOUs = response;
          this.pageNumbers[0] = pageNumber;
          if(this.approvalMatrixMOUs.length > 0){
            if(pageNumber == 1){
              this.maxPage = Math.ceil(this.approvalMatrixMOUs[0].totalRecords / 10);
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
        }, 
        error:(error) => {
          this.loading = false;
          console.error('Error :(', error);
          if(error.message !== undefined){
            this.errorMsg = JSON.stringify(error.error.message);
            console.log(this.errorMsg);
          }
          else{
            this.errorMsg = JSON.stringify(error.message);
            console.log(this.errorMsg);
          }
        }
      });
    }
    GetPage(pgNumber:number){
      if(this.maxPage >= pgNumber && pgNumber >= 1){
        this.GetApprovalMatrixMOU(pgNumber, 10);
      }
    }
    GetMOUContract(id:number){
      this.approverMatrixMouService.GetApprovalMatrixMOUById(id).subscribe({
        next:(response:ApprovalMatrixMou) => {
          this.approvalMatrixMou = response;
          console.log(response)
        }, 
        error:(error) => {
          console.error('Error :(', error);
          if(error.message !== undefined){
            this.errorMsg = JSON.stringify(error.error.message);
            console.log(this.errorMsg);
          }
          else{
            this.errorMsg = JSON.stringify(error.message);
            console.log(this.errorMsg);
          }
      }});
    }
}
