import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { LoaderComponent } from '../loader/loader.component';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { ContractListResponse, ContractTypeMaster, ContractTypeMasterDTO } from '../../models/contract-type-master';
import { ContractTypeMasterService } from '../../services/contract-type-master.service';
import { Pagination } from '../../utils/pagination';
import { Alert } from '../../utils/alert';
import { TYPE } from '../auth/login/values.constants';

@Component({
  selector: 'app-contract-type-master',
  standalone: true,
  imports: [CommonModule,LoaderComponent,FormsModule,RouterModule],
  templateUrl: './contract-type-master.component.html',
  styleUrl: './contract-type-master.component.css'
})
export class ContractTypeMasterComponent implements OnInit {
loading: boolean = true;
  maxPage = 1;
  pageNumbers = [1, 1, 2, 3, 4, 5];
  masterContract: ContractListResponse[]=[];
  showContract: ContractTypeMasterDTO[]=[];
  comp?:ContractTypeMaster;
  errorMsg:string = '';

  constructor(
      private contractService: ContractTypeMasterService,
      private router: Router
    ) {}

  ngOnInit(): void {
    this.getContract(1, 10);
  }

  getContract(pageNumber:number, pageSize:number):void{
    this.contractService.getContract(pageNumber, pageSize)
    .subscribe({
      next:(res:ContractTypeMasterDTO[])=>{
        this.loading=false;
        this.showContract=res;
        console.log(res);
        if (this.showContract!=undefined && this.showContract.length>0) {
            let result=Pagination.paginator(pageNumber,this.showContract[0].totalRecords,pageSize)
            this.maxPage=result.maxPage;
            this.pageNumbers=result.pageNumbers
        }
        
      }, error:(error) => {
              this.loading = false;
              console.error('Error :(', error);
              this.errorMsg = JSON.stringify((error.message !== undefined)?error.error.title: error.message);
              Alert.toast(TYPE.ERROR,true,this.errorMsg);
    }
  });
  }

  GetPage(pgNumber: number) {
    if (this.maxPage >= pgNumber && pgNumber >= 1) {
      this.getContract(pgNumber, 10);
    }
  }

}
