import { AfterViewInit, Component, OnInit, TemplateRef } from '@angular/core';
import { Audit } from '../../models/audit';
import { AuditReportService } from '../../services/audit-report.service';
import { TableComponent } from '../UtilComponents/table/table.component';
import { LoaderComponent } from '../UtilComponents/loader/loader.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-audit-screen',
  standalone: true,
  imports: [TableComponent, LoaderComponent, CommonModule],
  templateUrl: './audit-screen.component.html',
  styleUrl: './audit-screen.component.css',
})
export class AuditScreenComponent implements OnInit, AfterViewInit {
  loading: boolean = true;
  displayedColumns = [
    'tableName',
    'loggedBy',
    'logTime',
    'actionDescription',
    'statusName',
  ];
  columnsInfo: {
    [key: string]: {
      'title'?: string,
      'isSort'?: boolean,
      'templateRef': TemplateRef<any> | null,
    }
  } = {};
  allAudit: Audit[] = [];

  constructor(private auditService: AuditReportService) {}
  ngAfterViewInit(): void {
    
  }
  ngOnInit(): void {
    this.GetAllAudit(1, 10);
    this.columnsInfo = { 'tableName': { isSort: true ,'title':'Table Name','templateRef':null},
    'loggedBy' :{ isSort: true ,'title':'Employee Code','templateRef':null},
    'logTime' :{ isSort: true ,'title':'Date and Time','templateRef':null},
    'actionDescription' :{ isSort: true ,'title':'Action Description','templateRef':null},
    'statusName' :{ isSort: true ,'title':'Action','templateRef':null}};
  }

  GetAllAudit(pageNumber: Number, pageSize: Number) {
    this.auditService.getAllAudits(pageNumber, pageSize).subscribe({
      next: (res: Audit[]) => {
        this.loading = false;
        this.allAudit = res;
        // console.log(res);
      },
      error:(err)=>{
        console.log(err);
        
      }
    });
  }
}
