declare var bootstrap: any;
import { CommonModule, DOCUMENT } from '@angular/common';
import {
  Component,
  ElementRef,
  Inject,
  OnInit,
  Renderer2,
  ViewChild,
} from '@angular/core';
import {
  AbstractControl,
  FormControl,
  FormGroup,
  FormsModule,
  NgForm,
  ReactiveFormsModule,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import {
  AddContractDto,
  ContractsEntity,
  GetContractByIdDto,
} from '../../../models/contracts';
import { ContractsService } from '../../../services/contracts.service';
import { Pagination } from '../../../utils/pagination';
import { Alert } from '../../../utils/alert';
import { TYPE } from '../../auth/login/values.constants';
import { Title } from '@angular/platform-browser';
import { MasterEmployee } from '../../../models/master-employee';
import { GetAllDepartmentsDto } from '../../../models/master-department';
import { ContractTypeMasterDTO } from '../../../models/contract-type-master';
import {
  MasterApostille,
  MasterApostilleDto,
} from '../../../models/master-apostille';
import { CompanyMasterDto } from '../../../models/master-company';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MasterApostilleService } from '../../../services/master-apostille.service';
import { PostTerminationNoticeUploadDTO } from '../../../models/post-termination-notice';
import { PostTerminationService } from '../../../services/post-termination.service';
import { LoaderComponent } from '../../UtilComponents/loader/loader.component';
import { AddAddendumContractsService } from '../../../services/add-addendum-contracts.service';
import { AddAddendumContract } from '../../../models/add-addendum-contract';
import { firstValueFrom } from 'rxjs';
import { PDFExport } from '../../../utils/pdfExport';
import { PostTermination } from '../../../models/post-termination';
import {
  ApproveRejectWithdrawalDTO,
  WithdrawNoticeUploadDTO,
} from '../../../models/notice-withdrawal';
import { NoticeWithdrawalService } from '../../../services/notice-withdrawal.service';
import { DecodeToken } from '../../../utils/decodeToken';
import {
  ContractStatus,
  Location,
  MY_DATE_FORMATS,
} from '../../../utils/constants';
import { ExcelExport } from '../../../utils/excelExport';
import { ProgressBarComponent } from '../../UtilComponents/progress-bar/progress-bar.component';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import {
  dateBetweenValidator,
  dateRangeValidator,
  dateValidator,
} from '../../../utils/dateValidator';

@Component({
  selector: 'app-all-contracts',
  standalone: true,
  imports: [
    FormsModule,
    CommonModule,
    RouterModule,
    LoaderComponent,
    ReactiveFormsModule,
    MatTableModule,
    MatSortModule,
    ProgressBarComponent,
    MatDatepickerModule,
    MatFormFieldModule,
    MatInputModule,
    MatNativeDateModule,
  ],
  templateUrl: './all-contracts.component.html',
  styleUrl: './all-contracts.component.css',
})
export class AllContractsComponent implements OnInit {
  validDate = true;
  contractStatus = ContractStatus;
  locationSelect = Location;
  remarkTouched: boolean = false;
  statusKeys = Object.keys(this.contractStatus);
  locationSelectKeys = Object.keys(this.locationSelect);
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
  dataSource = new MatTableDataSource<ContractsEntity>();
  @ViewChild(MatSort) sort!: MatSort;
  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
  }
  addBtn: string = '';
  file: File | null = null;
  loading: boolean = true;
  maxPage = 1;
  pageNumbers = [1, 1, 2, 3, 4, 5];
  errorMsg: string = '';
  allContracts: ContractsEntity[] = [];
  contractDetails?: GetContractByIdDto;
  approverCheck: boolean = true;
  terminationCheck: boolean = true;
  withdrawCheck: boolean = true;
  mode: any;
  deptID?: number = 0;
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
    HasAddendum: new FormControl(-1),
  });
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
        console.log(Object.keys(ContractStatus).length);
        this.GetAllContracts({
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
        });
        if (status) {
          this.filterForm.patchValue({ ContractStatus: status });
          this.filterForm.get('ContractStatus')?.disable();
        }
        if (renewalIn) {
          this.filterForm.patchValue({ RenewalDueIn: renewalIn });
          this.filterForm.get('RenewalDueIn')?.disable();
        }
      } else {
        this.router.navigate(['/pageNotFound']);
      }
    });
    this.getAllDepartments();
    this.getAllContractTypes();
    this.getAllApostilleTypes();
    this.getAllCompanies();
  }
  Location = Location; // Makes the enum accessible in template
  locationOptions: string[];
  constructor(
    private contractsService: ContractsService,
    private router: Router,
    private route: ActivatedRoute,
    private renderer: Renderer2,
    private title: Title,
    private masterApostilleService: MasterApostilleService,
    private postTermService: PostTerminationService,
    private noticeWithdrawalService: NoticeWithdrawalService,
    private addAddendumContractsService: AddAddendumContractsService,
    @Inject(DOCUMENT) private document: Document
  ) {
    this.title.setTitle('All Contracts - CMS');
    this.locationOptions = Object.values(Location);
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
  getEnum(key: string) {
    return this.locationSelect[key as keyof typeof this.locationSelect];
  }
  checkNotNaN(number: string) {
    if (isNaN(Number(number))) return false;
    return true;
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

  GetAllContracts(filter: any) {
    console.log(filter);
    this.contractsService.getContracts(filter).subscribe({
      next: (res: ContractsEntity[]) => {
        this.loading = false;
        this.dataSource.data = res;
        console.log(this.dataSource.data);
        if (this.sort) {
          this.dataSource.sort = this.sort;
        }
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

  GetPage(pgNumber: number) {
    if (this.maxPage >= pgNumber && pgNumber >= 1) {
      const searchTerm = this.filterForm.get('SearchTerm')?.value;
      const fromDate = this.filterForm.get('FromDate')?.value;
      const toDate = this.filterForm.get('ToDate')?.value;
      const contractType = this.filterForm.get('ContractType')?.value;
      const renewalDueIn = this.filterForm.get('RenewalDueIn')?.value;
      const contractStatus = this.filterForm.get('ContractStatus')?.value;
      const department = this.filterForm.get('Department')?.value;
      const location = this.filterForm.get('Location')?.value;
      const hasAddendum = this.filterForm.get('HasAddendum')?.value;
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
        HasAddendum: hasAddendum == -1 ? null : hasAddendum,
      });
    }
  }

  GetContract(contractID: number) {
    this.contractsService.getContractByID(contractID).subscribe({
      next: (response: GetContractByIdDto) => {
        this.contractDetails = response;
        console.log(response);
        // Checking if the approver is the one who's logged in or not
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
      error: (error) => {
        console.error('Error :(', error);
        if (error.status == 401) {
          let errmsg = error.error;
          Alert.toast(TYPE.ERROR, true, errmsg);
        } else {
          if (error.message !== undefined) {
            this.errorMsg = JSON.stringify(error.error.message);
            console.log(this.errorMsg);
          } else {
            this.errorMsg = JSON.stringify(error.message);
            console.log(this.errorMsg);
          }
        }
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
              const searchTerm = this.filterForm.get('SearchTerm')?.value;
              const fromDate = this.filterForm.get('FromDate')?.value;
              const toDate = this.filterForm.get('ToDate')?.value;
              const contractType = this.filterForm.get('ContractType')?.value;
              const renewalDueIn = this.filterForm.get('RenewalDueIn')?.value;
              const contractStatus =
                this.filterForm.get('ContractStatus')?.value;
              const department = this.filterForm.get('Department')?.value;
              const location = this.filterForm.get('Location')?.value;
              const hasAddendum = this.filterForm.get('HasAddendum')?.value;
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
                Location: department == '' ? null : location,
                HasAddendum: hasAddendum == -1 ? null : hasAddendum,
              });
            },
            error: (error) => {
              console.error('Deletion Failed', error);
              if (error.status == 401) {
                let errmsg = error.error;
                Alert.toast(TYPE.ERROR, true, errmsg);
              } else {
                this.errorMsg = JSON.stringify(error.error.message);
                Alert.toast(TYPE.ERROR, true, this.errorMsg);
              }
            },
          });
        }
      }
    );
  }

  editContract(contract: ContractsEntity) {
    console.log(
      'Navigating to editContract with valueId:',
      contract.contractID
    );
    this.router.navigate(['contracts/editContract', contract.contractID]);
  }

  getAllDepartments() {
    this.contractsService.GetDepartments().subscribe({
      next: (response: GetAllDepartmentsDto[]) => {
        this.departments = response;
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
  getAllContractTypes() {
    this.contractsService.GetContractTypes().subscribe({
      next: (response: ContractTypeMasterDTO[]) => {
        this.contractTypes = response;
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
  getAllApostilleTypes() {
    this.masterApostilleService.getApostilles(1, 100).subscribe({
      next: (response: MasterApostilleDto) => {
        this.apostilleTypes = response.data;
        console.log(this.apostilleTypes);
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
  getAllCompanies() {
    this.contractsService.GetCompanies().subscribe({
      next: (response: CompanyMasterDto[]) => {
        this.companies = response;
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

  masterContractAddForm = new FormGroup(
    {
      contractName: new FormControl('', [
        Validators.required,
        Validators.maxLength(20),
      ]),
      departmentId: new FormControl('0', [Validators.required]),
      contractWithCompanyId: new FormControl('', [Validators.required]),
      contractTypeId: new FormControl('', [Validators.required]),
      apostilleTypeId: new FormControl('', [Validators.required]),
      actualDocRefNo: new FormControl('', [
        Validators.required,
        Validators.pattern('^[0-9]{1,10}$'),
      ]),
      retainerContract: new FormControl('', [Validators.required]),
      termsAndConditions: new FormControl('', [Validators.required]),
      validFrom: new FormControl('', [Validators.required, dateValidator()]),
      validTill: new FormControl('', [Validators.required, dateValidator()]),
      renewalFrom: new FormControl('', [Validators.required, dateValidator()]),
      renewalTill: new FormControl('', [Validators.required, dateValidator()]),
      addendumDate: new FormControl('', [dateValidator()]),
      empCustodianId: new FormControl('', [Validators.required]),
      location: new FormControl('', [
        Validators.required,
        Validators.pattern('^[A-Za-z]+$'),
      ]),
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
    },
    [
      dateRangeValidator('validFrom', 'validTill'),
      dateRangeValidator('renewalFrom', 'renewalTill'),
      dateBetweenValidator('validFrom', 'renewalFrom', 'validTill'),
    ]
  );

  async onAddFormSubmit() {
    let empName = DecodeToken.ECode;
    this.loading = true;
    this.masterContractAddForm
      .get('empCustodianId')
      ?.setValue(this.editEmpCustodianId.nativeElement.value);
    if (this.masterContractAddForm.invalid) {
      console.log('bhru Invalid form : ', this.masterContractAddForm.value);
      this.masterContractAddForm.markAllAsTouched();
      this.loading = false;
      // Alert.toast(TYPE.WARNING, true, 'There is still few fields to fill out. Please fill all the required fields.');
      return;
    } else {
      const departmentId = this.masterContractAddForm.value.departmentId;
      const contractWithCompanyId =
        this.masterContractAddForm.value.contractWithCompanyId;
      const contractTypeId = this.masterContractAddForm.value.contractTypeId;
      const apostilleTypeId = this.masterContractAddForm.value.apostilleTypeId;
      const actualDocRefNo = this.masterContractAddForm.value.actualDocRefNo;
      const retainerContract =
        this.masterContractAddForm.value.retainerContract;
      const empCustodianId = this.masterContractAddForm.value.empCustodianId;
      const approver1Status = this.masterContractAddForm.value.approver1Status;
      const approver2Status = this.masterContractAddForm.value.approver2Status;
      const approver3Status = this.masterContractAddForm.value.approver3Status;
      if (
        departmentId &&
        Number(departmentId) &&
        contractWithCompanyId &&
        Number(contractWithCompanyId) &&
        contractTypeId &&
        Number(contractTypeId) &&
        apostilleTypeId &&
        Number(apostilleTypeId) &&
        actualDocRefNo &&
        Number(actualDocRefNo) &&
        retainerContract &&
        Number(retainerContract) &&
        empCustodianId &&
        Number(empCustodianId) &&
        approver1Status &&
        Number(approver1Status) &&
        approver2Status &&
        Number(approver2Status) &&
        approver3Status &&
        Number(approver3Status)
      ) {
        const addFormValues: AddContractDto = new AddContractDto();
        addFormValues.contractName =
          this.masterContractAddForm.value.contractName;
        addFormValues.departmentId = Number(departmentId);
        addFormValues.contractWithCompanyId = Number(contractWithCompanyId);
        addFormValues.contractTypeId = Number(contractTypeId);
        addFormValues.apostilleTypeId = Number(apostilleTypeId);
        addFormValues.actualDocRefNo = Number(actualDocRefNo);
        addFormValues.retainerContract = Number(retainerContract);
        addFormValues.termsAndConditions =
          this.masterContractAddForm.value.termsAndConditions;
        addFormValues.validFrom = this.masterContractAddForm.value.validFrom;
        addFormValues.validTill = this.masterContractAddForm.value.validTill;
        addFormValues.renewalFrom =
          this.masterContractAddForm.value.renewalFrom;
        addFormValues.renewalTill =
          this.masterContractAddForm.value.renewalTill;
        // addFormValues.addendumDate = this.masterContractAddForm.value.renewalTill;
        addFormValues.empCustodianId = Number(empCustodianId);
        addFormValues.location = this.masterContractAddForm.value.location;
        addFormValues.approver1Status = Number(approver1Status);
        addFormValues.approver2Status = Number(approver2Status);
        addFormValues.approver3Status = Number(approver3Status);
        console.log(addFormValues);
        try {
          const response = await firstValueFrom(
            this.contractsService.addContract(addFormValues, empName)
          );
          if (response !== false) {
            Alert.toast(TYPE.SUCCESS, true, 'Added successfully');
            const searchTerm = this.filterForm.get('SearchTerm')?.value;
            const fromDate = this.filterForm.get('FromDate')?.value;
            const toDate = this.filterForm.get('ToDate')?.value;
            const contractType = this.filterForm.get('ContractType')?.value;
            const renewalDueIn = this.filterForm.get('RenewalDueIn')?.value;
            const contractStatus = this.filterForm.get('ContractStatus')?.value;
            const department = this.filterForm.get('Department')?.value;
            const location = this.filterForm.get('Location')?.value;
            const hasAddendum = this.filterForm.get('HasAddendum')?.value;
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
              HasAddendum: hasAddendum == -1 ? null : hasAddendum,
            });
            this.masterContractAddForm.reset();
            const modalElement = document.getElementById('contract-add');
            if (modalElement) {
              const modalInstance =
                bootstrap.Modal.getInstance(modalElement) ||
                new bootstrap.Modal(modalElement);
              modalInstance.hide();
            }
          }
        } catch (error) {
          console.error('Error :(', error);
          this.errorMsg = JSON.stringify(error); //?.error?.title ?? error.message
          Alert.toast(TYPE.ERROR, true, this.errorMsg);
        } finally {
          this.loading = false;
        }
      } else {
        console.log('should not come here ', this.masterContractAddForm.value);
        this.loading = false;
      }
    }
  }
  textChangeEmployeeCustodian(
    departmentId: number,
    event: Event,
    approverNumber: number
  ) {
    let input = event.target as HTMLInputElement;
    this.contractsService
      .GetEmployeeForInputText(departmentId, input.value)
      .subscribe({
        next: (response: MasterEmployee[]) => {
          if (approverNumber == 1) {
            this.employeeCustodians = response;
          }
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
  fillEmployeeCustodian(
    employeeId: number,
    employeeName: string,
    inputNumber: number
  ) {
    if (inputNumber == 1) {
      const input =
        this.editEmpCustodianCollapse.nativeElement.querySelector('input');
      input.value = '';
      console.log(input.value);
      this.employeeCustodians.length = 0;
      this.renderer.removeClass(
        this.editEmpCustodianCollapse.nativeElement,
        'show'
      );
      this.renderer.removeClass(
        this.addEmpCustodianCollapse.nativeElement,
        'show'
      );
      this.renderer.removeClass(
        this.addAddendumEmpCustodianCollapse.nativeElement,
        'show'
      );
      this.editEmpCustodianName.nativeElement.value = employeeName;
      this.editEmpCustodianId.nativeElement.value = employeeId;
      this.addEmpCustodianName.nativeElement.value = employeeName;
      this.addEmpCustodianId.nativeElement.value = employeeId;
      this.addAddendumEmpCustodianName.nativeElement.value = employeeName;
      this.addAddendumEmpCustodianId.nativeElement.value = employeeId;
      console.log(employeeId);
      console.log(this.addEmpCustodianCollapse.nativeElement.value);
    }
  }

  dateValidationForRenewalDueIn(cont: any): boolean {
    const today = new Date();
    const renewalTill = new Date(cont.toBeRenewedOn);
    if (today.getTime() <= renewalTill.getTime()) {
      return true;
    }
    return false;
  }

  get contractId() {
    return this.addaddendumForm.get('contractId');
  }
  get contractName() {
    return this.masterContractAddForm.get('contractName');
  }
  get departmentId() {
    return this.masterContractAddForm.get('departmentId');
  }
  get contractWithCompanyId() {
    return this.masterContractAddForm.get('contractWithCompanyId');
  }
  get contractTypeId() {
    return this.masterContractAddForm.get('contractTypeId');
  }
  get apostilleTypeId() {
    return this.masterContractAddForm.get('apostilleTypeId');
  }
  get actualDocRefNo() {
    return this.masterContractAddForm.get('actualDocRefNo');
  }
  get retainerContract() {
    return this.masterContractAddForm.get('retainerContract');
  }
  get termsAndConditions() {
    return this.masterContractAddForm.get('termsAndConditions');
  }
  get validFrom() {
    return this.masterContractAddForm.get('validFrom');
  }
  get validTill() {
    return this.masterContractAddForm.get('validTill');
  }
  get renewalFrom() {
    return this.masterContractAddForm.get('renewalFrom');
  }
  get renewalTill() {
    return this.masterContractAddForm.get('renewalTill');
  }
  get addendumDate() {
    return this.masterContractAddForm.get('addendumDate');
  }
  get empCustodianId() {
    return this.masterContractAddForm.get('empCustodianId');
  }
  get location() {
    return this.masterContractAddForm.get('location');
  }
  get approver1Status() {
    return this.masterContractAddForm.get('approver1Status');
  }
  get approver2Status() {
    return this.masterContractAddForm.get('approver2Status');
  }
  get approver3Status() {
    return this.masterContractAddForm.get('approver3Status');
  }

  onClick() {
    this.masterContractAddForm.reset({
      contractName: '',
      departmentId: '0',
      contractWithCompanyId: '',
      contractTypeId: '',
      apostilleTypeId: '',
      actualDocRefNo: '',
      retainerContract: '',
      termsAndConditions: '',
      validFrom: '',
      validTill: '',
      renewalFrom: '',
      renewalTill: '',
      // addendumDate: '',
      empCustodianId: '',
      location: '',
      approver1Status: '1',
      approver2Status: '1',
      approver3Status: '1',
    });
  }

  addaddendumForm = new FormGroup({
    addendumContractId: new FormControl('', [Validators.required]),
    contractId: new FormControl('', [Validators.required]),
    contractName: new FormControl('', [
      Validators.required,
      Validators.maxLength(20),
    ]),
    departmentId: new FormControl('', [Validators.required]),
    contractWithCompanyId: new FormControl('', [Validators.required]),
    contractTypeId: new FormControl('', [Validators.required]),
    apostilleTypeId: new FormControl('', [Validators.required]),
    actualDocRefNo: new FormControl('', [Validators.required]),
    retainerContract: new FormControl('', [Validators.required]),
    termsAndConditions: new FormControl('', [Validators.required]),
    validFrom: new FormControl('', [Validators.required]),
    validTill: new FormControl('', [Validators.required]),
    addendumDate: new FormControl('', [Validators.required]),
    empCustodianId: new FormControl('', [Validators.required]),
    location: new FormControl('', [Validators.required]),
  });

  contID: number = 0;

  fetchContractData(contractID?: string) {
    if (contractID != null) {
      this.addAddendumContractsService.fetchContractData(contractID).subscribe({
        next: (response) => {
          this.addaddendumForm.patchValue({
            contractId: String(contractID),
            contractName: String(response.contractName),
            departmentId: String(response.departmentId),
            contractWithCompanyId: String(response.contractWithCompanyId),
            contractTypeId: String(response.contractTypeId),
            apostilleTypeId: String(response.apostilleTypeId),
            actualDocRefNo: String(response.actualDocRefNo),
            retainerContract: String(response.retainerContract),
            termsAndConditions: response.termsAndConditions,
            validFrom: this.formatDate(String(response.validFrom)),
            validTill: this.formatDate(String(response.validTill)),
            empCustodianId: String(response.empCustodianId),
            location: String(response.location),
          });
          this.editEmpCustodianId.nativeElement.value = response.empCustodianId;
          this.editEmpCustodianName.nativeElement.value =
            response.empCustodianId;
          // console.log(response);
          return true;
        },
        error: (err) => {
          console.error('No Contract with this id exist', err);
          if (err.status == 401) {
            let errmsg = err.error;
            Alert.toast(TYPE.ERROR, true, errmsg);
          } else {
            this.errorMsg = JSON.stringify(
              err.message !== undefined ? err.error.message : err.message
            );
            Alert.toast(TYPE.ERROR, true, this.errorMsg);
          }
          return false;
        },
      });
      return false;
    } else {
      return false;
    }
  }

  fetchContractdata(contractID: any) {
    this.addAddendumContractsService.fetchContractData(contractID).subscribe({
      next: (response) => {
        this.addaddendumForm.patchValue({
          // contractId: String(response.contractId),
          contractName: String(response.contractName),
          departmentId: String(response.departmentId),
          contractWithCompanyId: String(response.contractWithCompanyId),
          contractTypeId: String(response.contractTypeId),
          apostilleTypeId: String(response.apostilleTypeId),
          actualDocRefNo: String(response.actualDocRefNo),
          retainerContract: String(response.retainerContract),
          termsAndConditions: response.termsAndConditions,
          validFrom: this.formatDate(String(response.validFrom)),
          validTill: this.formatDate(String(response.validTill)),
          addendumDate: this.formatDate(String(response.addendumDate)),
          empCustodianId: String(response.empCustodianId),
          location: String(response.location),
        });
        this.editEmpCustodianId.nativeElement.value = response.empCustodianId;
        this.editEmpCustodianName.nativeElement.value = response.empCustodianId;
        return true;
      },
      error: (err) => {
        console.error('No Contract with this id exist', err);
        if (err.status == 401) {
          let errmsg = err.error;
          Alert.toast(TYPE.ERROR, true, errmsg);
        } else {
          this.errorMsg = JSON.stringify(
            err.message !== undefined ? err.error.message : err.message
          );
          Alert.toast(TYPE.ERROR, true, this.errorMsg);
        }
        return false;
      },
    });
    return false;
  }

  private formatDate(date: string) {
    const d = new Date(date);
    let month = '' + (d.getMonth() + 1);
    let day = '' + d.getDate();
    const year = d.getFullYear();
    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;
    return [year, month, day].join('-');
  }

  onUpdateFormSubmit(contID: number) {
    // this.masterContractAddForm.get('empCustodianId')?.setValue(this.editEmpCustodianId.nativeElement.value)
    // if (this.masterContractAddForm.invalid) {
    //   this.masterContractAddForm.markAllAsTouched();
    //   return;
    // }
    // else {
    //   const departmentId = this.masterContractAddForm.value.departmentId;
    //   const contractWithCompanyId = this.masterContractAddForm.value.contractWithCompanyId;
    //   const contractTypeId = this.masterContractAddForm.value.contractTypeId;
    //   const apostilleTypeId = this.masterContractAddForm.value.apostilleTypeId;
    //   const actualDocRefNo = this.masterContractAddForm.value.actualDocRefNo;
    //   const retainerContract = this.masterContractAddForm.value.retainerContract;
    //   const empCustodianId = this.masterContractAddForm.value.empCustodianId;
    //   const approver1Status = this.masterContractAddForm.value.approver1Status;
    //   const approver2Status = this.masterContractAddForm.value.approver2Status;
    //   const approver3Status = this.masterContractAddForm.value.approver3Status;
    //   if (departmentId && Number(departmentId) &&
    //     contractWithCompanyId && Number(contractWithCompanyId) &&
    //     contractTypeId && Number(contractTypeId) &&
    //     apostilleTypeId && Number(apostilleTypeId) &&
    //     actualDocRefNo && Number(actualDocRefNo) &&
    //     retainerContract && Number(retainerContract) &&
    //     empCustodianId && Number(empCustodianId) &&
    //     approver1Status && Number(approver1Status) &&
    //     approver2Status && Number(approver2Status) &&
    //     approver3Status && Number(approver3Status)
    //   ) {
    //     const addFormValues: AddContractDto = new AddContractDto();
    //     addFormValues.contractName = this.masterContractAddForm.value.contractName;
    //     addFormValues.departmentId = Number(departmentId);
    //     addFormValues.contractWithCompanyId = Number(contractWithCompanyId);
    //     addFormValues.contractTypeId = Number(contractTypeId);
    //     addFormValues.apostilleTypeId = Number(apostilleTypeId);
    //     addFormValues.actualDocRefNo = Number(actualDocRefNo);
    //     addFormValues.retainerContract = Number(retainerContract);
    //     addFormValues.termsAndConditions = this.masterContractAddForm.value.termsAndConditions;
    //     addFormValues.validFrom = this.masterContractAddForm.value.validFrom;
    //     addFormValues.validTill = this.masterContractAddForm.value.validTill;
    //     addFormValues.renewalFrom = this.masterContractAddForm.value.renewalFrom;
    //     addFormValues.renewalTill = this.masterContractAddForm.value.renewalTill;
    //     addFormValues.addendumDate = this.masterContractAddForm.value.renewalTill;
    //     addFormValues.empCustodianId = Number(empCustodianId);
    //     addFormValues.location = this.masterContractAddForm.value.location;
    //     addFormValues.approver1Status = Number(approver1Status);
    //     addFormValues.approver2Status = Number(approver2Status);
    //     addFormValues.approver3Status = Number(approver3Status);
    //     console.log(addFormValues);
    //     this.contractsService.editContract(contractID, addFormValues).subscribe({
    //       next: (response: boolean) => {
    //         if (response !== false) {
    //           Alert.toast(TYPE.SUCCESS, true, 'Updated successfully');
    //           // this.router.navigate(['contracts/allContracts'])
    //           this.GetAllContracts(1, 10);
    //           //this.renderer.removeClass(this.addContractModal.nativeElement, 'show');
    //           this.masterContractAddForm.reset();
    //         }
    //       },
    //       error: (error) => {
    //         console.error('Error :(', error);
    //         this.errorMsg = JSON.stringify((error.message !== undefined) ? error.error.title : error.message);
    //         Alert.toast(TYPE.ERROR, true, this.errorMsg);
    //       }
    //     });
    //   }
    //   else {
    //     console.log("should not come here ", this.masterContractAddForm.value)
    //   }
    // }
  }

  onAddAddendumFormSubmit(contractID: number) {
    this.loading = true;
    const addendum = new AddAddendumContract();
    let empCode = DecodeToken.ECode;
    // var todaysDate = new Date().toISOString().split('T')[0];
    console.log(contractID);
    addendum.contractId = contractID;
    addendum.contractName = String(this.addaddendumForm.value.contractName);
    addendum.departmentId = Number(this.addaddendumForm.value.departmentId);
    addendum.contractWithCompanyId = Number(
      this.addaddendumForm.value.contractWithCompanyId
    );
    addendum.contractTypeId = Number(this.addaddendumForm.value.contractTypeId);
    addendum.apostilleTypeId = Number(
      this.addaddendumForm.value.apostilleTypeId
    );
    addendum.actualDocRefNo = Number(this.addaddendumForm.value.actualDocRefNo);
    addendum.retainerContract = Number(
      this.addaddendumForm.value.retainerContract
    );
    addendum.termsAndConditions = String(
      this.addaddendumForm.value.termsAndConditions
    );
    addendum.validFrom = String(this.addaddendumForm.value.validFrom);
    addendum.validTill = String(this.addaddendumForm.value.validTill);
    addendum.addendumDate = new Date();
    addendum.empCustodianId = Number(this.addaddendumForm.value.empCustodianId);
    addendum.location = String(this.addaddendumForm.value.location);
    console.log('Date', addendum.addendumDate);
    // console.log('Date', todaysDate);

    this.addAddendumContractsService
      .AddAddendum(addendum.contractId, addendum, empCode)
      .subscribe({
        next: () => {
          Alert.toast(
            TYPE.SUCCESS,
            true,
            'Approve Request to add addendum is sent to Approver 1'
          );
          const searchTerm = this.filterForm.get('SearchTerm')?.value;
          const fromDate = this.filterForm.get('FromDate')?.value;
          const toDate = this.filterForm.get('ToDate')?.value;
          const contractType = this.filterForm.get('ContractType')?.value;
          const renewalDueIn = this.filterForm.get('RenewalDueIn')?.value;
          const contractStatus = this.filterForm.get('ContractStatus')?.value;
          const department = this.filterForm.get('Department')?.value;
          const location = this.filterForm.get('Location')?.value;
          const hasAddendum = this.filterForm.get('HasAddendum')?.value;
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
            HasAddendum: hasAddendum == -1 ? null : hasAddendum,
          });
          this.masterContractAddForm.reset();
          this.loading = false;
        },
        error: (err) => {
          this.loading = false;
          console.error('Error adding addendum:', err);
          if (err.status == 401) {
            let errmsg = err.error;
            Alert.toast(TYPE.ERROR, true, errmsg);
          } else {
            this.errorMsg = JSON.stringify(
              err.message !== undefined ? err.error.title : err.message
            );
            Alert.toast(TYPE.ERROR, true, this.errorMsg);
          }
        },
      });
  }

  checkContractId = new FormGroup({
    contractId: new FormControl('', [Validators.required]),
  });

  onSubmitCheck() {
    const enteredValue = this.checkContractId.value.contractId;
    this.contractsService.getContracts({}).subscribe({
      next: (res: ContractsEntity[]) => {
        this.dataSource.data = res;
        console.log(this.dataSource.data);
        this.allContracts = res;
        console.log(this.allContracts);
        if (this.checkContractId.valid) {
          const foundContract = this.allContracts.find(
            (contract) =>
              contract.contractID.toString() === enteredValue ||
              contract.contractName === enteredValue
          );

          if (foundContract) {
            console.log('Contract Found: ', foundContract);
          } else {
            console.error('Contract not found');
          }
        } else {
          console.error('Form is invalid');
        }
      },
    });
  }

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

  getContractIdforPostTerm(contractId?: string) {
    this.contIdForPostTerm = Number(contractId);
    console.log(this.contIdForPostTerm, contractId);
  }
  async approveRejectContract(id?: string, status?: number) {
    this.loading = true;
    console.log('came here 1');
    console.log('id', id, status);

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
          const hasAddendum = this.filterForm.get('HasAddendum')?.value;
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
            Location: location == 0 ? null : location,
            HasAddendum: hasAddendum == -1 ? null : hasAddendum,
          });
        }
      } catch (error) {
        this.errorMsg = JSON.stringify(error);
        Alert.toast(TYPE.ERROR, true, this.errorMsg);
      } finally {
        this.loading = false;
      }
    } else {
      this.router.navigate(['/']);
    }
    this.loading = false;
  }
  //uploading the Post Termination Notice
  OnSavePostTermination(documentForm: NgForm) {
    console.log(documentForm.value);
    console.log(this.file);

    if (!this.file || !documentForm.valid) {
      this.addFile.nativeElement.value = '';
      this.postTerm.file = null;
      this.postTerm.notice_Duration = 0;
      this.postTerm.end_Date = new Date();
      this.postTerm.Remark = '';
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
      this.postTerm.file = null;
      this.postTerm.notice_Duration = 1;
      this.postTerm.end_Date = new Date();
      this.postTerm.Remark = '';
      Alert.toast(
        TYPE.WARNING,
        true,
        'Unsupported file format. Allowed formats: .pdf, .doc, .docx '
      );
      return;
    }
    if (this.file.size > 25 * 1048576) {
      this.addFile.nativeElement.value = '';
      this.postTerm.file = null;
      Alert.toast(TYPE.WARNING, true, 'File too large. Max 25MB allowed.');
      return;
    }

    if (!this.datecheck(this.postTerm.end_Date.toString())) {
      return;
    }

    this.checkPosterminationTextarea(this.postTerm.Remark);
    if (this.remarkTouched) {
      return;
    }

    const formData = new FormData();
    formData.append('file', this.file);
    formData.append('contractId', String(this.contIdForPostTerm));
    formData.append('notice_Duration', String(this.postTerm.notice_Duration));
    formData.append('end_Date', String(this.postTerm.end_Date));
    formData.append('Remark', String(this.postTerm.Remark));
    this.loading = true;
    this.postTermService.UploadDoc(formData).subscribe({
      next: (res) => {
        this.file = null;
        documentForm.reset();
        // this.addFile.nativeElement.value = "";
        //      this.postTerm.file=null;
        this.loading = false;
        Alert.toast(
          TYPE.SUCCESS,
          true,

          ' Notice added successfully'
        );
        this.GetPage(this.maxPage);
        const modalElement = document.getElementById(
          'Termination-Notice-Detail'
        );
        if (modalElement) {
          const modalInstance =
            bootstrap.Modal.getInstance(modalElement) ||
            new bootstrap.Modal(modalElement);
          modalInstance.hide();
        }
      },
      error: (error) => {
        this.loading = false;
        console.error('Error in creating Notice:', error);
        if (error.status == 401) {
          let errmsg = error.error;
          Alert.toast(TYPE.ERROR, true, errmsg);
        } else {
          Alert.bigToast(
            'Error!',
            'There was an error posting termination notice. ' +
              error.error.message,
            TYPE.ERROR,
            'Try Again'
          );
        }
        // this.file = null;
        // documentForm.reset();
        // this.addFile.nativeElement.value = "";
        // this.postTerm.file = null;
        // this.document.status = 1;
      },
    });
    // this.file = null;
    // documentForm.reset();
    // this.addFile.nativeElement.value = "";
    // this.document.file = null;
    // this.document.status = 1;
  }
  //for date validation post termination
  datecheck(event: string) {
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    return !(event && new Date(event) < today);
  }

  remarkTouchedFalse() {
    this.remarkTouched = false;
  }

  checkPosterminationTextarea(value: string) {
    if (value.length == 0) {
      this.remarkTouched = true;
    } else {
      this.remarkTouched = false;
    }
  }

  // Post Termination Notice
  postTermination: PostTermination = new PostTermination();
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
      console.log('came here 2');
      let email = DecodeToken.email;
      if (email) {
        try {
          this.postTermination.contractId = Number(contractId);
          this.postTermination.changeToStatus = this.statusTermOrReject;
          this.postTermination.emailSubject = emailSubject;
          this.postTermination.emailBody = emailBody;
          this.postTermination.employeeEmail = email;
          console.log(this.postTermination);

          const response = await firstValueFrom(
            this.postTermService.ApproveTerminationContract(
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
            const hasAddendum = this.filterForm.get('HasAddendum')?.value;
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
              Location: location == 0 ? null : location,
              HasAddendum: hasAddendum == -1 ? null : hasAddendum,
            });
            // const modalElement = document.getElementById('postTerm-mail');
            // if (modalElement) {
            //   const modalInstance = bootstrap.Modal.getInstance(modalElement) || new bootstrap.Modal(modalElement);
            //   modalInstance.hide();
            // }
          }
        } catch (error) {
          this.errorMsg = JSON.stringify(error);
          Alert.toast(TYPE.ERROR, true, this.errorMsg);
          console.error(error);
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
    this.noticeWithdrawalService.AddWithdrawalNotice(formData).subscribe({
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
        const modalElement = document.getElementById(
          'Notice-Withdrawal-Detail'
        );
        if (modalElement) {
          const modalInstance =
            bootstrap.Modal.getInstance(modalElement) ||
            new bootstrap.Modal(modalElement);
          modalInstance.hide();
        }
      },
      error: (error) => {
        console.error('Error in adding notice withdrawal:', error);
        if (error.status == 401) {
          let errmsg = error.error;
          Alert.toast(TYPE.ERROR, true, errmsg);
        } else {
          Alert.bigToast(
            'Error!',
            'There was an error adding notice withdrawal.',
            TYPE.ERROR,
            'Try Again'
          );
        }
        // this.file = null;
        // documentForm.reset();
        // this.addFile.nativeElement.value = "";
        // this.postTerm.file = null;
        // this.document.status = 1;
      },
    });
    // this.file = null;
    // documentForm.reset();
    // this.addFile.nativeElement.value = "";
    // this.document.file = null;
    // this.document.status = 1;
  }

  withdrawalNoticeEmailForm = new FormGroup({
    emailSubject: new FormControl('', [Validators.required]),
    emailBody: new FormControl('', [Validators.required]),
  });

  withdrawNoticeSend: ApproveRejectWithdrawalDTO =
    new ApproveRejectWithdrawalDTO();
  async approveWithdrawalNotice(contractId?: string) {
    if (this.withdrawalNoticeEmailForm.invalid) {
      this.withdrawalNoticeEmailForm.markAllAsTouched();
      return;
    } else {
      this.loading = true;
      const emailSubject = this.withdrawalNoticeEmailForm.value.emailSubject;
      const emailBody = this.withdrawalNoticeEmailForm.value.emailBody;
      console.log('came here 3');
      let email = DecodeToken.email;
      if (email) {
        try {
          this.withdrawNoticeSend.contractId = Number(contractId);
          this.withdrawNoticeSend.changeToStatus = this.statusTermOrReject;
          this.withdrawNoticeSend.emailSubject = emailSubject;
          this.withdrawNoticeSend.emailBody = emailBody;
          this.withdrawNoticeSend.employeeEmail = email;
          console.log(this.postTermination);

          const response = await firstValueFrom(
            this.noticeWithdrawalService.ApproveWithdrawalTermination(
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
            const hasAddendum = this.filterForm.get('HasAddendum')?.value;
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
              Location: location == 0 ? null : location,
              HasAddendum: hasAddendum == -1 ? null : hasAddendum,
            });
            // const modalElement = document.getElementById('withdrawal-mail');
            // if (modalElement) {
            //   const modalInstance = bootstrap.Modal.getInstance(modalElement) || new bootstrap.Modal(modalElement);
            //   modalInstance.hide();
            // }
          }
        } catch (error) {
          this.errorMsg = JSON.stringify(error);
          Alert.toast(TYPE.ERROR, true, this.errorMsg);
          console.error(error);
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
    // PDFExport.printToPDF(tableID, fileName);
    PDFExport.printToPDF(
      'contracts-table',
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
      'contracts-table',
      'CMS-Contracts.xlsx',
      selectedColumns
    );
  }

  getProgressType(status: number | undefined): string {
    if (status === undefined || status === null) {
      return '';
    }

    switch (status) {
      case 1:
        return 'Approval for';
      case 2:
        return 'Active'; // optional � maybe don't show this
      case 3:
        return 'Rejection for';
      case 4:
        return 'Termination of';
      case 5:
        return 'Expiration of';
      case 6:
        return 'Termination in progress for';
      case 7:
        return 'Termination approved for';
      case 8:
        return 'Notice withdrawal pending for';
      default:
        return 'Progress for ';
    }
  }
  phases = [
    'Contract Created',
    'L1 Approver Approval',
    'L2 Approver Approval',
    'L3 Approver Approval',
    'Contract Active',
  ];
}

//for required validations
//  get EndDate(){
//       return this.masterCompanyAddForm.get('pocContactNumber');
//     }
