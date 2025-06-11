import { Component, EventEmitter, Input, Output } from '@angular/core';
import { DecodeToken } from '../../../utils/decodeToken';
import { Router } from '@angular/router';
import { AddAddendumContractsService } from '../../../services/add-addendum-contracts.service';
import { TYPE } from '../../auth/login/values.constants';
import { CommonModule } from '@angular/common';
import { Alert } from '../../../utils/alert';
import { ErrorHandler } from '../../../utils/errorHandler';
import { AddAddendumContract } from '../../../models/add-addendum-contract';
import { ProgressBarComponent } from "../../UtilComponents/progress-bar/progress-bar.component";

@Component({
  selector: 'app-addendum-contract-modal',
  standalone: true,
  imports: [CommonModule, ProgressBarComponent],
  templateUrl: './addendum-contract-modal.component.html',
  styleUrl: './addendum-contract-modal.component.css'
})
export class AddendumContractModalComponent {
  @Output() loaderEmit = new EventEmitter<boolean>();
  @Input() GetAllAddendum !: (pageNumber:number, pageSize:number) => void;
  @Input() addAddendumContract?: AddAddendumContract;
  @Input() approverCheck: boolean = false;
  constructor(
    private addAddendumContractsService: AddAddendumContractsService,
    private router: Router
  ) { }
  async approveRejectContract(
    contractId?: number,
    id?: number,
    status?: number
  ) {
    this.loaderEmit.emit(true);
    console.log('came here');
    let email = DecodeToken.email;
    if (email) {
      this.addAddendumContractsService.approveRejectContract(
        contractId,
        id,
        email,
        status
      ).subscribe({
        next: (response: boolean) => {
          if (response) {
            Alert.toast(TYPE.SUCCESS, true, 'Updated successfully');
            this.GetAllAddendum(1, 10);
          }
          this.loaderEmit.emit(false);
        },
        error: (error) => {
          this.loaderEmit.emit(false);
          ErrorHandler.handle(error);
        }
      })
    }
    else {
      this.loaderEmit.emit(false);
      this.router.navigate(['/']);
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
    'Addendum Initial State',
    'L1 Approver Approval',
    'L2 Approver Approval',
    'L3 Approver Approval',
    'Addendum Final State',
  ];
}
