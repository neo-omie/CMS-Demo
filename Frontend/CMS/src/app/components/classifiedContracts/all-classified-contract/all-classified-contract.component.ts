import { Component, OnInit, Renderer2 } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { ClassifiedContractsService } from '../../../services/classified-contracts.service';
import { ClassifiedContracts, GetClassifiedContractByIdDto } from '../../../models/classified-contracts';
import { Pagination } from '../../../utils/pagination';
import { Alert } from '../../../utils/alert';
import { TYPE } from '../../auth/login/values.constants';
import { LoaderComponent } from "../../loader/loader.component";
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-all-classified-contract',
  standalone: true,
  imports: [LoaderComponent,FormsModule, CommonModule, RouterModule],
  templateUrl: './all-classified-contract.component.html',
  styleUrl: './all-classified-contract.component.css'
})
export class AllClassifiedContractComponent implements OnInit{

  loading: boolean = true;
  maxPage = 1;
  pageNumbers = [1, 1, 2, 3, 4, 5];
  errorMsg:string = "";
  allContracts: ClassifiedContracts[] = [];
  contractDetails?: GetClassifiedContractByIdDto;

  ngOnInit(): void {
    this.GetAllContracts(1, 10);
  }
  constructor(private contractsService: ClassifiedContractsService, private router: Router,private renderer : Renderer2) {}

  GetAllContracts(pageNumber: number, pageSize: number) {
      this.contractsService.getContracts(pageNumber, pageSize).subscribe({
        next: (res: ClassifiedContracts[]) => {
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
        error: (error: { message: undefined; error: { title: any; }; }) => {
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
        next: (response: GetClassifiedContractByIdDto) => {
          this.contractDetails = response;
          console.log(response);
        },
        error: (error: { message: undefined; error: { message: any; }; }) => {
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
              error: (error: { error: { message: any; }; }) => {
                console.error('Deletion Failed', error);
                this.errorMsg = JSON.stringify(error.error.message);
                Alert.toast(TYPE.ERROR, true, this.errorMsg);
              },
            });
          }
        }
      );
    }

}
