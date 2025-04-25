import { Component, ElementRef, OnInit, Renderer2, ViewChild } from '@angular/core';
import { ApprovalMatrixMou, EditApprovalMatrixMOUDto } from '../../models/approval-matrix-mou';
import { ApprovalMatrixMouService } from '../../services/approval-matrix-mou.service';
import { CommonModule } from '@angular/common';
import { LoaderComponent } from '../loader/loader.component';
import { MasterEmployee } from '../../models/master-employee';
import { Pagination } from '../../utils/pagination';
import { TYPE } from '../auth/login/values.constants';
import { Alert } from '../../utils/alert';

@Component({
  selector: 'app-approval-matrix-mou-screen',
  standalone: true,
  imports: [CommonModule, LoaderComponent],
  templateUrl: './approval-matrix-mou-screen.component.html',
  styleUrl: './approval-matrix-mou-screen.component.css'
})
export class ApprovalMatrixMouScreenComponent implements OnInit {
  loading:boolean = true
  approvers1:MasterEmployee[] = [];
  approvers2:MasterEmployee[] = [];
  approvers3:MasterEmployee[] = [];
  pageNumbers:number[] = [];
  maxPage:number = 1;
  approvalMatrixMOUs:ApprovalMatrixMou[] = [];
  approvalMatrixMou?:ApprovalMatrixMou;
    errorMsg ?: string
    @ViewChild('editApproverCollapse1') editApproverCollapse1!: ElementRef;
    @ViewChild('editApproverCollapse2') editApproverCollapse2!: ElementRef;
    @ViewChild('editApproverCollapse3') editApproverCollapse3!: ElementRef;
    @ViewChild('editApproverName1') editApproverName1!: ElementRef;
    @ViewChild('editApproverName2') editApproverName2!: ElementRef;
    @ViewChild('editApproverName3') editApproverName3!: ElementRef;
    @ViewChild('editApproverId1') editApproverId1!: ElementRef;
    @ViewChild('editApproverId2') editApproverId2!: ElementRef;
    @ViewChild('editApproverId3') editApproverId3!: ElementRef;
    @ViewChild('editNumberOfDays') editNumberOfDays!: ElementRef;
    constructor(private approverMatrixMouService : ApprovalMatrixMouService, private renderer : Renderer2){}
    ngOnInit(){
      this.GetApprovalMatrixMOU(1 ,10);
    }
    closeEditApproverCollapses() {
        this.renderer.removeClass(this.editApproverCollapse1.nativeElement,'show');
        this.renderer.removeClass(this.editApproverCollapse2.nativeElement,'show');
        this.renderer.removeClass(this.editApproverCollapse3.nativeElement,'show');
        this.approvalMatrixMou = undefined;
        this.approvers1.length = 0;
        this.approvers2.length = 0;
        this.approvers3.length = 0;
    }
    GetApprovalMatrixMOU(pageNumber : number, pageSize : number){
      this.approverMatrixMouService.GetApprovalMatrixMOU(pageNumber, pageSize).subscribe({
        next:(response:ApprovalMatrixMou[]) => {
          this.loading = false;
          this.approvalMatrixMOUs = response;
          if(this.approvalMatrixMOUs != undefined && this.approvalMatrixMOUs.length > 0){
            let result = Pagination.paginator(pageNumber,this.approvalMatrixMOUs[0].totalRecords,pageSize)
            this.maxPage = result.maxPage;
            this.pageNumbers = result.pageNumbers;
          }
        }, 
        error:(error) => {
          this.loading = false;
          console.error('Error :(', error);
          this.errorMsg = JSON.stringify((error.message !== undefined)?error.error.title: error.message);
          Alert.toast(TYPE.ERROR,true,this.errorMsg);
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
          console.log(this.approvalMatrixMou.numberOfDays);
          console.log(this.approvalMatrixMou.departmentId);
          
        }, 
        error:(error) => {
          console.error('Error :(', error);
          this.errorMsg = JSON.stringify((error.message !== undefined)?error.error.title: error.message);
          Alert.toast(TYPE.ERROR,true,this.errorMsg);
      }});
    }
    textChangeApprover(departmentId:number, event:Event, approverNumber:number){
      let input = event.target as HTMLInputElement;
      this.approverMatrixMouService.GetApproversForInputText(departmentId,input.value).subscribe(
        {
          next:(response:MasterEmployee[]) => {
            if(approverNumber == 1){
              this.approvers1 = response;
            }
            else if(approverNumber == 2){
              this.approvers2 = response;
            }
            else if(approverNumber == 3){
              this.approvers3 = response;
            }
          },
          error:(error) => {
            console.error('Error :(', error);
            this.errorMsg = JSON.stringify((error.message !== undefined)?error.error.title: error.message);
            Alert.toast(TYPE.ERROR,true,this.errorMsg);
          }
        }
      )
    }
    fillApprover(approverId:string, approverName:string, inputNumber:number){
      if(inputNumber == 1){
        const input = this.editApproverCollapse1.nativeElement.querySelector('input');
        input.value = "";
        console.log(input.value);
        this.approvers1.length = 0;
        this.renderer.removeClass(this.editApproverCollapse1.nativeElement,'show');
        this.editApproverName1.nativeElement.value = approverName;
        this.editApproverId1.nativeElement.value = approverId;
      }
      else if(inputNumber == 2){
        const input = this.editApproverCollapse2.nativeElement.querySelector('input');
        input.value = "";
        this.approvers2.length = 0;
        this.renderer.removeClass(this.editApproverCollapse2.nativeElement,'show');
        this.editApproverName2.nativeElement.value = approverName;
        this.editApproverId2.nativeElement.value = approverId;
      }
      else if(inputNumber == 3){
        const input = this.editApproverCollapse3.nativeElement.querySelector('input');
        input.value = "";
        this.approvers3.length = 0;
        this.renderer.removeClass(this.editApproverCollapse3.nativeElement,'show');
        this.editApproverName3.nativeElement.value = approverName;
        this.editApproverId3.nativeElement.value = approverId;
      }
    }
    editApprovalMatrixMOUDto:EditApprovalMatrixMOUDto =new EditApprovalMatrixMOUDto("","","",0);
    editApproverMatrixMOUSubmit(id:number){
      let nod = this.editNumberOfDays.nativeElement.value;
      if(nod !== "" && Number(nod) > 0){
        console.log(nod);
        this.editApprovalMatrixMOUDto.approverId1 = this.editApproverId1.nativeElement.value;
        this.editApprovalMatrixMOUDto.approverId2 = this.editApproverId2.nativeElement.value;
        this.editApprovalMatrixMOUDto.approverId3 = this.editApproverId3.nativeElement.value;
        this.editApprovalMatrixMOUDto.numberOfDays = nod;
        this.approverMatrixMouService.EditApproverMatrixMOU(id,this.editApprovalMatrixMOUDto).subscribe({
          next:(response:boolean)=>{
            if(response){
              Alert.toast(TYPE.SUCCESS,true,"Updated successfully");
              this.GetApprovalMatrixMOU(1, 10);
            }
    
          },
          error:(error)=>{
            console.error('Error :(', error);
            this.errorMsg = JSON.stringify((error.message !== undefined)?error.error.title: error.message);
            Alert.toast(TYPE.ERROR,true,this.errorMsg);
          }
        })
      }
      else{
        Alert.toast(TYPE.ERROR,true,"Incorrect number of days");
      }
      this.closeEditApproverCollapses();
    }
}
