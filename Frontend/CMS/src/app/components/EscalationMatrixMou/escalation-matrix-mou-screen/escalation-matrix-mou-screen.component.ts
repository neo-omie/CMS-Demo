import { Component, TemplateRef, ViewChild } from '@angular/core';
import { MasterEscalationMatrixMouDto } from '../../../models/master-escalation-matrix-mou-dto';
import { EscalationMatrixMouService } from '../../../services/escalation-matrix-mou.service';
import { Title } from '@angular/platform-browser';
import { Pagination } from '../../../utils/pagination';
import { Alert } from '../../../utils/alert';
import { TYPE } from '../../auth/login/values.constants';
import { CommonModule } from '@angular/common';
import { EscalationMatrixMouModalComponent } from '../escalation-matrix-mou-modal/escalation-matrix-mou-modal.component';
import { LoaderComponent } from '../../UtilComponents/loader/loader.component';
import { TableComponent } from '../../UtilComponents/table/table.component';
import { PaginationComponent } from '../../UtilComponents/pagination/pagination.component';

@Component({
  selector: 'app-escalation-matrix-mou-screen',
  standalone: true,
  imports: [
    TableComponent,
    LoaderComponent,
    CommonModule,
    EscalationMatrixMouModalComponent,
    PaginationComponent,
  ],
  templateUrl: './escalation-matrix-mou-screen.component.html',
  styleUrl: './escalation-matrix-mou-screen.component.css',
})
export class EscalationMatrixMouScreenComponent {
  loading: boolean = true;
  isEdit: boolean = false;
  maxPage: number = 1;
  errorMsg: string = '';
  escalationMatrixMou?: MasterEscalationMatrixMouDto;
  pageNumbers: number[] = [];
  matrixMous: MasterEscalationMatrixMouDto[] = [];
  displayedColumns: string[] = [
    'departmentName',
    'escalation1',
    'escalation2',
    'escalation3',
    'action',
  ];
  columnsInfo: {
    [key: string]: {
      title?: string;
      isSort?: boolean;
      templateRef: TemplateRef<any> | null;
    };
  } = {};

  @ViewChild('actionTemplateRef', { static: true })
  actionTemplateRef!: TemplateRef<any>;

  constructor(
    private escalationService: EscalationMatrixMouService,
    private title: Title
  ) {
    this.title.setTitle('Escalation Matrix (MOU) - CMS');
  }

  ngOnInit() {
    this.getMatrixMous(1, 10);
    this.columnsInfo = {
      departmentName: {
        title: 'Department',
        isSort: true,
        templateRef: null,
      },
      escalation1: {
        title: 'Escalator 1',
        isSort: true,
        templateRef: null,
      },
      escalation2: {
        title: 'Escalator 2',
        isSort: true,
        templateRef: null,
      },
      escalation3: {
        title: 'Escalator 3',
        isSort: true,
        templateRef: null,
      },
      action: {
        title: 'Action',
        isSort: false,
        templateRef: this.actionTemplateRef,
      },
    };
  }

  getMatrixMous(pageNumber: number, pageSize: number) {
    this.escalationService
      .getAllMatrixMou(pageNumber, pageSize)
      .subscribe((res) => {
        this.loading = false;
        this.matrixMous = res;
        if (this.matrixMous != undefined && this.matrixMous.length > 0) {
          let result = Pagination.paginator(
            pageNumber,
            this.matrixMous[0].totalRecords,
            pageSize
          );
          this.maxPage = result.maxPage;
          this.pageNumbers = result.pageNumbers;
        }
      });
  }

  GetPage(pgNumber: number) {
    if (this.maxPage >= pgNumber && pgNumber >= 1) {
      this.getMatrixMous(pgNumber, 10);
    }
  }

  GetMatrixMouById(valueId: number, isEdit: boolean) {
    this.isEdit = isEdit;
    this.escalationService.getMatrixMouById(valueId).subscribe({
      next: (response: MasterEscalationMatrixMouDto) => {
        this.escalationMatrixMou = response;
      },
      error: (error) => {
        console.error('Error :(', error);
        if (error.status == 401) {
          let errmsg = error.error;
          Alert.toast(TYPE.ERROR, true, errmsg);
        } else {
          this.errorMsg = JSON.stringify(
            error.message !== undefined ? error.error.title : error.message
          );
          Alert.toast(TYPE.ERROR, true, this.errorMsg);
        }
      },
    });
  }
}
