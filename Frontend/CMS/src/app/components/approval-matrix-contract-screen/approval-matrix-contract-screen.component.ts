import { Component, OnInit, ElementRef, Renderer2, ViewChild, AfterViewInit } from '@angular/core';
import { ApproverMatrixContractService } from '../../services/approver-matrix-contract.service';
import { ApprovalMatrixContract, EditApprovalMatrixContractDto } from '../../models/approval-matrix-contract';
import { CommonModule } from '@angular/common';
import { LoaderComponent } from '../loader/loader.component';
import { MasterEmployee } from '../../models/master-employee';
import { FormsModule } from '@angular/forms';
import { Alert } from '../../utils/alert';
import { TYPE } from '../auth/login/values.constants';
import { Pagination } from '../../utils/pagination';
import { MatTableModule } from '@angular/material/table';
import { MatSortModule } from '@angular/material/sort';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-approval-matrix-contract-screen',
  standalone: true,
  imports: [CommonModule,LoaderComponent,FormsModule,MatTableModule,MatSortModule,MatFormFieldModule,MatInputModule],
  templateUrl: './approval-matrix-contract-screen.component.html',
  styleUrl: './approval-matrix-contract-screen.component.css'
})
export class ApprovalMatrixContractScreenComponent implements OnInit {
  displayedColumns: string[] = ['departmentName', 'approverName1', 'approverName2', 'approverName3', 'action'];
  approvalMatrixContracts:ApprovalMatrixContract[] = [];
  dataSource = new MatTableDataSource<ApprovalMatrixContract>();
  @ViewChild(MatSort) sort!: MatSort;
  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
  }
  isView:boolean = false;
  approvers1:MasterEmployee[] = [];
  approvers2:MasterEmployee[] = [];
  approvers3:MasterEmployee[] = [];
  pageNumbers:number[] = [];
  loading:boolean = true;
  maxPage:number = 1;
  approvalMatrixContract?:ApprovalMatrixContract;
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
  constructor(private approverMatrixContractService : ApproverMatrixContractService, private renderer : Renderer2, private title:Title) {
    this.title.setTitle("Approval Matrix (Contract) - CMS");
  }
  ngOnInit(){
    this.GetApprovalMatrixContract(1,10);
  }
  closeEditApproverCollapses() {
      this.renderer.removeClass(this.editApproverCollapse1.nativeElement,'show');
      this.renderer.removeClass(this.editApproverCollapse2.nativeElement,'show');
      this.renderer.removeClass(this.editApproverCollapse3.nativeElement,'show');
      this.approvalMatrixContract = undefined;
      this.approvers1.length = 0;
      this.approvers2.length = 0;
      this.approvers3.length = 0;
  }
  GetApprovalMatrixContract(pageNumber : number, pageSize : number){
    this.approverMatrixContractService.GetApprovalMatrixContract(pageNumber, pageSize).subscribe({
      next:(response:ApprovalMatrixContract[]) => {
        this.loading = false;
        this.dataSource.data = response;
        if (this.sort) {
          this.dataSource.sort = this.sort;
        }
        this.approvalMatrixContracts = response;
        if(this.approvalMatrixContracts != undefined && this.approvalMatrixContracts.length > 0){
          let result = Pagination.paginator(pageNumber,this.approvalMatrixContracts[0].totalRecords,pageSize)
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
      this.GetApprovalMatrixContract(pgNumber, 10);
    }
  }
  GetContract(id:number,isView:boolean){
    this.isView = isView;
    this.approverMatrixContractService.GetApprovalMatrixContractById(id).subscribe({
      next:(response:ApprovalMatrixContract) => {
        this.approvalMatrixContract = response;
      }, 
      error:(error) => {
        console.error('Error :(', error);
        this.errorMsg = JSON.stringify((error.message !== undefined)?error.error.title: error.message);
        Alert.toast(TYPE.ERROR,true,this.errorMsg);
    }});
  }
  textChangeApprover(departmentId:number, event:Event, approverNumber:number){
    let input = event.target as HTMLInputElement;
    this.approverMatrixContractService.GetApproversForInputText(departmentId,input.value).subscribe(
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
  editApprovalMatrixContractDto:EditApprovalMatrixContractDto =new EditApprovalMatrixContractDto("","","",0);
  editApproverMatrixContractSubmit(id:number){
    let nod = this.editNumberOfDays.nativeElement.value;
    let ap1 = this.editApproverId1.nativeElement.value;
    let ap2 = this.editApproverId2.nativeElement.value;
    let ap3 = this.editApproverId3.nativeElement.value;
    if(nod !== "" && Number(nod) > 0){
      if(ap1 == ap2 || ap1 == ap3 || ap2 == ap3){
        Alert.toast(TYPE.ERROR,true,"Approvers cannot be same");
        return;
      }
      this.editApprovalMatrixContractDto.approverId1 = ap1;
      this.editApprovalMatrixContractDto.approverId2 = ap2;
      this.editApprovalMatrixContractDto.approverId3 = ap3;
      this.editApprovalMatrixContractDto.numberOfDays = nod;
      this.approverMatrixContractService.EditApproverMatrixContract(id,this.editApprovalMatrixContractDto).subscribe({
        next:(response:boolean)=>{
          if(response){
            Alert.toast(TYPE.SUCCESS,true,"Updated successfully");
            this.GetApprovalMatrixContract(1, 10);
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
  // getApproverList(){
  //   console.log("comming")
  // }
}