import { CommonModule } from '@angular/common';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { LoaderComponent } from '../loader/loader.component';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { ContractListResponse, ContractTypeMaster, ContractTypeMasterDTO } from '../../models/contract-type-master';
import { ContractTypeMasterService } from '../../services/contract-type-master.service';
import { Pagination } from '../../utils/pagination';
import { Alert } from '../../utils/alert';
import { TYPE } from '../auth/login/values.constants';
import { Title } from '@angular/platform-browser';

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
  cont?:ContractTypeMaster;
  errorMsg:string = '';
  @ViewChild('editContractName') editContractName!: ElementRef;

  constructor(
      private contractService: ContractTypeMasterService,
      private router: Router,
      private title:Title
    ) {
      this.title.setTitle("Contract Type Master - CMS");
    }

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


  //get contrat by id 
GetContract(id:number){
  console.log("fetch id",id);
  
    this.contractService.getContractById(id).subscribe({
      next:(res:ContractTypeMaster)=>{
      this.cont=res;
      console.log(res);
      
      },
      error:(error)=>{
        console.error('Error :(', error);
        this.errorMsg = JSON.stringify((error.message !== undefined)?error.error.title: error.message);
        Alert.toast(TYPE.ERROR,true,this.errorMsg);
      }
        })
  }


  //edit contract 
  editContract(id:number){
    let compName=this.editContractName.nativeElement.value;
    if (compName !=="") {
      console.log(compName);
      this.contractService.updateContract(id,compName).subscribe({
        next:(res:boolean)=>{
          if (res) {
            Alert.toast(TYPE.SUCCESS,true,"Updated Successfully")
            this.getContract(1,10);
          }
        },
        error:(error)=>{
          console.error('Error :(', error);
              this.errorMsg = JSON.stringify((error.message !== undefined)?error.error.title: error.message);
              Alert.toast(TYPE.ERROR,true,this.errorMsg);
        }
      })
      
    }
  }

  //delete contract 
  deleteContract(id:number){
    // let askFirst:boolean = confirm("Are you sure you want to delete this Contract?");
    Alert.confirmToast("Are you sure you want to delete this Contract?",
                       "You won't be able to revert this!", TYPE.WARNING,
                       "Yes, delete it!",
                       "Deleted successfully!",
                       "Contract has been deleted.", TYPE.SUCCESS,() => {
                        this.contractService.deleteContract(id).subscribe({
                          next:(response:boolean)=>{
                            if(response){
                              Alert.toast(TYPE.SUCCESS,true,"Deleted successfully");
                              this.getContract(1,10);
                            }
                    
                          },
                          error:(error)=>{
                            console.error('Error :(', error);
                            this.errorMsg = JSON.stringify((error.message !== undefined)?error.error.title: error.message);
                            Alert.toast(TYPE.ERROR,true,this.errorMsg);
                          }
                        });
                       });
  }

}
