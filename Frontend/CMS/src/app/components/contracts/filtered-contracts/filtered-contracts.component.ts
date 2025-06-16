import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { TableComponent } from "../../UtilComponents/table/table.component";
import { Title } from '@angular/platform-browser';
import { ContractsEntity, GetContractByIdDto } from '../../../models/contracts';
import { ContractsService } from '../../../services/contracts.service';
import { Pagination } from '../../../utils/pagination';
import { ExcelExport } from '../../../utils/excelExport';
import { PDFExport } from '../../../utils/pdfExport';
import { ErrorHandler } from '../../../utils/errorHandler';
import { FormGroup, FormControl, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { PaginationComponent } from "../../UtilComponents/pagination/pagination.component";
import { MasterApostilleService } from '../../../services/master-apostille.service';
import { MasterApostille, MasterApostilleDto } from '../../../models/master-apostille';
import { LoaderComponent } from "../../UtilComponents/loader/loader.component";
import { GetAllDepartmentsDto } from '../../../models/master-department';
import { ContractTypeMasterDTO } from '../../../models/contract-type-master';
import { ContractStatus, Location } from '../../../utils/constants';
import { DecodeToken } from '../../../utils/decodeToken';
import { Alert } from '../../../utils/alert';
import { TYPE } from '../../auth/login/values.constants';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { CompanyMasterDto } from '../../../models/master-company';
import { ContractViewModalComponent } from "../contract-view-modal/contract-view-modal.component";
import { ContractAddModalComponent } from "../contract-add-modal/contract-add-modal.component";
import { ContractAddAddendumContractModalComponent } from "../contract-add-addendum-contract-modal/contract-add-addendum-contract-modal.component";
import { PostTerminationNoticeModalComponent } from "../post-termination-notice-modal/post-termination-notice-modal.component";
import { TerminationWithdrawNoticeComponent } from "../termination-withdraw-notice/termination-withdraw-notice.component";

@Component({
  selector: 'app-filtered-contracts',
  standalone: true,
  imports: [TableComponent, RouterModule, CommonModule, ReactiveFormsModule, PaginationComponent, LoaderComponent, ContractViewModalComponent, ContractAddModalComponent, ContractAddAddendumContractModalComponent, PostTerminationNoticeModalComponent, TerminationWithdrawNoticeComponent],
  templateUrl: './filtered-contracts.component.html',
  styleUrl: './filtered-contracts.component.css'
})
export class FilteredContractsComponent implements OnInit {
  loading: boolean = true;
  approverCheck: boolean = false;
  terminationCheck: boolean = false;
  withdrawCheck: boolean = false;
  currentPage: number = 1
  maxPage: number = 1;
  statusTermOrReject: number = 0;
  mailType: string = ''
  contractStatus = ContractStatus;
  statusKeys = Object.keys(ContractStatus);
  locationSelectKeys = Object.keys(Location);
  pageNumbers: number[] = [];
  contracts: ContractsEntity[] = [];
  departments: GetAllDepartmentsDto[] = [];
  contractTypes: ContractTypeMasterDTO[] = [];
  apostilleTypes: MasterApostille[] = [];
  companies: CompanyMasterDto[] = [];
  contractDetails: GetContractByIdDto = new GetContractByIdDto();
  filter: {
    PageNumber: number,
    PageSize: number,
    SearchTerm: string | null,
    FromDate: string | null,
    ToDate: string | null,
    ContractType: string | null,
    RenewalDueIn: string | null,
    ContractStatus: string | null,
    Department: string | null,
    Location: string | null,
    HasAddendum: boolean | null,
  } = {
      PageNumber: 1,
      PageSize: 10,
      SearchTerm: null,
      FromDate: null,
      ToDate: null,
      ContractType: null,
      RenewalDueIn: null,
      ContractStatus: null,
      Department: null,
      Location: null,
      HasAddendum: null,
    }
  columnsInfo: {
    [key: string]: {
      title?: string;
      isSort?: boolean;
      templateRef: TemplateRef<any> | null;
    };
  } = {};
  statusClass: string[] = [
    '',
    'warning',
    'success',
    'danger',
    'dark',
    'secondary',
    'warning',
    'warning',
    'primary'
  ];
  statusText: string[] = [
    '',
    'Pending Approval',
    'Active',
    'Rejected',
    'Terminated',
    'Expired',
    'Pending Termination',
    'Approved for Termination',
    'Pending Notice Withdrawal'
  ]
  displayedColumns: string[] = [
    'contractName',
    'contractType',
    'departmentName',
    'effectiveDate',
    'expiryDate',
    'toBeRenewedOn',
    'addendumDate',
    'status',
    'renewalDueIn',
    'location',
    'action',
  ];

  @ViewChild('effectiveDateRef', { static: true }) effectiveDateRef!: TemplateRef<any>;
  @ViewChild('expiryDateRef', { static: true }) expiryDateRef!: TemplateRef<any>;
  @ViewChild('toBeRenewedOnRef', { static: true }) toBeRenewedOnRef!: TemplateRef<any>;
  @ViewChild('addendumDateRef', { static: true }) addendumDateRef!: TemplateRef<any>;
  @ViewChild('statusRef', { static: true }) statusRef!: TemplateRef<any>;
  @ViewChild('renewalDueInRef', { static: true }) renewalDueInRef!: TemplateRef<any>;
  @ViewChild('actionRef', { static: true }) actionRef!: TemplateRef<any>;

  filterForm: FormGroup = new FormGroup({
    SearchTerm: new FormControl(null),
    FromDate: new FormControl(null),
    ToDate: new FormControl(null),
    ContractType: new FormControl(0),
    RenewalDueIn: new FormControl(-1),
    ContractStatus: new FormControl(0),
    Department: new FormControl(0),
    Location: new FormControl(''),
    HasAddendum: new FormControl(-1),
  });

  constructor(
    private contractsService: ContractsService,
    private router: Router,
    private route: ActivatedRoute,
    private title: Title,
    private masterApostilleService: MasterApostilleService
  ) {
    this.title.setTitle('All Contracts - CMS');
  }
  ngOnInit(): void {
    this.route.queryParamMap.subscribe((params) => {
      let renewalIn = params.get('renewalIn');
      let status = params.get('status');
      if (
        (!renewalIn && !status) ||
        (Number(renewalIn) &&
          (Number(renewalIn) == 0 ||
            Number(renewalIn) == 30 ||
            Number(renewalIn) == 60 ||
            Number(renewalIn) == 90)) ||
        (Number(status) &&
          Number(status) > 0 &&
          Number(status) < Object.keys(ContractStatus).length)
      ) {
        this.filter = {
          PageNumber: 1,
          PageSize: 10,
          SearchTerm: null,
          FromDate: null,
          ToDate: null,
          ContractType: null,
          RenewalDueIn: renewalIn,
          ContractStatus: status,
          Department: null,
          Location: null,
          HasAddendum: null,
        }
        this.GetAllContracts();
        if (status) {
          this.filterForm.patchValue({ ContractStatus: status });
          this.filterForm.get('ContractStatus')?.disable();
        }
        if (renewalIn) {
          this.filterForm.patchValue({ RenewalDueIn: renewalIn });
          this.filterForm.get('RenewalDueIn')?.disable();
        }
        this.columnsInfo = {
          contractName: {
            title: 'Contract Name',
            isSort: true,
            templateRef: null,
          },
          contractType: {
            title: 'Contract Type',
            isSort: true,
            templateRef: null,
          },
          departmentName: {
            title: 'Department Name',
            isSort: true,
            templateRef: null,
          },
          effectiveDate: {
            title: 'Effective Date',
            isSort: true,
            templateRef: this.effectiveDateRef,
          },
          expiryDate: {
            title: 'Expiry Date',
            isSort: true,
            templateRef: this.expiryDateRef,
          },
          toBeRenewedOn: {
            title: 'To Be Renewed On',
            isSort: true,
            templateRef: this.toBeRenewedOnRef,
          },
          addendumDate: {
            title: 'Addendum Date',
            isSort: true,
            templateRef: this.addendumDateRef,
          },
          status: {
            title: 'Status',
            isSort: true,
            templateRef: this.statusRef,
          },
          renewalDueIn: {
            title: 'Renewal Due In',
            isSort: false,
            templateRef: this.renewalDueInRef,
          },
          location: {
            title: 'Location',
            isSort: true,
            templateRef: null,
          },
          action: {
            title: 'Action',
            isSort: false,
            templateRef: this.actionRef,
          },
        };
        this.initialLoad();
      } else {
        this.router.navigate(['/pageNotFound']);
      }
    });

  }

  initialLoad() {
    const ECode = DecodeToken.ECode
    if(ECode){
      this.contractsService.GetDepartments(ECode).subscribe({
        next: (res: GetAllDepartmentsDto[]) => { this.departments = res; },
        error: (err) => { ErrorHandler.handle(err) }
      });
  
      this.contractsService.GetContractTypes().subscribe({
        next: (res: ContractTypeMasterDTO[]) => { this.contractTypes = res; },
        error: (err) => { ErrorHandler.handle(err) },
      });
  
      this.masterApostilleService.getApostilles(1, 100).subscribe({
        next: (res: MasterApostilleDto) => {
          this.apostilleTypes = res.data;
        },
        error: (err) => { ErrorHandler.handle(err) },
      });
  
      this.contractsService.GetCompanies().subscribe({
        next: (res: CompanyMasterDto[]) => { this.companies = res; },
        error: (err) => { ErrorHandler.handle(err) }
      });
    }
    else{
      DecodeToken.clearUserCredentials();
      localStorage.clear()
      this.router.navigate(['/'])
    }
  }

  getEnum = (key: string) => (Location[key as keyof typeof Location]);
  checkNotNaN = (number: string): boolean => (!isNaN(Number(number)));
  setLoader = (data: boolean) => { this.loading = data };
  setStatusTermOrReject = (data: number) => {this.statusTermOrReject = data};
  setMailType = (data: string) => {this.mailType = data};

  filterfield(name: string) {
    if (name === 'reset') {
      this.filterForm.reset({
        SearchTerm: null,
        FromDate: null,
        ToDate: null,
        ContractType: 0,
        RenewalDueIn: -1,
        ContractStatus: 0,
        Department: 0,
        Location: '',
        HasAddendum: -1,
      });
      return;
    } else {
      return this.filterForm.get(name)?.value;
    }
  }

  GetAllContracts() {
    const eCode = DecodeToken.ECode;
    if(eCode){
      this.contractsService.getContracts(this.filter,eCode).subscribe({
        next: (res: ContractsEntity[]) => {
          this.loading = false;
          this.contracts = res;
          if (this.contracts != undefined && this.contracts.length > 0) {
            let result = Pagination.paginator(
              this.filter.PageNumber,
              this.contracts[0].totalRecords,
              this.filter.PageSize
            );
            this.currentPage = this.filter.PageNumber;
            this.maxPage = result.maxPage;
            this.pageNumbers = result.pageNumbers;
          }
        },
        error: (error) => {
          this.loading = false;
          ErrorHandler.handle(error);
        }
      })
    }
  };

  GetPage(pgNumber: number) {
    if (this.maxPage >= pgNumber && pgNumber >= 1) {
      this.filter.PageNumber = pgNumber;
      this.GetAllContracts();
    }
  }

  GetContract(contractID: number) {
    this.contractsService.getContractByID(contractID).subscribe({
      next: (response: GetContractByIdDto) => {
        this.contractDetails = response;
        if (
          (this.contractDetails.approver1Email == DecodeToken.email &&
            this.contractDetails.approver1Status == 1) ||
          (this.contractDetails.approver2Email == DecodeToken.email &&
            this.contractDetails.approver1Status == 2 &&
            this.contractDetails.approver2Status == 1) ||
          (this.contractDetails.approver3Email == DecodeToken.email &&
            this.contractDetails.approver1Status == 2 &&
            this.contractDetails.approver2Status == 2 &&
            this.contractDetails.approver3Status == 1)
        ) {
          this.approverCheck = true;
        } else {
          this.approverCheck = false;
        }
        if (
          (this.contractDetails.approver1Email == DecodeToken.email &&
            this.contractDetails.approver1Status == 6) ||
          (this.contractDetails.approver2Email == DecodeToken.email &&
            this.contractDetails.approver1Status == 7 &&
            this.contractDetails.approver2Status == 6) ||
          (this.contractDetails.approver3Email == DecodeToken.email &&
            this.contractDetails.approver1Status == 7 &&
            this.contractDetails.approver2Status == 7 &&
            this.contractDetails.approver3Status == 6)
        ) {
          this.terminationCheck = true;
        } else {
          this.terminationCheck = false;
        }
        if (
          (this.contractDetails.approver1Email == DecodeToken.email &&
            this.contractDetails.approver1Status == 8) ||
          (this.contractDetails.approver2Email == DecodeToken.email &&
            this.contractDetails.approver1Status == 2 &&
            this.contractDetails.approver2Status == 8) ||
          (this.contractDetails.approver3Email == DecodeToken.email &&
            this.contractDetails.approver1Status == 2 &&
            this.contractDetails.approver2Status == 2 &&
            this.contractDetails.approver3Status == 8)
        ) {
          this.withdrawCheck = true;
        } else {
          this.withdrawCheck = false;
        }
      },
      error: (err) => { ErrorHandler.handle(err) }
    });
  }

  DeleteContract(id?: number) {
    let empName = DecodeToken.ECode;
    Alert.confirmToast(
      'Are you sure you want to delete this contract?',
      "You won't be able to revert this!!",
      TYPE.WARNING,
      'Yes ,Delete it',
      'Deleted Successfully',
      'Contract has been Deleted',
      TYPE.SUCCESS,
      () => {
        if (id !== undefined) {
          this.contractsService.deleteContract(id, empName).subscribe({
            next: () => {
              if(this.contracts.length == 1 && this.currentPage > 1){
                this.currentPage--;
              }
              this.GetPage(this.currentPage);
            },
            error: (err) => { ErrorHandler.handle(err) }
          });
        }
      }
    );
  }

  filterReset() {
    this.filterfield('reset');
    this.onFilterSubmit();
  }

  onFilterSubmit() {
    const searchTerm = this.filterfield('SearchTerm');
    const fromDate = this.filterfield('FromDate');
    const toDate = this.filterfield('ToDate');
    const contractType = this.filterfield('ContractType');
    const renewalDueIn = this.filterfield('RenewalDueIn');
    const contractStatus = this.filterfield('ContractStatus');
    const department = this.filterfield('Department');
    const location = this.filterfield('Location');
    const hasAddendum = this.filterfield('HasAddendum');
    this.filter = {
      PageNumber: 1,
      PageSize: 10,
      SearchTerm: searchTerm == '' ? null : searchTerm,
      FromDate: fromDate == '' ? null : fromDate,
      ToDate: toDate == '' ? null : toDate,
      ContractType: contractType == 0 ? null : contractType,
      RenewalDueIn: renewalDueIn == -1 ? null : renewalDueIn,
      ContractStatus: contractStatus == 0 ? null : contractStatus,
      Department: department == 0 ? null : department,
      Location: location == '' ? null : location,
      HasAddendum: hasAddendum == -1 ? null : hasAddendum == 1 ? true : false,
    }
    this.GetAllContracts();
  }

  printToPDF() {
    const selectedColumns = [
      'Contract Name',
      'Contract Type',
      'Department Name',
      'Effective Date',
      'Expiry Date',
      'To be renewed on',
      'Status',
      'Renewal Due In',
      'Location',
    ];
    PDFExport.printToPDF(
      'table',
      'CMS-Contracts.pdf',
      selectedColumns
    );
  }
  
  exportToExcel(): void {
    const selectedColumns = [
      'Contract Name',
      'Contract Type',
      'Department Name',
      'Effective Date',
      'Expiry Date',
      'To Be Renewed On',
      'Status',
      'Renewal Due In',
      'Location',
    ];
    ExcelExport.printToExcel(
      'table',
      'CMS-Contracts.xlsx',
      selectedColumns
    );
  }
}