import { Component, TemplateRef, ViewChild } from '@angular/core';
import { AddAddendumContract } from '../../../models/add-addendum-contract';
import { AddAddendumContractsService } from '../../../services/add-addendum-contracts.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { AddendumContract } from '../../../models/addendum-contract';
import { Pagination } from '../../../utils/pagination';
import { ErrorHandler } from '../../../utils/errorHandler';
import { TableComponent } from "../../UtilComponents/table/table.component";
import { PaginationComponent } from "../../UtilComponents/pagination/pagination.component";
import { LoaderComponent } from "../../UtilComponents/loader/loader.component";
import { PDFExport } from '../../../utils/pdfExport';
import { ExcelExport } from '../../../utils/excelExport';
import { CommonModule } from '@angular/common';
import { DecodeToken } from '../../../utils/decodeToken';
import { AddendumContractModalComponent } from "../addendum-contract-modal/addendum-contract-modal.component";

@Component({
  selector: 'app-addendum-contracts-screen',
  standalone: true,
  imports: [TableComponent, PaginationComponent, LoaderComponent, CommonModule, AddendumContractModalComponent],
  templateUrl: './addendum-contracts-screen.component.html',
  styleUrl: './addendum-contracts-screen.component.css'
})
export class AddendumContractsScreenComponent {
  loading: boolean = true;
  maxPage: number = 1;
  addAddendumContract?: AddAddendumContract;
  approverCheck: boolean = true;
  pageNumbers: number[] = [];
  addAddendumContracts: AddAddendumContract[] = [];
  displayedColumns: string[] = [
    'contractName',
    'addendumDate',
    'status',
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
  @ViewChild('statusTemplateRef', { static: true })
  statusTemplateRef!: TemplateRef<any>;
  @ViewChild('AddendumDateRef', { static: true })
  AddendumDateRef!: TemplateRef<any>;

  constructor(
    private addAddendumContractsService: AddAddendumContractsService,
    private router: Router,
    private title: Title,
    private route: ActivatedRoute
  ) {
    this.title.setTitle('Approval Matrix (Contract) - CMS');
  }

  ngOnInit() {
    this.route.params.subscribe((params) => {
      const paramValueId = params['contractId'];
      if (paramValueId) {
        if (Number(paramValueId) && paramValueId > 0) {
          this.GetAllAddendum(1, 10, paramValueId);
        } else {
          this.router.navigate(['page-not-found']);
        }
      } else {
        this.GetAllAddendum(1, 10);
      }
    });

    this.columnsInfo = {
      contractName: {
        title: 'Contract Name',
        isSort: true,
        templateRef: null,
      },
      addendumDate: {
        title: 'Addendum Date',
        isSort: true,
        templateRef: this.AddendumDateRef,
      },
      status: {
        title: 'Status',
        isSort: true,
        templateRef: this.statusTemplateRef,
      },
      action: {
        title: 'Action',
        templateRef: this.actionTemplateRef,
      },
    };
  }

  GetAllAddendum(pageNumber: number, pageSize: number, id: number = 0) {
    this.loading = true;
    this.addAddendumContractsService
      .GetAllAddendum(pageNumber, pageSize, id)
      .subscribe({
        next: (response: AddendumContract) => {
          this.loading = false;
          this.addAddendumContracts = response.data;
          if (
            this.addAddendumContracts != undefined &&
            this.addAddendumContracts.length > 0
          ) {
            let result = Pagination.paginator(
              pageNumber,
              response.totalCount,
              pageSize
            );
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
      this.GetAllAddendum(pgNumber, 10);
    }
  }

  fetchAddedumData(addedumId: number) {
    this.addAddendumContractsService
      .GetAddenduByAddendumId(addedumId)
      .subscribe({
        next: (response) => {
          this.addAddendumContract = response;
          if (
            (this.addAddendumContract.approver1Email == DecodeToken.email &&
              this.addAddendumContract.approver1Status == 1) ||
            (this.addAddendumContract.approver2Email == DecodeToken.email &&
              this.addAddendumContract.approver1Status == 2 &&
              this.addAddendumContract.approver2Status == 1) ||
            (this.addAddendumContract.approver3Email == DecodeToken.email &&
              this.addAddendumContract.approver1Status == 2 &&
              this.addAddendumContract.approver2Status == 2 &&
              this.addAddendumContract.approver3Status == 1)
          ) {
            this.approverCheck = true;
          } else {
            this.approverCheck = false;
          }
        },
        error: (error) => {
          this.loading = false;
          ErrorHandler.handle(error);
        },
      });
  }

  printToPDF() {
    const selectedColumns = ['Contract Name', 'Addendum Date', 'Status'];
    PDFExport.printToPDF(
      'table',
      'CMS-Addendum Contracts.pdf',
      selectedColumns
    );
  }

  exportToExcel(): void {
    const selectedColumns = ['Contract Name', 'Addendum Date', 'Status'];

    ExcelExport.printToExcel(
      'table',
      'CMS-ContractsAddendum.xlsx',
      selectedColumns
    );
  }

  setLoader2(event:boolean){
    this.loading = event
  }
}