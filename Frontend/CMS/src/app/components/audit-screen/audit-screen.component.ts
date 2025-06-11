import {
  AfterViewInit,
  Component,
  ElementRef,
  OnInit,
  TemplateRef,
  ViewChild,
} from '@angular/core';
import { Audit } from '../../models/audit';
import { AuditReportService } from '../../services/audit-report.service';
import { TableComponent } from '../UtilComponents/table/table.component';
import { LoaderComponent } from '../UtilComponents/loader/loader.component';
import { CommonModule } from '@angular/common';
import { PaginationComponent } from '../UtilComponents/pagination/pagination.component';
import { Title } from '@angular/platform-browser';
import { Pagination } from '../../utils/pagination';
import { Alert } from '../../utils/alert';
import { TYPE } from '../auth/login/values.constants';
import { ExcelExport } from '../../utils/excelExport';
import { PDFExport } from '../../utils/pdfExport';
import { ErrorHandler } from '../../utils/errorHandler';

@Component({
  selector: 'app-audit-screen',
  standalone: true,
  imports: [TableComponent, LoaderComponent, CommonModule, PaginationComponent],
  templateUrl: './audit-screen.component.html',
  styleUrl: './audit-screen.component.css',
})
export class AuditScreenComponent implements OnInit, AfterViewInit {
  loading: boolean = true;
  maxPage: number = 1;
  pageNumbers: number[] = [];

  @ViewChild('logTime', { static: true }) logTime!: TemplateRef<any>;

  displayedColumns = [
    'tableName',
    'loggedBy',
    'logTime',
    'actionDescription',
    'statusName',
  ];
  columnsInfo: {
    [key: string]: {
      title?: string;
      isSort?: boolean;
      templateRef: TemplateRef<any> | null;
    };
  } = {};
  allAudit: Audit[] = [];
  errorMsg?: string;

  constructor(private auditService: AuditReportService, private title: Title) {
    this.title.setTitle('Audits  - CMS');
  }
  ngAfterViewInit(): void {}
  ngOnInit() {
    this.getAllAudit(1, 10);
    this.columnsInfo = {
      tableName: { isSort: true, title: 'Table Name', templateRef: null },
      loggedBy: { isSort: true, title: 'Emp Code', templateRef: null },
      logTime: {
        isSort: true,
        title: 'Date and Time',
        templateRef: this.logTime,
      },
      actionDescription: {
        isSort: true,
        title: 'Action Description',
        templateRef: null,
      },
      statusName: { isSort: true, title: 'Action', templateRef: null },
    };
  }

  GetPage(pgNumber: number) {
    if (this.maxPage >= pgNumber && pgNumber >= 1) {
      this.getAllAudit(pgNumber, 10);
    }
  }

  printToPDF() {
    // PDFExport.printToPDF(tableID, fileName);
    const selectedColumns = [
      'Table Name',
      'Emp Code',
      'Date and Time',
      'Action Description',
      'Action',
    ];
    PDFExport.printToPDF(
      'table',
      'CMS-ClassifiedContracts.pdf',
      selectedColumns
    );
  }

  exportToExcel(): void {
    const selectedColumns = [
      'Table Name',
      'Emp Code',
      'Date and Time',
      'Action Description',
      'Action',
    ];

    ExcelExport.printToExcel('table', 'CMS-Reports.xlsx', selectedColumns);
  }
  getAllAudit(pageNumber: number, pageSize: number) {
    this.auditService.getAllAudits(pageNumber, pageSize).subscribe({
      next: (res: Audit[]) => {
        this.loading = false;
        this.allAudit = res;
        console.log(res);
        if (this.allAudit != undefined && this.allAudit.length > 0) {
          let result = Pagination.paginator(
            pageNumber,
            this.allAudit[0].totalRecords,
            pageSize
          );
          this.maxPage = result.maxPage;
          this.pageNumbers = result.pageNumbers;
        }
      },
      error: (err) => {
        this.loading = false;
        ErrorHandler.handle(err);
      },
    });
  }
}
