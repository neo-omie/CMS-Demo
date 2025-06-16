declare var bootstrap: any;
import { CommonModule } from '@angular/common';
import {
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnInit,
  Output,
  Renderer2,
  ViewChild,
} from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { GetAllDepartmentsDto } from '../../../models/master-department';
import { ClassifiedContractsService } from '../../../services/classified-contracts.service';
import { MasterApostilleService } from '../../../services/master-apostille.service';
import { TYPE } from '../../auth/login/values.constants';
import { Alert } from '../../../utils/alert';
import { ContractTypeMasterDTO } from '../../../models/contract-type-master';
import {
  MasterApostille,
  MasterApostilleDto,
} from '../../../models/master-apostille';
import { CompanyMasterDto } from '../../../models/master-company';
import { MasterEmployee } from '../../../models/master-employee';
import {
  AddClassifiedContractDto,
  GetClassifiedContractByIdDto,
} from '../../../models/classified-contracts';
import { firstValueFrom } from 'rxjs';
import { DecodeToken } from '../../../utils/decodeToken';
import {
  dateBetweenValidator,
  dateRangeValidator,
  dateValidator,
} from '../../../utils/dateValidator';
import { ProgressBarComponent } from '../../UtilComponents/progress-bar/progress-bar.component';
import { Location } from '../../../utils/constants';
import { ErrorHandler } from '../../../utils/errorHandler';

@Component({
  selector: 'app-all-classified-contract-view-add-modal',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, ProgressBarComponent],
  templateUrl: './all-classified-contract-view-add-modal.component.html',
  styleUrl: './all-classified-contract-view-add-modal.component.css',
})
export class AllClassifiedContractViewAddModalComponent implements OnInit {
  @Output() loaderEmit = new EventEmitter<boolean>();
  @Input() approverCheck: boolean = false;
  @Input() terminationCheck: boolean = false;
  @Input() withdrawCheck: boolean = false;
  @Input() contractDetails?: GetClassifiedContractByIdDto;
  @Input() isCreate = false;
  @Input() termStatus!: (status: number) => void;
  @Input() getContractIdforPostTerm!: (classifiedContractID?: string) => void;
  @Input() approveRejectContract!: (id?: string, status?: number) => void;
  @Input() GetAllContracts!: (filter: any) => void;

  approverStatusColor: string[] = [
    '',
    '#ffc107',
    '#adff2f',
    '#ff4500',
    '',
    '#a9a9a9',
    '#ffc107',
    '#f08205',
    '#6a82c5',
  ];
  approverStatusValue: string[] = [
    '',
    'Pending Approval',
    'Active',
    'Rejected',
    'Terminated',
    'Expired',
    'Pending Termination',
    'Approved for Termination',
    'Pending Notice  Withdrawal',
  ];
  departments: GetAllDepartmentsDto[] = [];
  contractTypes: ContractTypeMasterDTO[] = [];
  apostilleTypes: MasterApostille[] = [];
  companies: CompanyMasterDto[] = [];
  employeeCustodians: MasterEmployee[] = [];

  @ViewChild('addEmpCustodianCollapse') addEmpCustodianCollapse!: ElementRef;

  dummyForm: FormGroup = new FormGroup({});
  masterContractAddForm: FormGroup = new FormGroup(
    {
      classifiedContractName: new FormControl('', [
        Validators.required,
        Validators.maxLength(20),
      ]),
      departmentId: new FormControl('', [Validators.required]),
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
      renewalFrom: new FormControl('', dateValidator()),
      renewalTill: new FormControl('', dateValidator()),
      // addendumDate: new FormControl(''),
      empCustodianName: new FormControl('', [Validators.required]),
      empCustodianId: new FormControl('', [Validators.required]),
      // location: new FormControl('', [Validators.required,Validators.pattern('^[A-Za-z]+$')]),
      location: new FormControl(''),
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
  Location = Location; // Makes the enum accessible in template
  locationOptions: string[];

  constructor(
    private contractsService: ClassifiedContractsService,
    private renderer: Renderer2,
    private masterApostilleService: MasterApostilleService
  ) {
    this.locationOptions = Object.values(Location);
  }

  ngOnInit(): void {
    this.initialRequirementLoad();
  }

  formfield(name: string) {
    if (name === 'reset') {
      this.masterContractAddForm?.reset({
        skipApproval: true,
        departmentId: '',
        contractWithCompanyId: '',
        contractTypeId: '',
        apostilleTypeId: '',
        retainerContract: '',
        renewalFrom: '',
        renewalTill: '',
        // addendumDate: '',
        empCustodianName: '',
        location:''
      });
      return;
    } else {
      return this.masterContractAddForm?.get(name);
    }
  }

  returnValue(value?: number): number {
    if (value) {
      return value;
    }
    return 0;
  }

  error(error: any) {
    console.error('Error :(', error);
            ErrorHandler.handle(error);

  }

  initialRequirementLoad() {
    const eCode = DecodeToken.ECode;
    if(eCode){
      this.contractsService.GetDepartments(eCode).subscribe({
        next: (response: GetAllDepartmentsDto[]) => {
          this.departments = response;
        },
        error: (error) => this.error(error),
      });
  
      this.contractsService.GetContractTypes().subscribe({
        next: (response: ContractTypeMasterDTO[]) => {
          this.contractTypes = response;
        },
        error: (error) => this.error(error),
      });
  
      this.masterApostilleService.getApostilles(1, 100).subscribe({
        next: (response: MasterApostilleDto) => {
          this.apostilleTypes = response.data;
        },
        error: (error) => this.error(error),
      });
  
      this.contractsService.GetCompanies().subscribe({
        next: (response: CompanyMasterDto[]) => {
          this.companies = response;
        },
        error: (error) => this.error(error),
      });
    }
  }

  getProgressType(status: number | undefined): string {
    if (status === undefined || status === null) {
      return '';
    }

    switch (status) {
      case 1:
        return 'Approval for';
      case 2:
        return 'Active';
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
    'Contract Initial State',
    'L1 Approver Approval',
    'L2 Approver Approval',
    'L3 Approver Approval',
    'Contract Final State',
  ];

  textChangeEmployeeCustodian(departmentId: number, event: Event) {
    let input = event.target as HTMLInputElement;
    this.contractsService
      .GetEmployeeForInputText(departmentId, input.value)
      .subscribe({
        next: (response: MasterEmployee[]) => {
          this.employeeCustodians = response;
        },
        error: (error) => {
                  ErrorHandler.handle(error);

        }
      });
  }

  fillEmployeeCustodian(employeeId: number, employeeName: string) {
    const input =
      this.addEmpCustodianCollapse.nativeElement.querySelector('input');
    input.value = '';
    this.employeeCustodians.length = 0;
    this.renderer.removeClass(
      this.addEmpCustodianCollapse.nativeElement,
      'show'
    );
    this.formfield('empCustodianId')?.setValue(employeeId);
    this.formfield('empCustodianName')?.setValue(employeeName);
  }

  async onAddFormSubmit() {
    let empName = DecodeToken.ECode;
    this.formfield('approver1Status')?.setValue(1);
    this.formfield('approver2Status')?.setValue(1);
    this.formfield('approver3Status')?.setValue(1);
    if (this.masterContractAddForm.invalid) {
      this.masterContractAddForm.markAllAsTouched();
      console.log(
        'MasterContractAddForm is invalid : ',
        this.masterContractAddForm.value
      );
      return;
    } else {
      this.loaderEmit.emit(true);
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
        Number(departmentId) &&
        Number(contractWithCompanyId) &&
        Number(contractTypeId) &&
        Number(apostilleTypeId) &&
        Number(actualDocRefNo) &&
        Number(retainerContract) &&
        Number(empCustodianId) &&
        Number(approver1Status) &&
        Number(approver2Status) &&
        Number(approver3Status)
      ) {
        const addFormValues: AddClassifiedContractDto =
          new AddClassifiedContractDto();
        addFormValues.classifiedContractName =
          this.masterContractAddForm.value.classifiedContractName;
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
          this.masterContractAddForm.value.renewalFrom != ''
            ? String(this.masterContractAddForm.value.renewalFrom)
            : null;
        addFormValues.renewalTill =
          this.masterContractAddForm.value.renewalTill != ''
            ? String(this.masterContractAddForm.value.renewalTill)
            : null;
        addFormValues.skipApproval =
          this.masterContractAddForm.value.skipApproval;
        addFormValues.empCustodianId = Number(empCustodianId);
        addFormValues.location = this.masterContractAddForm.value.location;
        addFormValues.approver1Status = Number(approver1Status);
        addFormValues.approver2Status = Number(approver2Status);
        addFormValues.approver3Status = Number(approver3Status);

        try {
          const response = await firstValueFrom(
            this.contractsService.addContract(addFormValues, empName)
          );

          if (response !== false) {
            Alert.toast(TYPE.SUCCESS, true, 'Added successfully');
            this.formfield('reset');
            this.GetAllContracts({
              PageNumber: 1,
              PageSize: 10,
            });
            const modalElement = document.getElementById('contract-add');
            if (modalElement) {
              const modalInstance =
                bootstrap.Modal.getInstance(modalElement) ||
                new bootstrap.Modal(modalElement);
              modalInstance.hide();
            }
          }
        } catch (error) {
          
          this.loaderEmit.emit(false);
                 ErrorHandler.handle(error);

        } finally {
          this.loaderEmit.emit(false);
        }
      } else {
        Alert.toast(TYPE.ERROR, true, 'Got a non number value in number field');
        this.loaderEmit.emit(false);
      }
    }
  }
}
