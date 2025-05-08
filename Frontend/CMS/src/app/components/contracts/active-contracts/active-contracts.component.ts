import { Component, ElementRef, OnInit, Renderer2, ViewChild } from '@angular/core';
import { TYPE } from '../../auth/login/values.constants';
import { Alert } from '../../../utils/alert';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { AddContractDto, ContractsEntity, GetContractByIdDto } from '../../../models/contracts';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MasterEmployee } from '../../../models/master-employee';
import { GetAllDepartmentsDto } from '../../../models/master-department';
import { ContractTypeMasterDTO } from '../../../models/contract-type-master';
import { MasterApostille, MasterApostilleDto } from '../../../models/master-apostille';
import { CompanyMasterDto } from '../../../models/master-company';
import { ContractsService } from '../../../services/contracts.service';
import { Router, RouterModule } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { MasterApostilleService } from '../../../services/master-apostille.service';
import { Pagination } from '../../../utils/pagination';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { LoaderComponent } from '../../loader/loader.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-active-contracts',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterModule, LoaderComponent, ReactiveFormsModule, MatTableModule, MatSortModule, MatFormFieldModule, MatInputModule],
  templateUrl: './active-contracts.component.html',
  styleUrl: './active-contracts.component.css'
})
export class ActiveContractsComponent implements OnInit{
  displayedColumns: string[] = ['contractID', 'contractName', 'contractType', 'departmentName', 'effectiveDate',
                                  'expiryDate', 'toBeRenewedOn', 'addendumDate', 'status', 'approvalPendingFrom',
                                  'renewalContractPerson', 'renewalDueIn', 'location', 'action'];
        dataSource = new MatTableDataSource<ContractsEntity>();
        @ViewChild(MatSort) sort!: MatSort;
        ngAfterViewInit() {
          this.dataSource.sort = this.sort;
        }
        addBtn:string = '';
    loading: boolean = true;
    maxPage = 1;
    pageNumbers = [1, 1, 2, 3, 4, 5];
    errorMsg:string = "";
    allContracts: ContractsEntity[] = [];
    contractDetails?: GetContractByIdDto;
    approverCheck: boolean = true;
    mode:any;
    deptID?:number;
    // Dropdowns
    employeeCustodians:MasterEmployee[] = [];
    departments:GetAllDepartmentsDto[] = [];
    contractTypes:ContractTypeMasterDTO[] = [];
    apostilleTypes:MasterApostille[] = [];
    companies:CompanyMasterDto[] =[]
    ngOnInit(): void {
      this.GetAllContracts(1, 10);
      this.getAllDepartments();
      this.getAllContractTypes();
      this.getAllApostilleTypes();
      this.getAllCompanies();
    }
    constructor(private contractsService: ContractsService, private router: Router, private renderer : Renderer2, private title:Title, private masterApostilleService : MasterApostilleService ) {
      this.title.setTitle("All Contracts - CMS");
    }
    @ViewChild('editEmpCustodianCollapse') editEmpCustodianCollapse!: ElementRef;
    @ViewChild('editEmpCustodianName') editEmpCustodianName!: ElementRef;
    @ViewChild('editEmpCustodianId') editEmpCustodianId!: ElementRef;
    @ViewChild('addContractModal') addContractModal!: ElementRef;
    @ViewChild('addEmpCustodianName') addEmpCustodianName!: ElementRef;
    @ViewChild('addEmpCustodianId') addEmpCustodianId!: ElementRef;
    @ViewChild('addEmpCustodianCollapse') addEmpCustodianCollapse!: ElementRef;
     
    
  
    GetAllContracts(pageNumber: number, pageSize: number) {
        this.contractsService.getActiveContracts(pageNumber, pageSize).subscribe({
          next: (res: ContractsEntity[]) => {
            this.loading = false;
            this.dataSource.data = res;
            console.log(this.dataSource.data)
            if (this.sort) {
              this.dataSource.sort = this.sort;
            }
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
            // Checking if the approver is the one who's logged in or not
            if(this.contractDetails.approver1Email == localStorage.getItem('email')) {
              this.approverCheck = false;
              console.log(this.approverCheck);
            } else {
              this.approverCheck = true;
            }
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
                  // Alert.toast(TYPE.SUCCESS, true, 'Contract Deleted successfully');
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
  
      getAllDepartments() {
        this.contractsService.GetDepartments().subscribe({
          next: (response:GetAllDepartmentsDto[]) => {
            this.departments = response;
          }, error: (error) => {
            console.error('Error :(', error);
            this.errorMsg = JSON.stringify((error.message !== undefined)?error.error.title: error.message);
            Alert.toast(TYPE.ERROR,true,this.errorMsg);
          }
        });
      }
      getAllContractTypes() {
        this.contractsService.GetContractTypes().subscribe({
          next: (response:ContractTypeMasterDTO[]) => {
            this.contractTypes = response;
          }, error: (error) => {
            console.error('Error :(', error);
            this.errorMsg = JSON.stringify((error.message !== undefined)?error.error.title: error.message);
            Alert.toast(TYPE.ERROR,true,this.errorMsg);
          }
        });
      }
      getAllApostilleTypes() {
        this.masterApostilleService.getApostilles(1,100).subscribe({
          next: (response:MasterApostilleDto) => {
            this.apostilleTypes = response.data;
            console.log(this.apostilleTypes)
          }, error: (error) => {
            console.error('Error :(', error);
            this.errorMsg = JSON.stringify((error.message !== undefined)?error.error.title: error.message);
            Alert.toast(TYPE.ERROR,true,this.errorMsg);
          }
        });
      }
      getAllCompanies() {
        this.contractsService.GetCompanies().subscribe({
          next: (response:CompanyMasterDto[]) => {
            this.companies = response;
          }, error: (error) => {
            console.error('Error :(', error);
            this.errorMsg = JSON.stringify((error.message !== undefined)?error.error.title: error.message);
            Alert.toast(TYPE.ERROR,true,this.errorMsg);
          }
        });
      }
      masterContractAddForm = new FormGroup({
          contractName : new FormControl('',[Validators.required]),
          departmentId : new FormControl('',[Validators.required]),
          contractWithCompanyId : new FormControl('',[Validators.required]),
          contractTypeId : new FormControl('',[Validators.required]),
          apostilleTypeId : new FormControl('',[Validators.required]),
          actualDocRefNo : new FormControl('',[Validators.required]),
          retainerContract : new FormControl('',[Validators.required]),
          termsAndConditions : new FormControl('',[Validators.required]),
          validFrom : new FormControl('',[Validators.required]),
          validTill : new FormControl('',[Validators.required]),
          renewalFrom : new FormControl('',[Validators.required]),
          renewalTill : new FormControl('',[Validators.required]),
          addendumDate : new FormControl('',[Validators.required]),
          empCustodianId : new FormControl('',[Validators.required]),
          location : new FormControl('',[Validators.required]),
          approver1Status : new FormControl('1',[Validators.required,Validators.pattern('^[0-9]$')]),
          approver2Status : new FormControl('1',[Validators.required,Validators.pattern('^[0-9]$')]),
          approver3Status : new FormControl('1',[Validators.required,Validators.pattern('^[0-9]$')])
        })
        textChangeEmployeeCustodian(departmentId:number, event:Event, approverNumber:number){
          let input = event.target as HTMLInputElement;
          this.contractsService.GetEmployeeForInputText(departmentId,input.value).subscribe(
            {
              next:(response:MasterEmployee[]) => {
                if(approverNumber == 1){
                  this.employeeCustodians = response;
                }
              },
              error:(error) => {
                console.error('Error :(', error);
                this.errorMsg = JSON.stringify((error.message !== undefined)?error.error.title: error.message);
                Alert.toast(TYPE.ERROR,true,this.errorMsg);
              }
            }
          )
        }
        fillEmployeeCustodian(employeeId:number, employeeName:string, inputNumber:number){
          if(inputNumber == 1){
            const input = this.editEmpCustodianCollapse.nativeElement.querySelector('input');
            input.value = "";
            console.log(input.value);
            this.employeeCustodians.length = 0;
            this.renderer.removeClass(this.editEmpCustodianCollapse.nativeElement,'show');
            this.renderer.removeClass(this.addEmpCustodianCollapse.nativeElement,'show');
            this.editEmpCustodianName.nativeElement.value = employeeName;
            this.editEmpCustodianId.nativeElement.value = employeeId;
            this.addEmpCustodianName.nativeElement.value = employeeName;
            this.addEmpCustodianId.nativeElement.value = employeeId;
            console.log(employeeId);
          }
        }
        get contractName(){
          return this.masterContractAddForm.get('contractName');
        }
        get departmentId(){
          return this.masterContractAddForm.get('departmentId');
        }
        get contractWithCompanyId(){
          return this.masterContractAddForm.get('contractWithCompanyId');
        }
        get contractTypeId(){
          return this.masterContractAddForm.get('contractTypeId');
        }
        get apostilleTypeId(){
          return this.masterContractAddForm.get('apostilleTypeId');
        }
        get actualDocRefNo(){
          return this.masterContractAddForm.get('actualDocRefNo');
        }
        get retainerContract(){
          return this.masterContractAddForm.get('retainerContract');
        }
        get termsAndConditions(){
          return this.masterContractAddForm.get('termsAndConditions');
        }
        get validFrom(){
          return this.masterContractAddForm.get('validFrom');
        }
        get validTill(){
          return this.masterContractAddForm.get('validTill');
        }
        get renewalFrom(){
          return this.masterContractAddForm.get('renewalFrom');
        }
        get renewalTill(){
          return this.masterContractAddForm.get('renewalTill');
        }
        get addendumDate(){
          return this.masterContractAddForm.get('addendumDate');
        }
        get empCustodianId(){
          return this.masterContractAddForm.get('empCustodianId');
        }
        get location(){
          return this.masterContractAddForm.get('location');
        }
        get approver1Status(){
          return this.masterContractAddForm.get('approver1Status');
        }
        get approver2Status(){
          return this.masterContractAddForm.get('approver2Status');
        }
        get approver3Status(){
          return this.masterContractAddForm.get('approver3Status');
        }
      
        onClick(){
          this.router.navigate(['contracts/allContracts']);
          this.masterContractAddForm.reset();
        }
        contID:number = 0;
        fetchContractData(contractID:number) {
          this.contID = contractID;
          this.contractsService.fetchContractData(contractID).subscribe({
            next: (response) => {
              this.masterContractAddForm.patchValue({
                contractName: response.contractName,
                departmentId: String(response.departmentId),
                contractWithCompanyId: String(response.contractWithCompanyId),
                contractTypeId: String(response.contractTypeId),
                apostilleTypeId: String(response.apostilleTypeId),
                actualDocRefNo: String(response.actualDocRefNo),
                retainerContract: String(response.retainerContract),
                termsAndConditions: response.termsAndConditions,
                validFrom: this.formatDate(String(response.validFrom)),
                validTill: this.formatDate(String(response.validTill)),
                renewalFrom: this.formatDate(String(response.renewalFrom)),
                renewalTill: this.formatDate(String(response.renewalTill)),
                addendumDate: this.formatDate(String(response.addendumDate)),
                empCustodianId: String(response.empCustodianId),
                location: response.location,
                approver1Status: String(response.approver1Status),
                approver2Status: String(response.approver2Status),
                approver3Status: String(response.approver3Status)
              });
              this.editEmpCustodianId.nativeElement.value = response.empCustodianId;
              this.editEmpCustodianName.nativeElement.value = response.empCustodianId;
            }
          })
        }
        private formatDate(date:string) {
          const d = new Date(date);
          let month = '' + (d.getMonth() + 1);
          let day = '' + d.getDate();
          const year = d.getFullYear();
          if (month.length < 2) month = '0' + month;
          if (day.length < 2) day = '0' + day;
          return [year, month, day].join('-');
          }
        onUpdateFormSubmit(contractID:number) {
          this.masterContractAddForm.get('empCustodianId')?.setValue(this.editEmpCustodianId.nativeElement.value)
          if (this.masterContractAddForm.invalid) {
            this.masterContractAddForm.markAllAsTouched();
            return;
          }
          else {
            const departmentId = this.masterContractAddForm.value.departmentId;
            const contractWithCompanyId = this.masterContractAddForm.value.contractWithCompanyId;
            const contractTypeId = this.masterContractAddForm.value.contractTypeId;
            const apostilleTypeId = this.masterContractAddForm.value.apostilleTypeId;
            const actualDocRefNo = this.masterContractAddForm.value.actualDocRefNo;
            const retainerContract = this.masterContractAddForm.value.retainerContract;
            const empCustodianId = this.masterContractAddForm.value.empCustodianId;
            const approver1Status = this.masterContractAddForm.value.approver1Status;
            const approver2Status = this.masterContractAddForm.value.approver2Status;
            const approver3Status = this.masterContractAddForm.value.approver3Status;
            if (departmentId && Number(departmentId) &&
              contractWithCompanyId && Number(contractWithCompanyId) &&
              contractTypeId && Number(contractTypeId) &&
              apostilleTypeId && Number(apostilleTypeId) &&
              actualDocRefNo && Number(actualDocRefNo) &&
              retainerContract && Number(retainerContract) &&
              empCustodianId && Number(empCustodianId) &&
              approver1Status && Number(approver1Status) &&
              approver2Status && Number(approver2Status) &&
              approver3Status && Number(approver3Status)
            ) {
              const addFormValues: AddContractDto = new AddContractDto();
              addFormValues.contractName = this.masterContractAddForm.value.contractName;
              addFormValues.departmentId = Number(departmentId);
              addFormValues.contractWithCompanyId = Number(contractWithCompanyId);
              addFormValues.contractTypeId = Number(contractTypeId);
              addFormValues.apostilleTypeId = Number(apostilleTypeId);
              addFormValues.actualDocRefNo = Number(actualDocRefNo);
              addFormValues.retainerContract = Number(retainerContract);
              addFormValues.termsAndConditions = this.masterContractAddForm.value.termsAndConditions;
              addFormValues.validFrom = this.masterContractAddForm.value.validFrom;
              addFormValues.validTill = this.masterContractAddForm.value.validTill;
              addFormValues.renewalFrom = this.masterContractAddForm.value.renewalFrom;
              addFormValues.renewalTill = this.masterContractAddForm.value.renewalTill;
              addFormValues.addendumDate = this.masterContractAddForm.value.renewalTill;
              addFormValues.empCustodianId = Number(empCustodianId);
              addFormValues.location = this.masterContractAddForm.value.location;
              addFormValues.approver1Status = Number(approver1Status);
              addFormValues.approver2Status = Number(approver2Status);
              addFormValues.approver3Status = Number(approver3Status);
              console.log(addFormValues);
              this.contractsService.editContract(contractID, addFormValues).subscribe({
                next: (response: boolean) => {
                  if (response !== false) {
                    Alert.toast(TYPE.SUCCESS, true, 'Updated successfully');
                    // this.router.navigate(['contracts/allContracts'])
                    this.GetAllContracts(1, 10);
                    //this.renderer.removeClass(this.addContractModal.nativeElement, 'show');
                    this.masterContractAddForm.reset();
                  }
                },
                error: (error) => {
                  console.error('Error :(', error);
                  this.errorMsg = JSON.stringify((error.message !== undefined) ? error.error.title : error.message);
                  Alert.toast(TYPE.ERROR, true, this.errorMsg);
                }
              });
            }
            else {
              console.log("should not come here ", this.masterContractAddForm.value)
            }
          }
        }
}
