declare var bootstrap: any;
import { CommonModule } from '@angular/common';
import { Component, ElementRef, EventEmitter, Input, OnChanges, Output, Renderer2, SimpleChanges, ViewChild } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Alert } from '../../../utils/alert';
import { TYPE } from '../../auth/login/values.constants';
import { ErrorHandler } from '../../../utils/errorHandler';
import { DecodeToken } from '../../../utils/decodeToken';
import { AddAddendumContract } from '../../../models/add-addendum-contract';
import { AddAddendumContractsService } from '../../../services/add-addendum-contracts.service';
import { GetContractByIdDto } from '../../../models/contracts';
import { GetAllDepartmentsDto } from '../../../models/master-department';
import { ContractTypeMasterDTO } from '../../../models/contract-type-master';
import { MasterApostille } from '../../../models/master-apostille';
import { CompanyMasterDto } from '../../../models/master-company';
import { MasterEmployee } from '../../../models/master-employee';
import { ContractsService } from '../../../services/contracts.service';

@Component({
  selector: 'app-contract-add-addendum-contract-modal',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './contract-add-addendum-contract-modal.component.html',
  styleUrl: './contract-add-addendum-contract-modal.component.css'
})
export class ContractAddAddendumContractModalComponent implements OnChanges {
  @Output() loaderEmit = new EventEmitter<boolean>();
  @Input() GetPage !: (pgNumber: number) => void;
  @Input() currentPage?: number
  @Input() departments?: GetAllDepartmentsDto[];
  @Input() contractTypes?: ContractTypeMasterDTO[];
  @Input() contractDetails?: GetContractByIdDto;
  @Input() apostilleTypes: MasterApostille[] = [];
  @Input() companies: CompanyMasterDto[] = [];
  employeeCustodians: MasterEmployee[] = [];
  validFrom?: string;
  validTill?: string;

  @ViewChild('addAddendumEmpCustodianCollapse') addAddendumEmpCustodianCollapse!: ElementRef;

  constructor(
    private renderer: Renderer2,
    private contractsService: ContractsService,
    private addAddendumContractsService: AddAddendumContractsService,
  ) { }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['contractDetails']) {
      this.patchForm();
    }
  }

  patchForm = () => {
    this.addaddendumForm.patchValue({
      addendumContractId: '',
      contractId: this.contractDetails?.contractId,
      contractName: this.contractDetails?.contractName,
      departmentId: this.contractDetails?.departmentId + '',
      contractWithCompanyId: this.contractDetails?.contractWithCompanyId + '',
      contractTypeId: this.contractDetails?.contractTypeId + '',
      apostilleTypeId: this.contractDetails?.apostilleTypeId + '',
      actualDocRefNo: this.contractDetails?.actualDocRefNo + '',
      retainerContract: this.contractDetails?.retainerContract + '',
      termsAndConditions: this.contractDetails?.termsAndConditions,
      validFrom: this.contractDetails?.validFrom?.toString().split('T')[0],
      validTill: this.contractDetails?.validTill?.toString().split('T')[0],
      empCustodianName: this.contractDetails?.empCustodianName,
      empCustodianId: this.contractDetails?.empCustodianId + '',
      location: this.contractDetails?.location,
    });
    this.validFrom = this.contractDetails?.validFrom?.toString().split('T')[0],
      this.validTill = this.contractDetails?.validTill?.toString().split('T')[0],
      this.addaddendumForm.get('validFrom')?.disable();
    this.addaddendumForm.get('validTill')?.disable();
  };

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
    empCustodianName: new FormControl('', [Validators.required]),
    empCustodianId: new FormControl('', [Validators.required]),
    empCustodianNameSearch: new FormControl(''),
    location: new FormControl('', [Validators.required]),
  });

  onAddAddendumFormSubmit(contractID: number) {
    this.loaderEmit.emit(true)
    const addendum = new AddAddendumContract();
    let empCode = DecodeToken.ECode;
    if (empCode) {
      addendum.contractId = contractID;
      addendum.contractName = String(this.addaddendumForm.value.contractName);
      addendum.departmentId = Number(this.addaddendumForm.value.departmentId);
      addendum.contractWithCompanyId = Number(this.addaddendumForm.value.contractWithCompanyId);
      addendum.contractTypeId = Number(this.addaddendumForm.value.contractTypeId);
      addendum.apostilleTypeId = Number(this.addaddendumForm.value.apostilleTypeId);
      addendum.actualDocRefNo = Number(this.addaddendumForm.value.actualDocRefNo);
      addendum.retainerContract = Number(this.addaddendumForm.value.retainerContract);
      addendum.termsAndConditions = String(this.addaddendumForm.value.termsAndConditions);
      addendum.validFrom = String(this.validFrom);
      addendum.validTill = String(this.validTill);
      addendum.addendumDate = new Date();
      addendum.empCustodianId = Number(this.addaddendumForm.value.empCustodianId);
      addendum.location = String(this.addaddendumForm.value.location);

      if (
        addendum.contractName === this.contractDetails?.contractName &&
        addendum.departmentId === this.contractDetails?.departmentId &&
        addendum.contractWithCompanyId === this.contractDetails?.contractWithCompanyId &&
        addendum.contractTypeId === this.contractDetails?.contractTypeId &&
        addendum.apostilleTypeId === this.contractDetails?.apostilleTypeId &&
        addendum.actualDocRefNo === this.contractDetails?.actualDocRefNo &&
        addendum.retainerContract === this.contractDetails?.retainerContract &&
        addendum.termsAndConditions === this.contractDetails?.termsAndConditions &&
        addendum.empCustodianId === this.contractDetails?.empCustodianId
      ) {
        Alert.toast(TYPE.ERROR, true, 'Nothing changed to create addendum');
        this.loaderEmit.emit(false);
      }
      else {
        this.addAddendumContractsService
          .AddAddendum(addendum.contractId, addendum, empCode)
          .subscribe({
            next: () => {
              this.loaderEmit.emit(false);
              Alert.toast(TYPE.SUCCESS, true, 'Approve Request to add addendum is sent to Approver 1');
              const modalElement = document.getElementById('contract-addendum');
              if (modalElement) {
                const modalInstance =
                  bootstrap.Modal.getInstance(modalElement) ||
                  new bootstrap.Modal(modalElement);
                modalInstance.hide();
              }
              if (this.currentPage) {
                this.GetPage(this.currentPage);
              }
            },
            error: (err) => {
              this.loaderEmit.emit(false);
              ErrorHandler.handle(err);
            },
          });
      }
    }
    else {
      this.loaderEmit.emit(false);
      Alert.toast(TYPE.ERROR, true, 'Logout and Login again');
    }
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
    this.employeeCustodians.length = 0;
    this.renderer.removeClass(
      this.addAddendumEmpCustodianCollapse.nativeElement,
      'show'
    );
    this.renderer.removeClass(
      this.addAddendumEmpCustodianCollapse.nativeElement,
      'show'
    );
    this.addaddendumForm.get('empCustodianNameSearch')?.setValue('');
    this.addaddendumForm.get('empCustodianName')?.setValue(employeeName);
    this.addaddendumForm.get('empCustodianId')?.setValue(employeeId + '');
  }
}
