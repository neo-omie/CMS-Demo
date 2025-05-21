import { Component, TemplateRef, ViewChild } from '@angular/core';
import { EscalationMatrixContract, GetMasterEscalationMatrixContractByIdDto, MasterEscalationMatrixContractDto } from '../../../models/escalation-matrix-contract';
import { Title } from '@angular/platform-browser';
import { EscalationMatrixContractService } from '../../../services/escalation-matrix-contract.service';
import { Pagination } from '../../../utils/pagination';
import { Alert } from '../../../utils/alert';
import { TYPE } from '../../auth/login/values.constants';
import { TableComponent } from '../../UtilComponents/table/table.component';
import { LoaderComponent } from '../../UtilComponents/loader/loader.component';
import { CommonModule } from '@angular/common';
import { PaginationComponent } from '../../UtilComponents/pagination/pagination.component';
import { EscalationMatrixContractModalComponent } from '../escalation-matrix-contract-modal/escalation-matrix-contract-modal.component';

@Component({
  selector: 'app-escalation-matrix-contract-screen',
  standalone: true,
  imports: [
    TableComponent,
    LoaderComponent,
    CommonModule,
    PaginationComponent,
    EscalationMatrixContractModalComponent
  ],
  templateUrl: './escalation-matrix-contract-screen.component.html',
  styleUrl: './escalation-matrix-contract-screen.component.css'
})
export class EscalationMatrixContractScreenComponent {
  loading: boolean = true;
  isEdit: boolean = false;
  maxPage: number = 1;
  errorMsg: string = "";
  matrixContract?: GetMasterEscalationMatrixContractByIdDto;
  pageNumbers: number[] = [];
  matrixContracts: EscalationMatrixContract[] = [];
  displayedColumns: string[] = ['departmentName', 'escalation1', 'escalation2', 'escalation3', 'action'];
  columnsInfo: {
    [key: string]: {
      'title'?: string,
      'isSort'?: boolean,
      'templateRef': TemplateRef<any> | null,
    }
  } = {};

  @ViewChild('actionTemplateRef', { static: true }) actionTemplateRef!: TemplateRef<any>;

  constructor(private escalationService: EscalationMatrixContractService, private title: Title) {
    this.title.setTitle("Escalation Matrix (Contract) - CMS");
  }

  ngOnInit() {
    this.getMatrixContracts(1, 10);
    this.columnsInfo = {
      'departmentName': {
        title: 'Department',
        isSort: true,
        templateRef: null
      },
      'escalation1': {
        title: 'Escalator 1',
        isSort: true,
        templateRef: null
      },
      'escalation2': {
        title: 'Escalator 2',
        isSort: true,
        templateRef: null
      },
      'escalation3': {
        title: 'Escalator 3',
        isSort: true,
        templateRef: null
      },
      'action': {
        title: 'Action',
        isSort: false,
        templateRef: this.actionTemplateRef
      }
    }
  }

  getMatrixContracts(pageNumber: number, pageSize: number) {
    this.escalationService
      .getAllMatrixContract(pageNumber, pageSize)
      .subscribe((res) => {
        this.loading = false;
        this.matrixContracts = res.getEscalationMatrixContractDto;
        if (this.matrixContracts != undefined && this.matrixContracts.length > 0) {
          let result = Pagination.paginator(pageNumber, res.totalCount, pageSize)
          this.maxPage = result.maxPage;
          this.pageNumbers = result.pageNumbers;
        }
      });
  }

  GetPage(pgNumber: number) {
    if (this.maxPage >= pgNumber && pgNumber >= 1) {
      this.getMatrixContracts(pgNumber, 10);
    }
  }

  GetMatrixMouById(valueId: number, isEdit: boolean) {
    this.isEdit = isEdit;
    this.escalationService.getMatrixContractById(valueId).subscribe({
      next: (response: GetMasterEscalationMatrixContractByIdDto) => {
        this.matrixContract = response;
      },
      error: (error) => {
        console.error('Error :(', error);
        this.errorMsg = JSON.stringify((error.message !== undefined) ? error.error.title : error.message);
        Alert.toast(TYPE.ERROR, true, this.errorMsg);
      }
    });
  }
}
