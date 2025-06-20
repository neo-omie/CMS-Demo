declare var bootstrap: any;

import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ProgressBarComponent } from "../../UtilComponents/progress-bar/progress-bar.component";
import { CommonModule } from '@angular/common';
import { GetContractByIdDto } from '../../../models/contracts';
import { DecodeToken } from '../../../utils/decodeToken';
import { ContractsService } from '../../../services/contracts.service';
import { ErrorHandler } from '../../../utils/errorHandler';
import { Router } from '@angular/router';
import { Alert } from '../../../utils/alert';
import { TYPE } from '../../auth/login/values.constants';

@Component({
  selector: 'app-contract-view-modal',
  standalone: true,
  imports: [ProgressBarComponent, CommonModule],
  templateUrl: './contract-view-modal.component.html',
  styleUrl: './contract-view-modal.component.css'
})
export class ContractViewModalComponent {
  @Output() loaderEmit = new EventEmitter<boolean>();
  @Output() mailTypeEmit = new EventEmitter<string>();
  @Output() statusTermOrRejectEmit = new EventEmitter<number>();
  @Input()  GetPage!: (pgNumber:number) => void;
  @Input() currentPage?: number;
  @Input() contractDetails?: GetContractByIdDto;
  @Input() approverCheck: boolean = false;
  @Input() terminationCheck: boolean = false;
  @Input() withdrawCheck: boolean = false;
  contIdForPostTerm: number = 0;
  phases = [
    'Contract Initial State',
    'L1 Approver Approval',
    'L2 Approver Approval',
    'L3 Approver Approval',
    'Contract Final State',
  ];

  constructor(
    private router: Router,
    private contractsService: ContractsService
  ) { }

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

  getContractIdforPostTerm(contractId?: string) {
    this.contIdForPostTerm = Number(contractId);
  }

  termStatus(status: number, mailType: string) {
    if (status == 7) {
      this.statusTermOrRejectEmit.emit(7);
      this.mailTypeEmit.emit(mailType);
    }
    if (status == 2) {
      this.statusTermOrRejectEmit.emit(2);
      this.mailTypeEmit.emit(mailType);
    }
  }

  approveRejectContract(id?: string, status?: number) {
    this.loaderEmit.emit(true)
    
    let email = DecodeToken.email;
    if (email) {
      this.contractsService.approveRejectContract(Number(id), email, status).subscribe({
        next: (res) => {
          this.loaderEmit.emit(false)
          Alert.toast(TYPE.SUCCESS,true,status==2? 'Approved Successfully':'Rejected Successfully' )
          const modalElement = document.getElementById('contract-detail');
            if (modalElement) {
              const modalInstance =
                bootstrap.Modal.getInstance(modalElement) ||
                new bootstrap.Modal(modalElement);
              modalInstance.hide();
            }
          if (res === true && this.currentPage != undefined) {
            this.GetPage(this.currentPage);
          }
        },
        error: (err) => {
          ErrorHandler.handle(err)
          this.loaderEmit.emit(false)
        }
      })
    }
    else {
      this.loaderEmit.emit(false)
      this.router.navigate(['/']);
    }
  }
}
