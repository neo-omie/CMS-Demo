import { CommonModule } from '@angular/common';
import { Component, OnInit, Renderer2 } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { LoaderComponent } from '../../loader/loader.component';
import { ContractsEntity, GetContractByIdDto } from '../../../models/contracts';
import { ContractsService } from '../../../services/contracts.service';
import { Pagination } from '../../../utils/pagination';
import { Alert } from '../../../utils/alert';
import { TYPE } from '../../auth/login/values.constants';

@Component({
  selector: 'app-all-contracts',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterModule, LoaderComponent],
  templateUrl: './all-contracts.component.html',
  styleUrl: './all-contracts.component.css'
})
export class AllContractsComponent implements OnInit {
  loading: boolean = true;
  maxPage = 1;
  pageNumbers = [1, 1, 2, 3, 4, 5];
  errorMsg:string = "";
  allContracts: ContractsEntity[] = [];
  contractDetails?: GetContractByIdDto;

  ngOnInit(): void {
    this.GetAllContracts(1, 10);
  }
  constructor(private contractsService: ContractsService, private router: Router,private renderer : Renderer2) {}

  GetAllContracts(pageNumber: number, pageSize: number) {
      this.contractsService.getContracts(pageNumber, pageSize).subscribe({
        next: (res: ContractsEntity[]) => {
          this.loading = false;
          this.allContracts = res;
          
          if (this.allContracts != undefined && this.allContracts.length > 0) {
            let result = Pagination.paginator(
              pageNumber,
              this.allContracts[0].totalRecords,
              pageSize
            );
            this.maxPage = result.maxPage;
            this.pageNumbers = result.pageNumbers;
          }
        },
        error: (error) => {
          this.loading = false;
          console.error('Error :(', error);
          this.errorMsg = JSON.stringify(
            error.message !== undefined ? error.error.title : error.message
          );
          Alert.toast(TYPE.ERROR, true, this.errorMsg);
        },
      });
    }
    GetPage(pgNumber: number) {
      if (this.maxPage >= pgNumber && pgNumber >= 1) {
        this.GetAllContracts(pgNumber, 10);
      }
    }
    GetContract(contractID: number) {
      this.contractsService.getContractByID(contractID).subscribe({
        next: (response: GetContractByIdDto) => {
          this.contractDetails = response;
          console.log(response);
        },
        error: (error) => {
          console.error('Error :(', error);
          if (error.message !== undefined) {
            this.errorMsg = JSON.stringify(error.error.message);
            console.log(this.errorMsg);
          }
          else {
            this.errorMsg = JSON.stringify(error.message);
            console.log(this.errorMsg);
          }
        }
      });
    }
}
