import { Component, OnInit, ElementRef, Renderer2, ViewChild, AfterViewInit } from '@angular/core';
import { ApproverMatrixContractService } from '../../../services/approver-matrix-contract.service';
import { ApprovalMatrixContract } from '../../../models/approval-matrix-contract';
import { CommonModule } from '@angular/common';
import { LoaderComponent } from '../../loader/loader.component';
import { FormsModule } from '@angular/forms';
import { Alert } from '../../../utils/alert';
import { TYPE } from '../../auth/login/values.constants';
import { Pagination } from '../../../utils/pagination';
import { Title } from '@angular/platform-browser';
import { ApproverMatrixContractTableComponent } from '../approver-matrix-contract-table/approver-matrix-contract-table.component';
import { ApproverMatrixContractModalComponent } from '../approver-matrix-contract-modal/approver-matrix-contract-modal.component';

@Component({
  selector: 'app-approval-matrix-contract-screen',
  standalone: true,
  imports: [ApproverMatrixContractModalComponent ,ApproverMatrixContractTableComponent, CommonModule, LoaderComponent, FormsModule],
  templateUrl: './approval-matrix-contract-screen.component.html',
  styleUrl: './approval-matrix-contract-screen.component.css'
})
export class ApprovalMatrixContractScreenComponent implements OnInit {
  loading: boolean = true;
  isEdit: boolean = false;
  maxPage: number = 1;
  errorMsg: string = "";
  approvalMatrixContract?: ApprovalMatrixContract;
  pageNumbers: number[] = [];
  approvalMatrixContracts: ApprovalMatrixContract[] = [];

  constructor(private approverMatrixContractService: ApproverMatrixContractService, private renderer: Renderer2, private title: Title) {
    this.title.setTitle("Approval Matrix (Contract) - CMS");
  }

  ngOnInit() {
    this.GetApprovalMatrixContract(1, 10);
  }
  
  GetApprovalMatrixContract(pageNumber: number, pageSize: number) {
    this.approverMatrixContractService.GetApprovalMatrixContract(pageNumber, pageSize).subscribe({
      next: (response: ApprovalMatrixContract[]) => {
        this.loading = false;
        this.approvalMatrixContracts = response;
        if (this.approvalMatrixContracts != undefined && this.approvalMatrixContracts.length > 0) {
          let result = Pagination.paginator(pageNumber, this.approvalMatrixContracts[0].totalRecords, pageSize)
          this.maxPage = result.maxPage;
          this.pageNumbers = result.pageNumbers;
        }
      },
      error: (error) => {
        this.loading = false;
        console.error('Error :(', error);
        this.errorMsg = JSON.stringify((error.message !== undefined) ? error.error.title : error.message);
        Alert.toast(TYPE.ERROR, true, this.errorMsg);
      }
    });
  }

  GetPage(pgNumber: number) {
    if (this.maxPage >= pgNumber && pgNumber >= 1) {
      this.GetApprovalMatrixContract(pgNumber, 10);
    }
  }

  GetContract(id: number, isEdit:boolean) {
    this.isEdit = isEdit;
    this.approverMatrixContractService.GetApprovalMatrixContractById(id).subscribe({
      next: (response: ApprovalMatrixContract) => {
        this.approvalMatrixContract = response;
      },
      error: (error) => {
        console.error('Error :(', error);
        this.errorMsg = JSON.stringify((error.message !== undefined) ? error.error.title : error.message);
        Alert.toast(TYPE.ERROR, true, this.errorMsg);
      }
    });
  }
}