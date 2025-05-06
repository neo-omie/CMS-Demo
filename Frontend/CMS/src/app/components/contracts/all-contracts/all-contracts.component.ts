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
import { Title } from '@angular/platform-browser';

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
  constructor(private contractsService: ContractsService, private router: Router,private renderer : Renderer2, private title:Title) {
    this.title.setTitle("All Contracts - CMS");
  }

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
    DeleteContract(id?: number) {
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
            this.contractsService.deleteContract(id).subscribe({
              next: () => {
                Alert.toast(TYPE.SUCCESS, true, 'Contract Deleted successfully');
                this.GetAllContracts(1, 10);
              },
              error: (error) => {
                console.error('Deletion Failed', error);
                this.errorMsg = JSON.stringify(error.error.message);
                Alert.toast(TYPE.ERROR, true, this.errorMsg);
              },
            });
          }
        }
      );
    }
    editContract(contract:ContractsEntity){
      console.log('Navigating to editContract with valueId:', contract.contractID);
      this.router.navigate(['contracts/editContract', contract.contractID]);
    }
}
