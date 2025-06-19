declare var bootstrap: any;

import {
  Component,
  ElementRef,
  Inject,
  OnInit,
  Renderer2,
  TemplateRef,
  ViewChild,
} from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { ClassifiedContractsService } from '../../../services/classified-contracts.service';
import {
  AddClassifiedContractDto,
  ClassifiedContracts,
  GetClassifiedContractByIdDto,
} from '../../../models/classified-contracts';
import { Pagination } from '../../../utils/pagination';
import { Alert } from '../../../utils/alert';
import { ExcelExport } from '../../../utils/excelExport';
import { TYPE } from '../../auth/login/values.constants';
import {
  dateBetweenValidator,
  dateRangeValidator,
  dateValidator,
} from '../../../utils/dateValidator';
import { CommonModule, DatePipe, DOCUMENT } from '@angular/common';
import {
  FormControl,
  FormGroup,
  FormsModule,
  NgForm,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ContractTypeMasterDTO } from '../../../models/contract-type-master';
import {
  MasterApostille,
  MasterApostilleDto,
} from '../../../models/master-apostille';
import { CompanyMasterDto } from '../../../models/master-company';
import { GetAllDepartmentsDto } from '../../../models/master-department';
import { MasterEmployee } from '../../../models/master-employee';
import { Title } from '@angular/platform-browser';
import { MasterApostilleService } from '../../../services/master-apostille.service';
import { PostTerminationNoticeUploadDTO } from '../../../models/post-termination-notice';
import { PDFExport } from '../../../utils/pdfExport';
import { ClassifiedPostTermination } from '../../../models/post-termination';
import {
  ClassifiedApproveRejectWithdrawalDTO,
  WithdrawNoticeUploadDTO,
} from '../../../models/notice-withdrawal';
import { NoticeWithdrawalService } from '../../../services/notice-withdrawal.service';
import { firstValueFrom } from 'rxjs/internal/firstValueFrom';
import { PostTerminationService } from '../../../services/post-termination.service';
import { LoaderComponent } from '../../UtilComponents/loader/loader.component';
import { TableComponent } from '../../UtilComponents/table/table.component';
import { PaginationComponent } from '../../UtilComponents/pagination/pagination.component';
import { AllClassifiedContractViewAddModalComponent } from '../all-classified-contract-view-add-modal/all-classified-contract-view-add-modal.component';
import { DecodeToken } from '../../../utils/decodeToken';
import { ContractStatus, Location } from '../../../utils/constants';
import { AllClassifiedContractPostTerminationModalComponent } from '../all-classified-contract-post-termination-modal/all-classified-contract-post-termination-modal.component';
import { ErrorHandler } from '../../../utils/errorHandler';

@Component({
  selector: 'app-all-classified-contract',
  standalone: true,
  imports: [
    AllClassifiedContractViewAddModalComponent,
    PaginationComponent,
    TableComponent,
    LoaderComponent,
    FormsModule,
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    AllClassifiedContractPostTerminationModalComponent,
    MatInputModule,
  ],
  templateUrl: './all-classified-contract.component.html',
  styleUrl: './all-classified-contract.component.css',
})
export class AllClassifiedContractComponent implements OnInit {
  loading2 = false;
  currentPgNumber: number = 1;
  isCreate: boolean = false;
  contractStatus = ContractStatus;
  locationSelect = Location;
  statusKeys = Object.keys(this.contractStatus);
  locationSelectKeys = Object.keys(this.locationSelect);
  columnsInfo: {
    [key: string]: {
      title?: string;
      isSort?: boolean;
      templateRef: TemplateRef<any> | null;
    };
  } = {};
  getEnum(key: string) {
    return this.locationSelect[key as keyof typeof this.locationSelect];
  }
  checkNotNaN(number: string) {
    if (isNaN(Number(number))) return false;
    return true;
  }
  displayedColumns: string[] = [
    'classifiedContractName',
    'contractType',
    'departmentName',
    'effectiveDate',
    'expiryDate',
    'toBeRenewedOn',
    'status',
    'renewalDueIn',
    'location',
    'action',
  ];

  @ViewChild('effectiveDateRef', { static: true })
  effectiveDateRef!: TemplateRef<any>;
  @ViewChild('expiryDateRef', { static: true })
  expiryDateRef!: TemplateRef<any>;
  @ViewChild('toBeRenewedOnRef', { static: true })
  toBeRenewedOnRef!: TemplateRef<any>;
  @ViewChild('addendumDateRef', { static: true })
  addendumDateRef!: TemplateRef<any>;
  @ViewChild('statusRef', { static: true }) statusRef!: TemplateRef<any>;
  @ViewChild('approvalPendingFromRef', { static: true })
  approvalPendingFromRef!: TemplateRef<any>;
  @ViewChild('renewalDueInRef', { static: true })
  renewalDueInRef!: TemplateRef<any>;
  @ViewChild('actionRef', { static: true }) actionRef!: TemplateRef<any>;
  addBtn: string = '';
  file: File | null = null;
  loading: boolean = true;
  maxPage = 1;
  pageNumbers = [1, 1, 2, 3, 4, 5];
  errorMsg: string = '';
  allContracts: ClassifiedContracts[] = [];
  contractDetails?: GetClassifiedContractByIdDto;
  approverCheck: boolean = true;
  terminationCheck: boolean = true;
  withdrawCheck: boolean = true;

  mode: any;
  deptID?: number;
  // Dropdowns
  employeeCustodians: MasterEmployee[] = [];
  departments: GetAllDepartmentsDto[] = [];
  contractTypes: ContractTypeMasterDTO[] = [];
  apostilleTypes: MasterApostille[] = [];
  companies: CompanyMasterDto[] = [];
  postTerm: PostTerminationNoticeUploadDTO = new PostTerminationNoticeUploadDTO(
    null,
    0,
    new Date(),
    ''
  );
  withdrawNotice: WithdrawNoticeUploadDTO = new WithdrawNoticeUploadDTO(
    null,
    ''
  );
  contIdForPostTerm?: number = 0;

  filterForm: FormGroup = new FormGroup({
    SearchTerm: new FormControl(null),
    FromDate: new FormControl(null),
    ToDate: new FormControl(null),
    ContractType: new FormControl(0),
    RenewalDueIn: new FormControl(-1),
    ContractStatus: new FormControl(0),
    Department: new FormControl(0),
    Location: new FormControl(''),
  });

  ngOnInit(): void {
    this.route.queryParamMap.subscribe((params) => {
      this.GetAllContracts({
        PageNumber: 1,
        PageSize: 10,
        SearchTerm: null,
        FromDate: null,
        ToDate: null,
        ContractType: null,
        RenewalDueIn: params.get('renewalIn'),
        ContractStatus: params.get('status'),
        Department: null,
        Location: null,
      });
      if (params.get('status')) {
        this.filterForm.patchValue({ ContractStatus: params.get('status') });
        this.filterForm.get('ContractStatus')?.disable();
      }
      if (params.get('renewalIn')) {
        this.filterForm.patchValue({ RenewalDueIn: params.get('renewalIn') });
        this.filterForm.get('RenewalDueIn')?.disable();
      }
    });
    this.getAllDepartments();
    this.getAllContractTypes();
    this.getAllApostilleTypes();
    this.getAllCompanies();
    this.columnsInfo = {
      classifiedContractName: {
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
      approvalPendingFrom: {
        title: 'Approval Pending From',
        isSort: true,
        templateRef: this.approvalPendingFromRef,
      },
      renewalContractPerson: {
        title: 'Renewal Contract Person',
        isSort: true,
        templateRef: null,
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
  }
  constructor(
    private route: ActivatedRoute,
    private contractsService: ClassifiedContractsService,
    private router: Router,
    private renderer: Renderer2,
    private title: Title,
    private masterApostilleService: MasterApostilleService,
    private postTermService: PostTerminationService,
    private noticeWithdrawalService: NoticeWithdrawalService,
    @Inject(DOCUMENT) private document: Document
  ) {
    this.title.setTitle('All Classified Contracts - CMS');
  }
  @ViewChild('editEmpCustodianCollapse') editEmpCustodianCollapse!: ElementRef;
  @ViewChild('editEmpCustodianName') editEmpCustodianName!: ElementRef;
  @ViewChild('editEmpCustodianId') editEmpCustodianId!: ElementRef;
  @ViewChild('addContractModal') addContractModal!: ElementRef;
  @ViewChild('addEmpCustodianName') addEmpCustodianName!: ElementRef;
  @ViewChild('addEmpCustodianId') addEmpCustodianId!: ElementRef;
  @ViewChild('addEmpCustodianCollapse') addEmpCustodianCollapse!: ElementRef;
  @ViewChild('addAddendumEmpCustodianName')
  addAddendumEmpCustodianName!: ElementRef;
  @ViewChild('addAddendumEmpCustodianCollapse')
  addAddendumEmpCustodianCollapse!: ElementRef;
  @ViewChild('addAddendumEmpCustodianId')
  addAddendumEmpCustodianId!: ElementRef;
  @ViewChild('addFile') addFile!: ElementRef;

  GetAllContracts(filter: any) {
    this.contractsService.getContracts(filter).subscribe({
      next: (res: ClassifiedContracts[]) => {
        this.loading = false;
        this.allContracts = res;
        if (this.allContracts != undefined && this.allContracts.length > 0) {
          let result = Pagination.paginator(
            filter.PageNumber,
            this.allContracts[0].totalRecords,
            filter.PageSize
          );
          this.maxPage = result.maxPage;
          this.pageNumbers = result.pageNumbers;
        }
      },
      error: (error) => {
        this.loading = false;
        ErrorHandler.handle(error);
      },
    });
  }
  GetPage(pgNumber: number) {
    if (this.maxPage >= pgNumber && pgNumber >= 1) {
      this.currentPgNumber = pgNumber;
      const searchTerm = this.filterForm.get('SearchTerm')?.value;
      const fromDate = this.filterForm.get('FromDate')?.value;
      const toDate = this.filterForm.get('ToDate')?.value;
      const contractType = this.filterForm.get('ContractType')?.value;
      const renewalDueIn = this.filterForm.get('RenewalDueIn')?.value;
      const contractStatus = this.filterForm.get('ContractStatus')?.value;
      const department = this.filterForm.get('Department')?.value;
      const location = this.filterForm.get('Location')?.value;
      this.GetAllContracts({
        PageNumber: pgNumber,
        PageSize: 10,
        SearchTerm: searchTerm == '' ? null : searchTerm,
        FromDate: fromDate == '' ? null : fromDate,
        ToDate: toDate == '' ? null : toDate,
        ContractType: contractType == 0 ? null : contractType,
        RenewalDueIn: renewalDueIn == -1 ? null : renewalDueIn,
        ContractStatus: contractStatus == 0 ? null : contractStatus,
        Department: department == 0 ? null : department,
        Location: location == '' ? null : location,
      });
    }
  }

  getAllDepartments() {
    const eCode = DecodeToken.ECode;
    if (eCode) {
      this.contractsService.GetDepartments(eCode).subscribe({
        next: (response: GetAllDepartmentsDto[]) => {
          this.departments = response;
        },
        error: (error) => {
          console.error('Error :(', error);
          ErrorHandler.handle(error);
        },
      });
    }
  }
  getAllContractTypes() {
    this.contractsService.GetContractTypes().subscribe({
      next: (response: ContractTypeMasterDTO[]) => {
        this.contractTypes = response;
      },
      error: (error) => {
        console.error('Error :(', error);
        ErrorHandler.handle(error);
      },
    });
  }
  getAllApostilleTypes() {
    this.masterApostilleService.getApostilles(1, 100).subscribe({
      next: (response: MasterApostilleDto) => {
        this.apostilleTypes = response.data;
      },
      error: (error) => {
        console.error('Error :(', error);
        ErrorHandler.handle(error);
      },
    });
  }
  getAllCompanies() {
    this.contractsService.GetCompanies().subscribe({
      next: (response: CompanyMasterDto[]) => {
        this.companies = response;
      },
      error: (error) => {
        console.error('Error :(', error);
        ErrorHandler.handle(error);
      },
    });
  }

  GetContract(contractID: number) {
    this.contractsService.getContractByID(contractID).subscribe({
      next: (response: GetClassifiedContractByIdDto) => {
        console.log(response);

        this.contractDetails = response;
        this.contractDetails.validFrom = response.validFrom
          ?.toString()
          .split('T')[0];
        this.contractDetails.validTill = response.validTill
          ?.toString()
          .split('T')[0];
        this.contractDetails.renewalFrom =
          response.renewalFrom != null
            ? response.renewalFrom?.toString().split('T')[0]
            : '';
        this.contractDetails.renewalTill =
          response.renewalTill != null
            ? response.renewalTill?.toString().split('T')[0]
            : '';
        this.contractDetails.addendumDate =
          response.addendumDate != null
            ? response.addendumDate?.toString().split('T')[0]
            : '';
        console.log(response);
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
        this.isCreate = false;
      },
      error: (error) => {
        console.error('Error :(', error);
        ErrorHandler.handle(error);
      },
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
              Alert.toast(TYPE.SUCCESS, true, 'Contract Deleted successfully');
              const searchTerm = this.filterForm.get('SearchTerm')?.value;
              const fromDate = this.filterForm.get('FromDate')?.value;
              const toDate = this.filterForm.get('ToDate')?.value;
              const contractType = this.filterForm.get('ContractType')?.value;
              const renewalDueIn = this.filterForm.get('RenewalDueIn')?.value;
              const contractStatus =
                this.filterForm.get('ContractStatus')?.value;
              const department = this.filterForm.get('Department')?.value;
              const location = this.filterForm.get('Location')?.value;
              this.GetAllContracts({
                PageNumber: this.currentPgNumber,
                PageSize: 10,
                SearchTerm: searchTerm == '' ? null : searchTerm,
                FromDate: fromDate == '' ? null : fromDate,
                ToDate: toDate == '' ? null : toDate,
                ContractType: contractType == 0 ? null : contractType,
                RenewalDueIn: renewalDueIn == -1 ? null : renewalDueIn,
                ContractStatus: contractStatus == 0 ? null : contractStatus,
                Department: department == 0 ? null : department,
                Location: location == '' ? null : location,
              });
            },
            error: (error) => {
              console.error('Deletion Failed', error);
              ErrorHandler.handle(error);
            },
          });
        }
      }
    );
  }

  onClick() {
    // this.router.navigate(['classifiedContracts/allContracts']);
    this.masterContractAddForm.reset({
      classifiedContractName: '',
      departmentId: '',
      contractWithCompanyId: '',
      contractTypeId: '',
      apostilleTypeId: '',
      actualDocRefNo: '',
      retainerContract: '',
      termsAndConditions: '',
      validFrom: '',
      validTill: '',
      addendumDate: '',
      approver1Status: '',

      approver2Status: '',

      approver3Status: '',

      skipApproval: true,
    });
  }

  masterContractAddForm = new FormGroup(
    {
      classifiedContractName: new FormControl('', [Validators.required]),
      departmentId: new FormControl('', [Validators.required]),
      contractWithCompanyId: new FormControl('', [Validators.required]),
      contractTypeId: new FormControl('', [Validators.required]),
      apostilleTypeId: new FormControl('', [Validators.required]),
      actualDocRefNo: new FormControl('', [Validators.required]),
      retainerContract: new FormControl('', [Validators.required]),
      termsAndConditions: new FormControl('', [Validators.required]),
      validFrom: new FormControl('', [Validators.required, dateValidator()]),
      validTill: new FormControl('', [Validators.required, dateValidator()]),
      renewalFrom: new FormControl('', [dateValidator()]),
      renewalTill: new FormControl('', [dateValidator()]),
      addendumDate: new FormControl(''),
      empCustodianId: new FormControl('', [Validators.required]),
      location: new FormControl('', [Validators.required]),
      approver1Status: new FormControl('1', [
        Validators.required,
        Validators.pattern('^[0-9]$'),
      ]),
      approver2Status: new FormControl('1', [
        Validators.required,
        Validators.pattern('^[0-9]$'),
      ]),
      approver3Status: new FormControl('1', [
        Validators.required,
        Validators.pattern('^[0-9]$'),
      ]),
      skipApproval: new FormControl(true, [Validators.required]),
    },
    [
      dateRangeValidator('validFrom', 'validTill'),
      dateRangeValidator('renewalFrom', 'renewalTill'),
      dateBetweenValidator('validFrom', 'renewalFrom', 'validTill'),
    ]
  );

  contID: number = 0;

  checkContractId = new FormGroup({
    contractId: new FormControl('', [Validators.required]),
  });

  uploadFile(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files?.length) {
      // TODO check file size and type
      this.file = input.files[0];
    }

    if (!this.file) {
      Alert.toast(
        TYPE.WARNING,
        true,
        'Please select a file and fill the form correctly.'
      );
      return;
    }

    const allowedExtensions = [
      '.pdf',
      '.doc',
      '.docx',
      '.jpg',
      '.jpeg',
      '.png',
    ];
    const fileExtension = this.file.name
      .substring(this.file.name.lastIndexOf('.'))
      .toLowerCase();

    if (!allowedExtensions.includes(fileExtension)) {
      Alert.toast(
        TYPE.WARNING,
        true,
        'Unsupported file format. Allowed formats: .pdf, .doc, .docx, .jpg, .jpeg and .png.'
      );
      return;
    }

    if (this.file.size > 25 * 1048576) {
      Alert.toast(TYPE.WARNING, true, 'File too large. Max 25MB allowed.');
      return;
    }
  }

  getContractIdforPostTerm(classifiedContractID?: string) {
    this.contIdForPostTerm = Number(classifiedContractID);
  }
  async approveRejectContract(id?: string, status?: number) {
    this.loading = true;
    console.log('came here 1');
    console.log('id and status', id, status);

    let email = DecodeToken.email;
    if (email) {
      try {
        const response = await firstValueFrom(
          this.contractsService.approveRejectContract(Number(id), email, status)
        );
        if (response !== false) {
          Alert.toast(TYPE.SUCCESS, true, 'Updated successfully');
          const searchTerm = this.filterForm.get('SearchTerm')?.value;
          const fromDate = this.filterForm.get('FromDate')?.value;
          const toDate = this.filterForm.get('ToDate')?.value;
          const contractType = this.filterForm.get('ContractType')?.value;
          const renewalDueIn = this.filterForm.get('RenewalDueIn')?.value;
          const contractStatus = this.filterForm.get('ContractStatus')?.value;
          const department = this.filterForm.get('Department')?.value;
          const location = this.filterForm.get('Location')?.value;
          this.GetAllContracts({
            PageNumber: this.currentPgNumber,
            PageSize: 10,
            SearchTerm: searchTerm == '' ? null : searchTerm,
            FromDate: fromDate == '' ? null : fromDate,
            ToDate: toDate == '' ? null : toDate,
            ContractType: contractType == 0 ? null : contractType,
            RenewalDueIn: renewalDueIn == -1 ? null : renewalDueIn,
            ContractStatus: contractStatus == 0 ? null : contractStatus,
            Department: department == 0 ? null : department,
            Location: location == '' ? null : location,
          });
        }
      } catch (error) {
        ErrorHandler.handle(error);
      } finally {
        this.loading = false;
      }
    } else {
      this.router.navigate(['/']);
    }
    this.loading = false;
  }
  //uploading the Post Termination Notice

  // Post Termination Notice
  postTermination: ClassifiedPostTermination = new ClassifiedPostTermination();
  statusTermOrReject?: number = 0;
  termStatus(status: number) {
    if (status == 7) {
      this.statusTermOrReject = 7;
    }
    if (status == 2) {
      this.statusTermOrReject = 2;
    }
  }
  postTerminationEmailForm = new FormGroup({
    emailSubject: new FormControl('', [Validators.required]),
    emailBody: new FormControl('', [Validators.required]),
  });
  async approveTerminateContract(contractId?: string) {
    if (this.postTerminationEmailForm.invalid) {
      this.postTerminationEmailForm.markAllAsTouched();
      return;
    } else {
      this.loading = true;
      const emailSubject = this.postTerminationEmailForm.value.emailSubject;
      const emailBody = this.postTerminationEmailForm.value.emailBody;
      console.log('ApproveTerminationClassifiedContract came here ');
      let email = DecodeToken.email;
      if (email) {
        try {
          this.postTermination.classifiedContractId = Number(contractId);
          this.postTermination.changeToStatus = this.statusTermOrReject;
          this.postTermination.emailSubject = emailSubject;
          this.postTermination.emailBody = emailBody;
          this.postTermination.employeeEmail = email;
          // console.log(this.postTermination);

          const response = await firstValueFrom(
            this.postTermService.ApproveTerminationClassifiedContract(
              this.postTermination
            )
          );
          if (response !== false) {
            Alert.toast(TYPE.SUCCESS, true, 'Updated successfully');
            const searchTerm = this.filterForm.get('SearchTerm')?.value;
            const fromDate = this.filterForm.get('FromDate')?.value;
            const toDate = this.filterForm.get('ToDate')?.value;
            const contractType = this.filterForm.get('ContractType')?.value;
            const renewalDueIn = this.filterForm.get('RenewalDueIn')?.value;
            const contractStatus = this.filterForm.get('ContractStatus')?.value;
            const department = this.filterForm.get('Department')?.value;
            const location = this.filterForm.get('Location')?.value;
            this.GetAllContracts({
              PageNumber: this.currentPgNumber,
              PageSize: 10,
              SearchTerm: searchTerm == '' ? null : searchTerm,
              FromDate: fromDate == '' ? null : fromDate,
              ToDate: toDate == '' ? null : toDate,
              ContractType: contractType == 0 ? null : contractType,
              RenewalDueIn: renewalDueIn == -1 ? null : renewalDueIn,
              ContractStatus: contractStatus == 0 ? null : contractStatus,
              Department: department == 0 ? null : department,
              Location: location == '' ? null : location,
            });
          }
        } catch (error) {
          ErrorHandler.handle(error);
        } finally {
          this.loading = false;
        }
      } else {
        this.router.navigate(['/']);
      }
    }
    this.loading = false;
  }

  //uploading the Withdrawal Notice
  OnSaveWithdrawalNotice(documentForm: NgForm) {
    console.log(documentForm.value);
    console.log(this.file);

    if (!this.file || !documentForm.valid) {
      this.addFile.nativeElement.value = '';
      this.withdrawNotice.file = null;
      this.withdrawNotice.Remark = '';
      Alert.toast(
        TYPE.WARNING,
        true,
        'Please select a file and fill the Form Correctly'
      );
      return;
    }
    const allowedExtensions = ['.pdf', '.doc', '.docx'];
    const fileExtension = this.file.name
      .substring(this.file.name.lastIndexOf('.'))
      .toLowerCase();

    if (!allowedExtensions.includes(fileExtension)) {
      this.addFile.nativeElement.value = '';
      this.withdrawNotice.file = null;
      this.withdrawNotice.Remark = '';
      Alert.toast(
        TYPE.WARNING,
        true,
        'Unsupported file format. Allowed formats: .pdf, .doc, .docx '
      );
      return;
    }
    if (this.file.size > 25 * 1048576) {
      this.addFile.nativeElement.value = '';
      this.withdrawNotice.file = null;
      Alert.toast(TYPE.WARNING, true, 'File too large. Max 25MB allowed.');
      return;
    }

    const formData = new FormData();
    formData.append('file', this.file);
    formData.append('contractId', String(this.contIdForPostTerm));
    formData.append('postTermId', String(1));
    formData.append('Remark', String(this.withdrawNotice.Remark));
    this.noticeWithdrawalService
      .AddClassifiedWithdrawalNotice(formData)
      .subscribe({
        next: (res) => {
          this.file = null;
          documentForm.reset();
          // this.addFile.nativeElement.value = "";
          //      this.postTerm.file=null;

          Alert.bigToast(
            'Success!',
            'Withdrawal Notice Added Successfully!',
            TYPE.SUCCESS,
            'Ok'
          );
          this.GetPage(this.maxPage);
        },
        error: (error) => {
          ErrorHandler.handle(error);
        },
      });
  }

  withdrawalNoticeEmailForm = new FormGroup({
    emailSubject: new FormControl('', [Validators.required]),
    emailBody: new FormControl('', [Validators.required]),
  });

  withdrawNoticeSend: ClassifiedApproveRejectWithdrawalDTO =
    new ClassifiedApproveRejectWithdrawalDTO();
  async approveWithdrawalNotice(contractId?: string) {
    if (this.withdrawalNoticeEmailForm.invalid) {
      this.withdrawalNoticeEmailForm.markAllAsTouched();
      return;
    } else {
      this.loading = true;
      const emailSubject = this.withdrawalNoticeEmailForm.value.emailSubject;
      const emailBody = this.withdrawalNoticeEmailForm.value.emailBody;
      console.log('approveWithdrawalNotice came here');
      let email = DecodeToken.email;
      if (email) {
        try {
          this.withdrawNoticeSend.classifiedContractId = Number(contractId);
          this.withdrawNoticeSend.changeToStatus = this.statusTermOrReject;
          this.withdrawNoticeSend.emailSubject = emailSubject;
          this.withdrawNoticeSend.emailBody = emailBody;
          this.withdrawNoticeSend.employeeEmail = email;
          console.log(this.postTermination);

          const response = await firstValueFrom(
            this.noticeWithdrawalService.ClassifiedApproveWithdrawalTermination(
              this.withdrawNoticeSend
            )
          );
          if (response !== false) {
            Alert.toast(TYPE.SUCCESS, true, 'Updated successfully');
            const searchTerm = this.filterForm.get('SearchTerm')?.value;
            const fromDate = this.filterForm.get('FromDate')?.value;
            const toDate = this.filterForm.get('ToDate')?.value;
            const contractType = this.filterForm.get('ContractType')?.value;
            const renewalDueIn = this.filterForm.get('RenewalDueIn')?.value;
            const contractStatus = this.filterForm.get('ContractStatus')?.value;
            const department = this.filterForm.get('Department')?.value;
            const location = this.filterForm.get('Location')?.value;
            this.GetAllContracts({
              PageNumber: this.currentPgNumber,
              PageSize: 10,
              SearchTerm: searchTerm == '' ? null : searchTerm,
              FromDate: fromDate == '' ? null : fromDate,
              ToDate: toDate == '' ? null : toDate,
              ContractType: contractType == 0 ? null : contractType,
              RenewalDueIn: renewalDueIn == -1 ? null : renewalDueIn,
              ContractStatus: contractStatus == 0 ? null : contractStatus,
              Department: department == 0 ? null : department,
              Location: location == '' ? null : location,
            });
          }
        } catch (error) {
          ErrorHandler.handle(error);
        } finally {
          this.loading = false;
        }
      } else {
        this.router.navigate(['/']);
      }
    }
    this.loading = false;
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
    ]; // Put the exact header text here
    PDFExport.printToPDF(
      'table',
      'CMS-ClassifiedContracts.pdf',
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
      'CMS-ClassifeidContracts.xlsx',
      selectedColumns
    );
  }
  setLoader(data: boolean) {
    this.loading2 = data;
  }

  onFilterSubmit() {
    const searchTerm = this.filterForm.get('SearchTerm')?.value;
    const fromDate = this.filterForm.get('FromDate')?.value;
    const toDate = this.filterForm.get('ToDate')?.value;
    const contractType = this.filterForm.get('ContractType')?.value;
    const renewalDueIn = this.filterForm.get('RenewalDueIn')?.value;
    const contractStatus = this.filterForm.get('ContractStatus')?.value;
    const department = this.filterForm.get('Department')?.value;
    const location = this.filterForm.get('Location')?.value;
    const hasAddendum = this.filterForm.get('HasAddendum')?.value;
    console.log(department, location, hasAddendum);
    this.GetAllContracts({
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
    });
  }
}
