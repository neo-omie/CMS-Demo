import { Component, TemplateRef, ViewChild } from '@angular/core';
import { ApprovalMatrixMou } from '../../../models/approval-matrix-mou';
import { Title } from '@angular/platform-browser';
import { ApprovalMatrixMouService } from '../../../services/approval-matrix-mou.service';
import { Pagination } from '../../../utils/pagination';
import { Alert } from '../../../utils/alert';
import { TYPE } from '../../auth/login/values.constants';
import { TableComponent } from '../../UtilComponents/table/table.component';
import { CommonModule } from '@angular/common';
import { LoaderComponent } from '../../UtilComponents/loader/loader.component';
import { FormsModule } from '@angular/forms';
import { PaginationComponent } from '../../UtilComponents/pagination/pagination.component';
import { ApprovalMatrixMouModalComponent } from '../approval-matrix-mou-modal/approval-matrix-mou-modal.component';

@Component({
  selector: 'app-approval-matrix-mou-screen',
  standalone: true,
  imports: [
    TableComponent,
    ApprovalMatrixMouModalComponent,
    CommonModule, LoaderComponent, FormsModule,
    PaginationComponent
  ],
  templateUrl: './approval-matrix-mou-screen.component.html',
  styleUrl: './approval-matrix-mou-screen.component.css'
})
export class ApprovalMatrixMouScreenComponent {
  loading: boolean = true;
  isEdit: boolean = false;
  maxPage: number = 1;
  errorMsg: string = "";
  approvalMatrixMOUs: ApprovalMatrixMou[] = [];
  approvalMatrixMOU?: ApprovalMatrixMou;
  pageNumbers: number[] = [];
  displayedColumns: string[] = ['departmentName', 'approverName1', 'approverName2', 'approverName3', 'action'];
  columnsInfo: {
    [key: string]: {
      'title'?: string,
      'isSort'?: boolean,
      'templateRef': TemplateRef<any> | null,
    }
  } = {};

  @ViewChild('actionTemplateRef', { static: true }) actionTemplateRef!: TemplateRef<any>;

  constructor(private approverMatrixMouService: ApprovalMatrixMouService, private title: Title) {
    this.title.setTitle("Approval Matrix (Contract) - CMS");
  }

  ngOnInit() {
    this.GetApprovalMatrixMou(1, 10);
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

  GetApprovalMatrixMou(pageNumber: number, pageSize: number) {
    this.approverMatrixMouService.GetApprovalMatrixMOU(pageNumber, pageSize).subscribe({
      next: (response: ApprovalMatrixMou[]) => {
        this.loading = false;
        this.approvalMatrixMOUs = response;
        if (this.approvalMatrixMOUs != undefined && this.approvalMatrixMOUs.length > 0) {
          let result = Pagination.paginator(pageNumber, this.approvalMatrixMOUs[0].totalRecords, pageSize)
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
      this.GetApprovalMatrixMou(pgNumber, 10);
    }
  }

  GetMou(id: number, isEdit: boolean) {
    this.isEdit = isEdit;
    this.approverMatrixMouService.GetApprovalMatrixMOUById(id).subscribe({
      next: (response: ApprovalMatrixMou) => {
        this.approvalMatrixMOU = response;
      },
      error: (error) => {
        console.error('Error :(', error);
        this.errorMsg = JSON.stringify((error.message !== undefined) ? error.error.title : error.message);
        Alert.toast(TYPE.ERROR, true, this.errorMsg);
      }
    });
  }
}
