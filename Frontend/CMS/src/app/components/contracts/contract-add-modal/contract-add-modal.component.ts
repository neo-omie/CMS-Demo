declare var bootstrap: any
import { Component, ElementRef, EventEmitter, Input, Output, Renderer2, ViewChild } from '@angular/core';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { dateValidator, dateRangeValidator, dateBetweenValidator } from '../../../utils/dateValidator';
import { CommonModule } from '@angular/common';
import { ContractTypeMasterDTO } from '../../../models/contract-type-master';
import { MasterApostille } from '../../../models/master-apostille';
import { MasterEmployee } from '../../../models/master-employee';
import { CompanyMasterDto } from '../../../models/master-company';
import { GetAllDepartmentsDto } from '../../../models/master-department';
import { ContractsService } from '../../../services/contracts.service';
import { ErrorHandler } from '../../../utils/errorHandler';
import { AddContractDto } from '../../../models/contracts';
import { DecodeToken } from '../../../utils/decodeToken';
import { Alert } from '../../../utils/alert';
import { TYPE } from '../../auth/login/values.constants';
import { Location } from '../../../utils/constants';

@Component({
  selector: 'app-contract-add-modal',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './contract-add-modal.component.html',
  styleUrl: './contract-add-modal.component.css'
})
export class ContractAddModalComponent {
  @Output() loaderEmit = new EventEmitter<boolean>();
  @Input() GetAllContracts !: () => void;
  @Input() departments?: GetAllDepartmentsDto[];
  @Input() contractTypes?: ContractTypeMasterDTO[];
  @Input() apostilleTypes: MasterApostille[] = [];
  @Input() companies: CompanyMasterDto[] = [];
  employeeCustodians: MasterEmployee[] = [];
  locationOptions: string[];

  @ViewChild('addEmpCustodianCollapse') addEmpCustodianCollapse!: ElementRef;
  @ViewChild('addEmpCustodianName') addEmpCustodianName!: ElementRef;

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
      renewalFrom: new FormControl('', [dateValidator()]),
      renewalTill: new FormControl('', [dateValidator()]),
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

  constructor(
    private renderer: Renderer2,
    private contractsService: ContractsService,
  ) {
    this.locationOptions = Object.values(Location);
  }

  formField(name: string) {
    if (name == 'reset') {
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
        empCustodianId: '',
        location: '',
        approver1Status: '1',
        approver2Status: '1',
        approver3Status: '1',
      });
    }
    return this.masterContractAddForm.get(name);
  }

  textChangeEmployeeCustodian(
    departmentId: number,
    event: Event
  ) {
    let input = event.target as HTMLInputElement;
    this.contractsService
      .GetEmployeeForInputText(departmentId, input.value)
      .subscribe({
        next: (res: MasterEmployee[]) => {
          this.employeeCustodians = res;
        },
        error: (err) => { ErrorHandler.handle(err) }
      });
  }

  fillEmployeeCustodian(
    employeeId: number,
    employeeName: string
  ) {
    const input =
      this.addEmpCustodianCollapse.nativeElement.querySelector('input');
    input.value = '';
    this.employeeCustodians.length = 0;
    this.renderer.removeClass(
      this.addEmpCustodianCollapse.nativeElement,
      'show'
    );
    this.renderer.removeClass(
      this.addEmpCustodianCollapse.nativeElement,
      'show'
    );
    this.addEmpCustodianName.nativeElement.value = employeeName;
    this.masterContractAddForm.get('empCustodianId')?.setValue(employeeId + '');
  }

  onAddFormSubmit() {
    let empName = DecodeToken.ECode;
    if (this.masterContractAddForm.invalid) {
      this.masterContractAddForm.markAllAsTouched();
      return;
    }
    else {
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
        this.loaderEmit.emit(true);
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
        addFormValues.empCustodianId = Number(empCustodianId);
        addFormValues.location = this.masterContractAddForm.value.location;
        addFormValues.approver1Status = Number(approver1Status);
        addFormValues.approver2Status = Number(approver2Status);
        addFormValues.approver3Status = Number(approver3Status);
        this.contractsService.addContract(addFormValues, empName).subscribe({
          next: (res) => {
            this.loaderEmit.emit(false)
            if (res === true) {
              Alert.toast(TYPE.SUCCESS, true, 'Added successfully');
              this.formField('reset');
              this.GetAllContracts();
            }
            const modalElement = document.getElementById('contract-add');
            if (modalElement) {
              const modalInstance =
                bootstrap.Modal.getInstance(modalElement) ||
                new bootstrap.Modal(modalElement);
              modalInstance.hide();
            }
          },
          error: (err) => { ErrorHandler.handle(err) }
        })
      }
      else {
        Alert.toast(TYPE.ERROR, true, 'Number field/s is getting non-number value');
      }
    }
  }
}
