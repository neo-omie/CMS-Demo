import { Component, OnInit, ElementRef, Renderer2, ViewChild, AfterViewInit, TemplateRef } from '@angular/core';
import { ApproverMatrixContractService } from '../../../services/approver-matrix-contract.service';
import { ApprovalMatrixContract } from '../../../models/approval-matrix-contract';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Alert } from '../../../utils/alert';
import { TYPE } from '../../auth/login/values.constants';
import { Title } from '@angular/platform-browser';
import { ApproverMatrixContractModalComponent } from '../approver-matrix-contract-modal/approver-matrix-contract-modal.component';
import { LoaderComponent } from '../../UtilComponents/loader/loader.component';
import { TableComponent } from '../../UtilComponents/table/table.component';
import { PaginationComponent } from '../../UtilComponents/pagination/pagination.component';
import { Pagination } from '../../../utils/pagination';
import { ErrorHandler } from '../../../utils/errorHandler';

@Component({
  selector: 'app-approval-matrix-contract-screen',
  standalone: true,
  imports: [
    TableComponent,
    ApproverMatrixContractModalComponent,
    CommonModule, LoaderComponent, FormsModule,
    PaginationComponent
  ],
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
  displayedColumns: string[] = ['departmentName', 'approverName1', 'approverName2', 'approverName3', 'action'];
  columnsInfo:{[key:string]:{
    'title' ?: string,
    'isSort' ?: boolean,
    'templateRef' : TemplateRef<any> | null,
  }} = {};

  @ViewChild('actionTemplateRef', { static: true }) actionTemplateRef!: TemplateRef<any>;

  constructor(private approverMatrixContractService: ApproverMatrixContractService, private title: Title) {
    this.title.setTitle("Approval Matrix (Contract) - CMS");
  }

  ngOnInit() {
    this.GetApprovalMatrixContract(1, 10);
    this.columnsInfo = {
      'departmentName': {
        'title': 'Department',
        'isSort': true,
        'templateRef': null
      },
      'approverName1': {
        'title': 'Approver 1',
        'isSort': true,
        'templateRef': null
      },
      'approverName2': {
        'title': 'Approver 2',
        'isSort': true,
        'templateRef': null
      },
      'approverName3': {
        'title': 'Approver 3',
        'isSort': true,
        'templateRef': null
      },
      'action': {
        'title': 'Action',
        'templateRef': this.actionTemplateRef
      }
    };
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
                                   ErrorHandler.handle(error);

      }
    });
  }

  GetPage(pgNumber: number) {
    if (this.maxPage >= pgNumber && pgNumber >= 1) {
      this.GetApprovalMatrixContract(pgNumber, 10);
    }
  }

  GetContract(id: number, isEdit: boolean) {
    this.isEdit = isEdit;
    this.approverMatrixContractService.GetApprovalMatrixContractById(id).subscribe({
      next: (response: ApprovalMatrixContract) => {
        this.approvalMatrixContract = response;
      },
      error: (error) => {
                                   ErrorHandler.handle(error);

      }
    });
  }
}