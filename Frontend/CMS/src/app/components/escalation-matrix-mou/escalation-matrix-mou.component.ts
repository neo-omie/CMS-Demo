import { CommonModule } from '@angular/common';
import { Component, ElementRef, Renderer2, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { LoaderComponent } from '../loader/loader.component';
import { MasterEscalationMatrixMouDto, UpdateMatrixMouDto } from '../../models/master-escalation-matrix-mou-dto';
import { EscalationMatrixMouService } from '../../services/escalation-matrix-mou.service';
import { Pagination } from '../../utils/pagination';
import { MasterEmployee } from '../../models/master-employee';
import { TYPE } from '../auth/login/values.constants';
import { Alert } from '../../utils/alert';
import { ApproverMatrixContractService } from '../../services/approver-matrix-contract.service';
import { Title } from '@angular/platform-browser';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-escalation-matrix-mou',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterModule, LoaderComponent, MatTableModule, MatSortModule, MatFormFieldModule, MatInputModule],
  templateUrl: './escalation-matrix-mou.component.html',
  styleUrl: './escalation-matrix-mou.component.css'
})
export class EscalationMatrixMouComponent {
  loading: boolean = true;
  displayedColumns: string[] = ['departmentName', 'escalation1', 'escalation2', 'escalation3', 'action'];
  dataSource = new MatTableDataSource<MasterEscalationMatrixMouDto>();
  @ViewChild(MatSort) sort!: MatSort;
  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
  }
  approvers1:MasterEmployee[] = [];
  approvers2:MasterEmployee[] = [];
  approvers3:MasterEmployee[] = [];
  @ViewChild('editApproverCollapse1') editApproverCollapse1!: ElementRef;
  @ViewChild('editApproverCollapse2') editApproverCollapse2!: ElementRef;
  @ViewChild('editApproverCollapse3') editApproverCollapse3!: ElementRef;
  @ViewChild('editApproverName1') editApproverName1!: ElementRef;
  @ViewChild('editApproverName2') editApproverName2!: ElementRef;
  @ViewChild('editApproverName3') editApproverName3!: ElementRef;
  @ViewChild('editApproverId1') editApproverId1!: ElementRef;
  @ViewChild('editApproverId2') editApproverId2!: ElementRef;
  @ViewChild('editApproverId3') editApproverId3!: ElementRef;
  @ViewChild('editNumberOfDays1') editNumberOfDays1!: ElementRef;
  @ViewChild('editNumberOfDays2') editNumberOfDays2!: ElementRef;
  @ViewChild('editNumberOfDays3') editNumberOfDays3!: ElementRef;
  maxPage = 1;
  pageNumbers = [1, 1, 2, 3, 4, 5];
  matrixMous?: MasterEscalationMatrixMouDto[];
  escalationMatrixMou? : MasterEscalationMatrixMouDto;
  updateMatrixMou: UpdateMatrixMouDto = new UpdateMatrixMouDto(0,'','','','',0,0,0);
  errorMsg ?: string

  ngOnInit(): void {
    this.getMatrixMous(1, 10);
  }
  constructor(
    private escalationService: EscalationMatrixMouService,
    private router: Router,
    private renderer : Renderer2,
    private approverMatrixContractService : ApproverMatrixContractService,
    private title:Title
  ) {
    this.title.setTitle("Escalation Matrix (MOU) - CMS");
  }
  closeEditApproverCollapses() {
    this.renderer.removeClass(this.editApproverCollapse1.nativeElement,'show');
    this.renderer.removeClass(this.editApproverCollapse2.nativeElement,'show');
    this.renderer.removeClass(this.editApproverCollapse3.nativeElement,'show');
    this.escalationMatrixMou = undefined;
    this.approvers1.length = 0;
    this.approvers2.length = 0;
    this.approvers3.length = 0;
  }
  getMatrixMous(pageNumber: number, pageSize: number) {
    this.escalationService
      .getAllMatrixMou(pageNumber, pageSize)
      .subscribe((res) => {
        this.loading = false;
        this.dataSource.data = res;
          if (this.sort) {
            this.dataSource.sort = this.sort;
          }
        this.matrixMous = res;
        this.pageNumbers[0] = pageNumber;
        if (this.matrixMous != undefined && this.matrixMous.length > 0) {
          let result = Pagination.paginator(pageNumber,this.matrixMous[0].totalRecords,pageSize)
          this.maxPage = result.maxPage;
          this.pageNumbers = result.pageNumbers;
        }
      });
  }

  GetPage(pgNumber:number){
    if(this.maxPage >= pgNumber && pgNumber >= 1){
      this.getMatrixMous(pgNumber, 10);
    }
  }
  GetMatrixMouById(valueId:number){
    this.escalationService.getMatrixMouById(valueId).subscribe({
          next:(response:MasterEscalationMatrixMouDto) => {
            this.escalationMatrixMou = response;
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
  textChangeApprover(departmentId:number, event:Event, approverNumber:number){
      let input = event.target as HTMLInputElement;
      console.log("Department ID : ",departmentId)
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
    editApproverMatrixContractSubmit(id:number){
      let nod1 = this.editNumberOfDays1.nativeElement.value;
      let nod2 = this.editNumberOfDays2.nativeElement.value;
      let nod3 = this.editNumberOfDays3.nativeElement.value;
      let ap1 = this.editApproverId1.nativeElement.value;
      let ap2 = this.editApproverId2.nativeElement.value;
      let ap3 = this.editApproverId3.nativeElement.value;
      if(nod1 !== "" && Number(nod1) > 0 &&
      nod2 !== "" && Number(nod2) > 0 &&
      nod3 !== "" && Number(nod3) > 0){
        if(ap1 == ap2 || ap1 == ap3 || ap2 == ap3){
          Alert.toast(TYPE.ERROR,true,"Approvers cannot be same");
          return;
        }
        this.updateMatrixMou.escalationId1 = ap1;
        this.updateMatrixMou.escalationId2 = ap2;
        this.updateMatrixMou.escalationId3 = ap3;
        this.updateMatrixMou.triggerDaysEscalation1 = nod1;
        this.updateMatrixMou.triggerDaysEscalation2 = nod2;
        this.updateMatrixMou.triggerDaysEscalation3 = nod3;
        this.escalationService.postMatrixMouById(id,this.updateMatrixMou).subscribe({
          next:(response:any)=>{
            Alert.toast(TYPE.SUCCESS,true,response.message);
            this.getMatrixMous(1, 10);
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
