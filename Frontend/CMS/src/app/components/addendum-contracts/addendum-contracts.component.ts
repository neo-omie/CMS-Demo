import { Component, TemplateRef, ViewChild } from '@angular/core';
import { AddAddendumContract } from '../../models/add-addendum-contract';
import { Title } from '@angular/platform-browser';
import { Alert } from '../../utils/alert';
import { TYPE } from '../auth/login/values.constants';
import { Pagination } from '../../utils/pagination';
import { AddAddendumContractsService } from '../../services/add-addendum-contracts.service';
import { AddendumContract } from '../../models/addendum-contract';
import { CommonModule } from '@angular/common';
import { TableComponent } from '../UtilComponents/table/table.component';

import { PaginationComponent } from '../UtilComponents/pagination/pagination.component';
import { ActivatedRoute, Router } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import { LoaderComponent } from '../UtilComponents/loader/loader.component';
import { PDFExport } from '../../utils/pdfExport';

@Component({
  selector: 'app-addendum-contracts',
  standalone: true,
  imports: [CommonModule,TableComponent,LoaderComponent,PaginationComponent],
  templateUrl: './addendum-contracts.component.html',
  styleUrl: './addendum-contracts.component.css'
})
export class AddendumContractsComponent {
  loading: boolean = true;
  //isEdit: boolean = false;
  maxPage: number = 1;
  errorMsg: string = "";
  addAddendumContract?: AddAddendumContract;
  approverCheck: boolean = true;
  pageNumbers: number[] = [];
  addAddendumContracts: AddAddendumContract[] = [];
  displayedColumns: string[] = ['contractName', 'addendumDate', 'status', 'action'];
  // dataSource = new MatTableDataSource<AddAddendumContract>();
  columnsInfo: {
    [key: string]: {
      'title'?: string,
      'isSort'?: boolean,
      'templateRef': TemplateRef<any> | null,
    }
  } = {};

  @ViewChild('actionTemplateRef', { static: true }) actionTemplateRef!: TemplateRef<any>;
  @ViewChild('statusTemplateRef', { static: true }) statusTemplateRef!: TemplateRef<any>;

  constructor(private addAddendumContractsService: AddAddendumContractsService, private router:Router, private title: Title, private route:ActivatedRoute) {
    this.title.setTitle("Approval Matrix (Contract) - CMS");
  }

  ngOnInit() {
    this.route.params.subscribe(params=>{
      console.log('Route Params:', params);
      const paramValueId=params['contractId'];
      if(paramValueId){
        if(Number(paramValueId) && paramValueId > 0){
          this.GetAllAddendum(1, 10, paramValueId);
        }
        else{
          this.router.navigate(['page-not-found']);
        }
      }
      else{
        this.GetAllAddendum(1, 10);
      }
  })

    this.columnsInfo = {
      'contractName': {
        'title': 'Contract Name',
        'isSort': true,
        'templateRef': null
      },
      'addendumDate': {
        'title': 'Addendum Date',
        'isSort': true,
        'templateRef': null
      },
      'status':{
        'title': 'Status',
        'isSort': true,
        'templateRef': this.statusTemplateRef
      },
      'action': {
        'title': 'Action',
        'templateRef': this.actionTemplateRef
      }
    };
  }

  GetAllAddendum(pageNumber: number, pageSize: number, id:number = 0) {
    this.addAddendumContractsService.GetAllAddendum(pageNumber, pageSize, id).subscribe({
      next: (response: AddendumContract) => {
        this.loading = false;
        this.addAddendumContracts = response.data;
        console.log(response.data)
        if (this.addAddendumContracts != undefined && this.addAddendumContracts.length > 0) {
          let result = Pagination.paginator(pageNumber, response.totalCount, pageSize)
          this.maxPage = result.maxPage;
          this.pageNumbers = result.pageNumbers;
        }
      },
      error: (error) => {
        this.loading = false;
        console.error('Error :(', error);
        this.errorMsg = JSON.stringify((error.message !== undefined) ? error.error.title : error.message);
        Alert.toast(TYPE.ERROR, true, this.errorMsg);
      }
    });
  }

  GetPage(pgNumber: number) {
    if (this.maxPage >= pgNumber && pgNumber >= 1) {
      this.GetAllAddendum(pgNumber, 10);
    }
  }

  async approveRejectContract(contractId?:number, id?: number, status?: number) {
      this.loading = true;
      console.log('came here')
      let email = localStorage.getItem('email');
      if (email) {
        try {
          const response = await firstValueFrom(this.addAddendumContractsService.approveRejectContract(contractId, id, email, status))
          if (response !== false) {
            Alert.toast(TYPE.SUCCESS, true, 'Updated successfully');
            this.GetAllAddendum(1, 10);
          }
        }
        catch (error) {
          this.errorMsg = JSON.stringify(error);
          Alert.toast(TYPE.ERROR, true, this.errorMsg);
          console.error(error);
        }
        finally {
          this.loading = false
        }
      }
      else {
        this.router.navigate(['/']);
      }
      this.loading = false;
    }

    fetchAddedumData(addedumId:number){
      this.addAddendumContractsService.GetAddenduByAddendumId(addedumId).subscribe({
        next:(response)=>{
          this.addAddendumContract=response;
          if ((this.addAddendumContract.approver1Email == localStorage.getItem('email') &&
          this.addAddendumContract.approver1Status == 1) ||
          (this.addAddendumContract.approver2Email == localStorage.getItem('email') &&
            this.addAddendumContract.approver1Status == 2 &&
            this.addAddendumContract.approver2Status == 1) ||
          (this.addAddendumContract.approver3Email == localStorage.getItem('email') &&
            this.addAddendumContract.approver1Status == 2 &&
            this.addAddendumContract.approver2Status == 2 &&
            this.addAddendumContract.approver3Status == 1)
        ) {
          this.approverCheck = true;
        } else {
          this.approverCheck = false;
        }
        }
      })
    }
    printToPDF(tableID: string, fileName: string) {
    PDFExport.printToPDF(tableID, fileName);
  }
}
