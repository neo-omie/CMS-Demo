import { CommonModule } from '@angular/common';
import { Component, ElementRef, OnInit, Type, ViewChild } from '@angular/core';
import { LoaderComponent } from '../loader/loader.component';
import { FormControl, FormGroup, FormsModule, NgForm, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AddContractDTO, ContractListResponse, ContractTypeMaster, ContractTypeMasterDTO } from '../../models/contract-type-master';
import { ContractTypeMasterService } from '../../services/contract-type-master.service';
import { Pagination } from '../../utils/pagination';
import { Alert } from '../../utils/alert';
import { TYPE } from '../auth/login/values.constants';
import { Title } from '@angular/platform-browser';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';

@Component({
  selector: 'app-contract-type-master',
  standalone: true,
  imports: [CommonModule, LoaderComponent, FormsModule, RouterModule, ReactiveFormsModule,
            MatTableModule, MatSortModule, MatFormFieldModule, MatInputModule],
  templateUrl: './contract-type-master.component.html',
  styleUrl: './contract-type-master.component.css'
})
export class ContractTypeMasterComponent implements OnInit {
  loading: boolean = true;
  displayedColumns: string[] = ['contractTypeName', 'status', 'action'];
      dataSource = new MatTableDataSource<ContractTypeMasterDTO>();
      @ViewChild(MatSort) sort!: MatSort;
      ngAfterViewInit() {
        this.dataSource.sort = this.sort;
      }
  maxPage = 1;
  pageNumbers = [1, 1, 2, 3, 4, 5];
  masterContract: ContractListResponse[] = [];
  showContract: ContractTypeMasterDTO[] = [];
  cont: AddContractDTO = new AddContractDTO();
  errorMsg: string = '';
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

  getContract(pageNumber: number, pageSize: number): void {
    this.contractService.getContract(pageNumber, pageSize)
      .subscribe({
        next: (res: ContractTypeMasterDTO[]) => {
          this.loading = false;
          this.dataSource.data = res;
          if (this.sort) {
            this.dataSource.sort = this.sort;
          }
          this.showContract = res;
          console.log(res);
          if (this.showContract != undefined && this.showContract.length > 0) {
            let result = Pagination.paginator(pageNumber, this.showContract[0].totalRecords, pageSize)
            this.maxPage = result.maxPage;
            this.pageNumbers = result.pageNumbers
          }

        }, error: (error) => {
          this.loading = false;
          console.error('Error :(', error);
          this.errorMsg = JSON.stringify((error.message !== undefined) ? error.error.title : error.message);
          Alert.toast(TYPE.ERROR, true, this.errorMsg);
        }
      });
  }

  GetPage(pgNumber: number) {
    if (this.maxPage >= pgNumber && pgNumber >= 1) {
      this.getContract(pgNumber, 10);
    }
  }


  //get contrat by id 
  GetContract(id: number) {
    console.log("fetch id", id);

    this.contractService.getContractById(id).subscribe({
      next: (res: ContractTypeMaster) => {
        this.cont = res;
        console.log(res);

      },
      error: (error) => {
        console.error('Error :(', error);
        this.errorMsg = JSON.stringify((error.message !== undefined) ? error.error.title : error.message);
        Alert.toast(TYPE.ERROR, true, this.errorMsg);
      }
    })
  }


  //edit contract 
  // editContract(id: number) {
  //   let compName = this.editContractName.nativeElement.value;
  //   if (compName !== "") {
  //     console.log(compName);
  //     this.contractService.updateContract(id, compName).subscribe({
  //       next: (res: boolean) => {
  //         if (res) {
  //           Alert.toast(TYPE.SUCCESS, true, "Updated Successfully")
  //           this.getContract(1, 10);
  //         }
  //       },
  //       error: (error) => {
  //         console.error('Error :(', error);
  //         this.errorMsg = JSON.stringify((error.message !== undefined) ? error.error.title : error.message);
  //         Alert.toast(TYPE.ERROR, true, this.errorMsg);
  //       }
  //     })

  //   }
  // }

  //delete contract 
  deleteContract(id: number) {
    // let askFirst:boolean = confirm("Are you sure you want to delete this Contract?");
    Alert.confirmToast("Are you sure you want to delete this Contract?",
      "You won't be able to revert this!", TYPE.WARNING,
      "Yes, delete it!",
      "Deleted successfully!",
      "Contract has been deleted.", TYPE.SUCCESS, () => {
        this.contractService.deleteContract(id).subscribe({
          next: (response: boolean) => {
            if (response) {
              // Alert.toast(TYPE.SUCCESS, true, "Deleted successfully");
              this.getContract(1, 10);
            }

          },
          error: (error) => {
            console.error('Error :(', error);
            this.errorMsg = JSON.stringify((error.message !== undefined) ? error.error.title : error.message);
            Alert.toast(TYPE.ERROR, true, this.errorMsg);
          }
        });
      });
  }

  //add contract
  addCompany(contractForm: NgForm) {
    this.cont = contractForm.value;

    this.contractService.addContract(this.cont).subscribe({
      next: (response) => {
        Alert.bigToast('Success!', 'Contract added successfully.', TYPE.SUCCESS, 'Ok');
        contractForm.resetForm();
        this.GetPage(this.maxPage);
      },
      error: (error) => {
        console.error('Error adding Contract:', error);
        Alert.bigToast('Error!', 'There was an error adding the Contract.', TYPE.ERROR, 'Try Again');
      }
    });
  }

  contractTypeMasterAddForm = new FormGroup({
    contractTypeName: new FormControl('', [Validators.required]),
    status: new FormControl('', [Validators.required])
  })
  onAddFormSubmitContract() {
    console.log(this.contractTypeMasterAddForm.value);
    
    if (this.contractTypeMasterAddForm.invalid) {
      this.contractTypeMasterAddForm.markAllAsTouched();
      return;
    }
    else {
      const contractTypeName = this.contractTypeMasterAddForm.value.contractTypeName;
      const status = this.contractTypeMasterAddForm.value.status;
        const addFormValues: AddContractDTO = new AddContractDTO();
        addFormValues.contractTypeName = this.contractTypeMasterAddForm.value.contractTypeName;
        addFormValues.status = Number(status) == 1 ? true : false;
        console.log(this.contractTypeMasterAddForm);
        this.contractService.addContract(addFormValues).subscribe({
          next: (response: ContractTypeMaster) => {
            if (response != undefined || response != null) {
              Alert.toast(TYPE.SUCCESS, true, 'Added Successfully');
              this.getContract(1, 10);
            }
          },
          error: (error) => {
            console.error('Error :(', error);
            this.errorMsg = JSON.stringify((error.message !== undefined) ? error.error.title : error.message);
            Alert.toast(TYPE.ERROR, true, this.errorMsg);
          }
        });
    }
  }
  get contractTypeName(){
    return this.contractTypeMasterAddForm.get('contractTypeName');
  }
  get status(){
    return this.contractTypeMasterAddForm.get('status');
  }

  contID:number = 0
    fetchContractData(contractID:number) {
      this.contID = contractID;
      this.contractService.getContractById(contractID).subscribe({
        next: (res:ContractTypeMaster) => {
          this.contractTypeMasterAddForm.patchValue({
            contractTypeName: res.contractTypeName,
            status: String(Number(res.status))
           
          });
        }
      })
    }
    onUpdateFormSubmit(){
      if(this.contractTypeMasterAddForm.invalid){
        this.contractTypeMasterAddForm.markAllAsTouched();
        return;
      }
      else{
        const contractTypeName = this.contractTypeMasterAddForm.value.contractTypeName;
        const Status = this.contractTypeMasterAddForm.value.status;
        const updatFormValues:AddContractDTO = new AddContractDTO();
        updatFormValues.contractTypeName=contractTypeName;
        updatFormValues.status= Number(Status) == 1 ? true : false;
        console.log(updatFormValues);
        this.contractService.updateContract(this.contID, updatFormValues).subscribe({
          next: (response: ContractTypeMaster) => {
            if(response != undefined && response != null)
            {
              Alert.toast(TYPE.SUCCESS, true, 'Updated Successfully')
              this.contractTypeMasterAddForm.reset();
              this.getContract(1, 10);
            }
          }, error: (error) => {
            Alert.toast(TYPE.ERROR, true, error.error.message)
            console.error(error.error);
          }
        })
      }

    }
  }




