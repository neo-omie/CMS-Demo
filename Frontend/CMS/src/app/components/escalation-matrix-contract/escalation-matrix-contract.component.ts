import { Component, OnInit } from '@angular/core';
import { MasterEscalationMatrixContractDto } from '../../models/escalation-matrix-contract';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { LoaderComponent } from '../loader/loader.component';
import { Router, RouterModule } from '@angular/router';
import { EscalationMatrixContractService } from '../../services/escalation-matrix-contract.service';

@Component({
  selector: 'app-escalation-matrix-contract',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterModule, LoaderComponent],
  templateUrl: './escalation-matrix-contract.component.html',
  styleUrl: './escalation-matrix-contract.component.css',
})
export class EscalationMatrixContractComponent implements OnInit {
  loading: boolean = true;
  maxPage = 1;
  pageNumbers = [1, 1, 2, 3, 4, 5];
  matrixContracts?: MasterEscalationMatrixContractDto;

  ngOnInit(): void {
    this.getMatrixContracts(1, 10);
  }
  constructor(
    private escalationService: EscalationMatrixContractService,
    private router: Router
  ) {}

  getMatrixContracts(pageNumber: number, pageSize: number) {
    this.escalationService
      .getAllMatrixContract(pageNumber, pageSize)
      .subscribe((res) => {
        this.loading = false;
        console.log(res);
        
        this.matrixContracts = res;
        this.pageNumbers[0] = pageNumber;
        if (this.matrixContracts.matrixContract.length > 0) {
          if (pageNumber == 1) {
            this.maxPage = Math.ceil(this.matrixContracts.totalCount / 10);
          }
          let diff = this.maxPage - pageNumber;
          if (diff >= 0 && this.maxPage >= 5) {
            if (this.pageNumbers[0] == 1) {
              this.pageNumbers[1] = pageNumber;
              this.pageNumbers[2] = pageNumber + 1;
              this.pageNumbers[3] = pageNumber + 2;
              this.pageNumbers[4] = pageNumber + 3;
              this.pageNumbers[5] = pageNumber + 4;
            } else if (this.pageNumbers[0] == 2) {
              this.pageNumbers[1] = pageNumber - 1;
              this.pageNumbers[2] = pageNumber;
              this.pageNumbers[3] = pageNumber + 1;
              this.pageNumbers[4] = pageNumber + 2;
              this.pageNumbers[5] = pageNumber + 3;
            } else if (diff == 0) {
              this.pageNumbers[1] = pageNumber - 4;
              this.pageNumbers[2] = pageNumber - 3;
              this.pageNumbers[3] = pageNumber - 2;
              this.pageNumbers[4] = pageNumber - 1;
              this.pageNumbers[5] = pageNumber;
            } else if (diff == 1) {
              this.pageNumbers[1] = pageNumber - 3;
              this.pageNumbers[2] = pageNumber - 2;
              this.pageNumbers[3] = pageNumber - 1;
              this.pageNumbers[4] = pageNumber;
              this.pageNumbers[5] = pageNumber + 1;
            } else {
              this.pageNumbers[1] = pageNumber - 2;
              this.pageNumbers[2] = pageNumber - 1;
              this.pageNumbers[3] = pageNumber;
              this.pageNumbers[4] = pageNumber + 1;
              this.pageNumbers[5] = pageNumber + 2;
            }
          }
        }
      });
  }

  GetPage(pgNumber:number){
    if(this.maxPage >= pgNumber && pgNumber >= 1){
      this.getMatrixContracts(pgNumber, 10);
    }
  }
  GetMatrixContractById(valueId:number){
    this.escalationService.getMatrixContractById(valueId).subscribe((res)=>{
      this.loading = false;
      this.matrixContracts = res;
    })
  }
}
