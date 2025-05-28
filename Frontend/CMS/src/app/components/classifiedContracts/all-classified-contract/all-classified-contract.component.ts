declare var bootstrap: any;

import { Component, ElementRef, Inject, OnInit, Renderer2, ViewChild } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { ClassifiedContractsService } from '../../../services/classified-contracts.service';
import { AddClassifiedContractDto, ClassifiedContracts, GetClassifiedContractByIdDto } from '../../../models/classified-contracts';
import { Pagination } from '../../../utils/pagination';
import { Alert } from '../../../utils/alert';
import { TYPE } from '../../auth/login/values.constants';
// import { LoaderComponent } from "../../loader/loader.component";
import { CommonModule, DatePipe, DOCUMENT } from '@angular/common';
import { FormControl, FormGroup, FormsModule, NgForm, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatInputModule } from '@angular/material/input';
import { ContractTypeMasterDTO } from '../../../models/contract-type-master';
import { MasterApostille, MasterApostilleDto } from '../../../models/master-apostille';
import { CompanyMasterDto } from '../../../models/master-company';
import { GetAllDepartmentsDto } from '../../../models/master-department';
import { MasterEmployee } from '../../../models/master-employee';
import { Title } from '@angular/platform-browser';
import { MasterApostilleService } from '../../../services/master-apostille.service';
import { PostTerminationNoticeUploadDTO } from '../../../models/post-termination-notice';
// import { AddAddendumContract } from '../../../models/add-addendum-contract';
import { AddAddendumContractsService } from '../../../services/add-addendum-contracts.service';
import { PDFExport } from '../../../utils/pdfExport';
import { ClassifiedPostTermination } from '../../../models/post-termination';
import {  ClassifiedApproveRejectWithdrawalDTO, WithdrawNoticeUploadDTO } from '../../../models/notice-withdrawal';
import { NoticeWithdrawalService } from '../../../services/notice-withdrawal.service';
import { firstValueFrom } from 'rxjs/internal/firstValueFrom';
import { PostTerminationService } from '../../../services/post-termination.service';
import { LoaderComponent } from '../../UtilComponents/loader/loader.component';
import { DecodeToken } from '../../../utils/decodeToken';
import { ProgressBarComponent } from '../../UtilComponents/progress-bar/progress-bar.component';


@Component({
  selector: 'app-all-classified-contract',
  standalone: true,
  imports: [LoaderComponent, FormsModule, CommonModule, RouterModule, ReactiveFormsModule, MatTableModule, MatSortModule, MatFormFieldModule, MatInputModule, ProgressBarComponent],
  templateUrl: './all-classified-contract.component.html',
  styleUrl: './all-classified-contract.component.css'
})
export class AllClassifiedContractComponent implements OnInit{
  
   displayedColumns: string[] = ['classifiedContractID', 'classifiedContractName', 'contractType', 'departmentName', 'effectiveDate',
                                  'expiryDate', 'toBeRenewedOn', 'addendumDate', 'status', 'approvalPendingFrom',
                                  'renewalContractPerson', 'renewalDueIn', 'location', 'action'];
        dataSource = new MatTableDataSource<ClassifiedContracts>();
        @ViewChild(MatSort) sort!: MatSort;
        ngAfterViewInit() {
          this.dataSource.sort = this.sort;
        }
        addBtn:string = '';
         file: File | null = null;
          loading: boolean = true;
          maxPage = 1;
          pageNumbers = [1, 1, 2, 3, 4, 5];
          errorMsg:string = "";
          allContracts: ClassifiedContracts[] = [];
          contractDetails?: GetClassifiedContractByIdDto;
           approverCheck: boolean = true;
           terminationCheck: boolean = true;
           withdrawCheck:boolean = true;
        
    mode:any;
    deptID?:number;
    // Dropdowns
    employeeCustodians:MasterEmployee[] = [];
    departments:GetAllDepartmentsDto[] = [];
    contractTypes:ContractTypeMasterDTO[] = [];
    apostilleTypes:MasterApostille[] = [];
    companies:CompanyMasterDto[] =[]
    postTerm: PostTerminationNoticeUploadDTO = new PostTerminationNoticeUploadDTO(null, 0, new Date(), '');
      withdrawNotice: WithdrawNoticeUploadDTO = new WithdrawNoticeUploadDTO(null, '');
      contIdForPostTerm?: number = 0;
    ngOnInit(): void {
      this.GetAllContracts(1, 10);
      this.getAllDepartments();
      this.getAllContractTypes();
      this.getAllApostilleTypes();
      this.getAllCompanies();
    }
  constructor(private addAddendumContractsService: AddAddendumContractsService
  ,private contractsService: ClassifiedContractsService, private router: Router,private renderer : Renderer2, private title:Title,private masterApostilleService: MasterApostilleService,
      private postTermService: PostTerminationService,
      private noticeWithdrawalService: NoticeWithdrawalService,
    @Inject(DOCUMENT) private document: Document ) {
    this.title.setTitle("All Classified Contracts - CMS");
  }
  @ViewChild('editEmpCustodianCollapse') editEmpCustodianCollapse!: ElementRef;
  @ViewChild('editEmpCustodianName') editEmpCustodianName!: ElementRef;
  @ViewChild('editEmpCustodianId') editEmpCustodianId!: ElementRef;
  @ViewChild('addContractModal') addContractModal!: ElementRef;
  @ViewChild('addEmpCustodianName') addEmpCustodianName!: ElementRef;
  @ViewChild('addEmpCustodianId') addEmpCustodianId!: ElementRef;
  @ViewChild('addEmpCustodianCollapse') addEmpCustodianCollapse!: ElementRef;
    @ViewChild('addAddendumEmpCustodianName') addAddendumEmpCustodianName!: ElementRef;
  @ViewChild('addAddendumEmpCustodianCollapse') addAddendumEmpCustodianCollapse!: ElementRef;
  @ViewChild('addAddendumEmpCustodianId') addAddendumEmpCustodianId!: ElementRef;
  @ViewChild('addFile') addFile!: ElementRef;
  
   GetAllContracts(pageNumber: number, pageSize: number) {
       this.contractsService.getContracts(pageNumber, pageSize).subscribe({
         next: (res: ClassifiedContracts[]) => {
           this.loading = false;
           this.dataSource.data = res;
           console.log(res);
           
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
              // console.log(this.apostilleTypes)
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

    GetContract(contractID: number) {
      this.contractsService.getContractByID(contractID).subscribe({
        next: (response: GetClassifiedContractByIdDto) => {
          console.log(response);
          
          this.contractDetails = response;
          this.contractDetails.validFrom = response.validFrom?.toString().split('T')[0]
          this.contractDetails.validTill = response.validTill?.toString().split('T')[0]
          this.contractDetails.renewalFrom = response.renewalFrom != null ? response.renewalFrom?.toString().split('T')[0] : ""
          this.contractDetails.renewalTill = response.renewalTill != null ? response.renewalTill?.toString().split('T')[0] : ""
          this.contractDetails.addendumDate = response.addendumDate != null ? response.addendumDate?.toString().split('T')[0] : ""
          console.log(response);
          if ((this.contractDetails.approver1Email == DecodeToken.email &&
          this.contractDetails.approver1Status == 1) ||
          (this.contractDetails.approver2Email == DecodeToken.email &&
            this.contractDetails.approver1Status == 2 &&
            this.contractDetails.approver2Status == 1) ||
          (this.contractDetails.approver3Email == DecodeToken.email &&
            this.contractDetails.approver1Status == 2 &&
            this.contractDetails.approver2Status == 2 &&
            this.contractDetails.approver3Status == 1)
        ) {
          this.approverCheck = true;
        } else {
          this.approverCheck = false;
        }
        if ((this.contractDetails.approver1Email == DecodeToken.email &&
          this.contractDetails.approver1Status == 6) ||
          (this.contractDetails.approver2Email == DecodeToken.email &&
            this.contractDetails.approver1Status == 7 &&
            this.contractDetails.approver2Status == 6) ||
          (this.contractDetails.approver3Email == DecodeToken.email &&
            this.contractDetails.approver1Status == 7 &&
            this.contractDetails.approver2Status == 7 &&
            this.contractDetails.approver3Status == 6)
        ) {
          this.terminationCheck = true;
        } else {
          this.terminationCheck = false;
        }
        if ((this.contractDetails.approver1Email == DecodeToken.email &&
          this.contractDetails.approver1Status == 8) ||
          (this.contractDetails.approver2Email == DecodeToken.email &&
            this.contractDetails.approver1Status == 2 &&
            this.contractDetails.approver2Status == 8) ||
          (this.contractDetails.approver3Email == DecodeToken.email &&
            this.contractDetails.approver1Status == 2 &&
            this.contractDetails.approver2Status == 2 &&
            this.contractDetails.approver3Status == 8)
        ) {
          this.withdrawCheck = true;
        } else {
          this.withdrawCheck = false;
        }
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

    //  editContract(contract: ClassifiedContracts) {
    //     console.log('Navigating to editContract with valueId:', contract.classifiedContractID);
    //     this.router.navigate(['contracts/editContract', contract.classifiedContractID]);
    //   }
    masterContractAddForm = new FormGroup({
      classifiedContractName : new FormControl('',[Validators.required]),
            departmentId : new FormControl('',[Validators.required]),
            contractWithCompanyId : new FormControl('',[Validators.required]),
            contractTypeId : new FormControl('',[Validators.required]),
            apostilleTypeId : new FormControl('',[Validators.required]),
            actualDocRefNo : new FormControl('',[Validators.required]),
            retainerContract : new FormControl('',[Validators.required]),
            termsAndConditions : new FormControl('',[Validators.required]),
            validFrom : new FormControl('',[Validators.required]),
            validTill : new FormControl('',[Validators.required]),
            renewalFrom : new FormControl(''),
            renewalTill : new FormControl(''),
            addendumDate : new FormControl(''),
            empCustodianId : new FormControl('',[Validators.required]),
            location : new FormControl('',[Validators.required]),
            approver1Status : new FormControl('1',[Validators.required,Validators.pattern('^[0-9]$')]),
            approver2Status : new FormControl('1',[Validators.required,Validators.pattern('^[0-9]$')]),
            approver3Status : new FormControl('1',[Validators.required,Validators.pattern('^[0-9]$')]),
            skipApproval : new FormControl(true,[Validators.required])
          })
          async onAddFormSubmit(){
            this.loading=true;
            this.masterContractAddForm.get('empCustodianId')?.setValue(this.editEmpCustodianId.nativeElement.value)
            if(this.masterContractAddForm.invalid){
              this.masterContractAddForm.markAllAsTouched();
              console.log("bla bla bla",this.masterContractAddForm.value)
              // Alert.toast(TYPE.WARNING, true, 'There is still few fields to fill out. Please fill all the required fields.');
              return;
            }
            else{
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
              if(departmentId && Number(departmentId) &&
              contractWithCompanyId && Number(contractWithCompanyId) &&
              contractTypeId && Number(contractTypeId) &&
              apostilleTypeId && Number(apostilleTypeId) &&
              actualDocRefNo && Number(actualDocRefNo) &&
              retainerContract && Number(retainerContract) &&
              empCustodianId && Number(empCustodianId) &&
              approver1Status && Number(approver1Status) &&
              approver2Status && Number(approver2Status) &&
              approver3Status && Number(approver3Status) 
              ){
                // console.log(this.masterContractAddForm.value.addendumDate);
                
                const addFormValues:AddClassifiedContractDto = new AddClassifiedContractDto();
                addFormValues.classifiedContractName = this.masterContractAddForm.value.classifiedContractName;
                addFormValues.departmentId = Number(departmentId);
                addFormValues.contractWithCompanyId = Number(contractWithCompanyId);
                addFormValues.contractTypeId = Number(contractTypeId);
                addFormValues.apostilleTypeId = Number(apostilleTypeId);
                addFormValues.actualDocRefNo = Number(actualDocRefNo);
                addFormValues.retainerContract = Number(retainerContract);
                addFormValues.termsAndConditions = this.masterContractAddForm.value.termsAndConditions;
                addFormValues.validFrom = this.masterContractAddForm.value.validFrom;
                addFormValues.validTill = this.masterContractAddForm.value.validTill;
                addFormValues.renewalFrom = this.masterContractAddForm.value.renewalFrom != "" ? String(this.masterContractAddForm.value.renewalFrom) : null;
                addFormValues.renewalTill = this.masterContractAddForm.value.renewalTill != "" ? String(this.masterContractAddForm.value.renewalTill) : null;
                addFormValues.addendumDate = this.masterContractAddForm.value.addendumDate != "" ? String(this.masterContractAddForm.value.addendumDate) : null;
                addFormValues.skipApproval =this.masterContractAddForm.value.skipApproval;
                addFormValues.empCustodianId = Number(empCustodianId);
                addFormValues.location = this.masterContractAddForm.value.location;
                addFormValues.approver1Status = Number(approver1Status);
                addFormValues.approver2Status = Number(approver2Status);
                addFormValues.approver3Status = Number(approver3Status);
                
                    try {
                              const response = await firstValueFrom(this.contractsService.addContract(addFormValues));
                              
                    if( response !== false){
                      // console.log(this.skipApproval);
                      Alert.toast(TYPE.SUCCESS,true,'Added successfully');
                      this.masterContractAddForm.reset({
                      skipApproval : true,
                      approver1Status : "1",
                      approver2Status : "1",
                      approver3Status : "1",
                      renewalFrom : "",
                      renewalTill : "",
                      addendumDate : ""
                    });
                    console.log("After rest : ",this.masterContractAddForm.value);
                      this.GetAllContracts(1, 10);
                      const modalElement = document.getElementById('contract-add');
            if (modalElement) {
              const modalInstance = bootstrap.Modal.getInstance(modalElement) || new bootstrap.Modal(modalElement);
              modalInstance.hide();
            }
                    }
                  }
                  // error:(error) => {
                    catch (error) {
                    console.error('Error :(', error);
                    // this.errorMsg = JSON.stringify((error.message !== undefined)?error.error.title: error.message);
                    // console.log(this.skipApproval);
                    this.errorMsg = JSON.stringify(error);
                    Alert.toast(TYPE.ERROR,true,this.errorMsg);
                    this.masterContractAddForm.reset();
                    this.masterContractAddForm.patchValue({
                      skipApproval : true,
                      approver1Status : "1",
                      approver2Status : "1",
                      approver3Status : "1",
                      renewalFrom : "",
                      renewalTill : "",
                      addendumDate : ""
                    })
                  }
                  finally {
          this.loading = false;
        }
                // });

              }
              else{
                console.log("should not come here 1", this.masterContractAddForm.value)
              }
            }
            // console.log("should not come here 2 ", this.masterContractAddForm.value)
            // console.log(this.skipApproval);
            // this.masterContractAddForm.reset();
            // this.masterContractAddForm.patchValue({
            //           skipApproval : true,
            //           approver1Status : "1",
            //           approver2Status : "1",
            //           approver3Status : "1",
            //})
          }
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
              // console.log(input.value);
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
          get classifiedContractName(){
            return this.masterContractAddForm.get('classifiedContractName');
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
          get skipApproval(){
            return this.masterContractAddForm.get('skipApproval');
          }
        
          onClick(){
            this.router.navigate(['classifiedContracts/allContracts']);
            this.masterContractAddForm.reset();
          }


           addaddendumForm = new FormGroup({
              addendumContractId: new FormControl('', [Validators.required]),
              classifiedContractID: new FormControl('', [Validators.required]),
              contractName: new FormControl('', [Validators.required]),
              departmentId: new FormControl('', [Validators.required]),
              contractWithCompanyId: new FormControl('', [Validators.required]),
              contractTypeId: new FormControl('', [Validators.required]),
              apostilleTypeId: new FormControl('', [Validators.required]),
              actualDocRefNo: new FormControl('', [Validators.required]),
              retainerContract: new FormControl('', [Validators.required]),
              termsAndConditions: new FormControl('', [Validators.required]),
              validFrom: new FormControl('', [Validators.required]),
              validTill: new FormControl('', [Validators.required]),
              empCustodianId: new FormControl('', [Validators.required])
            });
          

          contID:number = 0;

          fetchContractData(classifiedContractID:any) {
            this.contID = classifiedContractID;
            this.contractsService.fetchContractData(classifiedContractID).subscribe({
              next: (response) => {     
                this.masterContractAddForm.patchValue({
                  classifiedContractName: response.classifiedContractName,
                  departmentId: String(response.departmentId),
                  contractWithCompanyId: String(response.contractWithCompanyId),
                  contractTypeId: String(response.contractTypeId),
                  apostilleTypeId: String(response.apostilleTypeId),
                  actualDocRefNo: String(response.actualDocRefNo),
                  retainerContract: String(response.retainerContract),
                  termsAndConditions: response.termsAndConditions,
                  validFrom: this.formatDate(String(response.validFrom)),  
                  // validTill: this.formatDate(response.validTill),                    
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
               return true;
      },
      error: (err) => {
        console.error('No Contract with this id exist', err);
        this.errorMsg = JSON.stringify((err.message !== undefined) ? err.error.message : err.message);
        Alert.toast(TYPE.ERROR, true, this.errorMsg);
        return false;
      }
    })
    return false;
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
            // this.masterContractAddForm.get('empCustodianId')?.setValue(this.editEmpCustodianId.nativeElement.value)
            // if (this.masterContractAddForm.invalid) {
            //   this.masterContractAddForm.markAllAsTouched();
            //   return;
            // }
            // else {
            //   const departmentId = this.masterContractAddForm.value.departmentId;
            //   const contractWithCompanyId = this.masterContractAddForm.value.contractWithCompanyId;
            //   const contractTypeId = this.masterContractAddForm.value.contractTypeId;
            //   const apostilleTypeId = this.masterContractAddForm.value.apostilleTypeId;
            //   const actualDocRefNo = this.masterContractAddForm.value.actualDocRefNo;
            //   const retainerContract = this.masterContractAddForm.value.retainerContract;
            //   const empCustodianId = this.masterContractAddForm.value.empCustodianId;
            //   const approver1Status = this.masterContractAddForm.value.approver1Status;
            //   const approver2Status = this.masterContractAddForm.value.approver2Status;
            //   const approver3Status = this.masterContractAddForm.value.approver3Status;
            //   if (departmentId && Number(departmentId) &&
            //     contractWithCompanyId && Number(contractWithCompanyId) &&
            //     contractTypeId && Number(contractTypeId) &&
            //     apostilleTypeId && Number(apostilleTypeId) &&
            //     actualDocRefNo && Number(actualDocRefNo) &&
            //     retainerContract && Number(retainerContract) &&
            //     empCustodianId && Number(empCustodianId) &&
            //     approver1Status && Number(approver1Status) &&
            //     approver2Status && Number(approver2Status) &&
            //     approver3Status && Number(approver3Status)
            //   ) {
            //     const addFormValues: AddClassifiedContractDto = new AddClassifiedContractDto();
            //     addFormValues.classifiedContractName = this.masterContractAddForm.value.classifiedContractName;
            //     addFormValues.departmentId = Number(departmentId);
            //     addFormValues.contractWithCompanyId = Number(contractWithCompanyId);
            //     addFormValues.contractTypeId = Number(contractTypeId);
            //     addFormValues.apostilleTypeId = Number(apostilleTypeId);
            //     addFormValues.actualDocRefNo = Number(actualDocRefNo);
            //     addFormValues.retainerContract = Number(retainerContract);
            //     addFormValues.termsAndConditions = this.masterContractAddForm.value.termsAndConditions;
            //     addFormValues.validFrom = this.masterContractAddForm.value.validFrom;
            //     addFormValues.validTill = this.masterContractAddForm.value.validTill;
            //     addFormValues.renewalFrom = this.masterContractAddForm.value.renewalFrom;
            //     addFormValues.renewalTill = this.masterContractAddForm.value.renewalTill;
            //     addFormValues.addendumDate = this.masterContractAddForm.value.renewalTill;
            //     addFormValues.empCustodianId = Number(empCustodianId);
            //     addFormValues.location = this.masterContractAddForm.value.location;
            //     addFormValues.approver1Status = Number(approver1Status);
            //     addFormValues.approver2Status = Number(approver2Status);
            //     addFormValues.approver3Status = Number(approver3Status);
            //     console.log(addFormValues);
            //     this.contractsService.editContract(contractID, addFormValues).subscribe({
            //       next: (response: boolean) => {
            //         if (response !== false) {
            //           Alert.toast(TYPE.SUCCESS, true, 'Updated successfully');
            //           // this.router.navigate(['contracts/allContracts'])
            //           this.GetAllContracts(1, 10);
            //           //this.renderer.removeClass(this.addContractModal.nativeElement, 'show');
            //           this.masterContractAddForm.reset();
            //         }
            //       },
            //       error: (error) => {
            //         console.error('Error :(', error);
            //         this.errorMsg = JSON.stringify((error.message !== undefined) ? error.error.title : error.message);
            //         Alert.toast(TYPE.ERROR, true, this.errorMsg);
            //       }
            //     });
            //   }
            //   else {
            //     console.log("should not come here ", this.masterContractAddForm.value)
            //   }
            // }
          }

          // onAddAddendumFormSubmit(contractID: number) {
          //     const addendum = new AddAddendumContract();
          //     addendum.contractId = Number(this.addaddendumForm.value.contractId);
          //     addendum.contractName=String(this.addaddendumForm.value.contractName);
          //     addendum.departmentId = Number(this.addaddendumForm.value.departmentId);
          //     addendum.contractWithCompanyId = Number(this.addaddendumForm.value.contractWithCompanyId);
          //     addendum.contractTypeId = Number(this.addaddendumForm.value.contractTypeId);
          //     addendum.apostilleTypeId = Number(this.addaddendumForm.value.apostilleTypeId);
          //     addendum.actualDocRefNo = Number(this.addaddendumForm.value.actualDocRefNo);
          //     addendum.retainerContract = Number(this.addaddendumForm.value.retainerContract);
          //     addendum.termsAndConditions = String(this.addaddendumForm.value.termsAndConditions);
          //     addendum.validFrom = String(this.addaddendumForm.value.validFrom);
          //     addendum.validTill = String(this.addaddendumForm.value.validTill);
          //     addendum.empCustodianId = Number(this.addaddendumForm.value.empCustodianId);
          
          //     this.addAddendumContractsService.AddAddendum(addendum.contractId, addendum).subscribe({
          //       next: () => {
          //         Alert.toast(TYPE.SUCCESS, true, 'Approve Request to add addendum is sent to Approver 1');
          //         this.GetAllContracts(1, 10);
          //         this.masterContractAddForm.reset();
          //       },
          //       error: (err) => {
          //         console.error('Error adding addendum:', err);
          //         this.errorMsg = JSON.stringify((err.message !== undefined) ? err.error.title : err.message);
          //         Alert.toast(TYPE.ERROR, true, this.errorMsg);
          //       }
          //     })
          
          //   }

             checkContractId = new FormGroup({
                contractId: new FormControl('', [Validators.required])
              });
      
               onSubmitCheck() {
                  const enteredValue = this.checkContractId.value.contractId;
                  this.contractsService.getContracts(1, 100).subscribe({
                    next: (res: ClassifiedContracts[]) => {
                      this.dataSource.data = res;
                      console.log(this.dataSource.data);
                      this.allContracts = res;
                      console.log(this.allContracts);
                      if (this.checkContractId.valid) {
                        const foundContract = this.allContracts.find((contract) => contract.classifiedContractID.toString() === enteredValue
                          || contract.classifiedContractName === enteredValue);
              
                        if (foundContract) {
                          console.log('Contract Found: ', foundContract);
                        } else {
                          console.error("Contract not found");
                        }
                      }
                      else {
                        console.error('Form is invalid');
                      }
                    }
                  });
                }
                uploadFile(event: Event) {
                  const input = event.target as HTMLInputElement;
                  if (input.files?.length) {
                    // TODO check file size and type
                    this.file = input.files[0];
                  }
              
                  if (!this.file) {
                    Alert.toast(TYPE.WARNING, true, "Please select a file and fill the form correctly.");
                    return;
                  }
              
                  const allowedExtensions = ['.pdf', '.doc', '.docx', '.jpg', '.jpeg', '.png'];
                  const fileExtension = this.file.name.substring(this.file.name.lastIndexOf('.')).toLowerCase();
              
                  if (!allowedExtensions.includes(fileExtension)) {
                    Alert.toast(TYPE.WARNING, true, "Unsupported file format. Allowed formats: .pdf, .doc, .docx, .jpg, .jpeg and .png.");
                    return;
                  }
              
                  if (this.file.size > 25 * 1048576) {
                    Alert.toast(TYPE.WARNING, true, "File too large. Max 25MB allowed.");
                    return;
                  }
                }
              
                getContractIdforPostTerm(classifiedContractID?: string) {
                  this.contIdForPostTerm = Number(classifiedContractID);
                  console.log(this.contIdForPostTerm, classifiedContractID);
                }
                async approveRejectContract(id?: string, status?: number) {
                  this.loading = true;
                  console.log('came here 1')
                  console.log("id and status",id,status);
                  
                  let email = DecodeToken.email;
                  if (email) {
                    try {
                      const response = await firstValueFrom(this.contractsService.approveRejectContract(Number(id), email, status))
                      if (response !== false) {
                        Alert.toast(TYPE.SUCCESS, true, 'Updated successfully');
                        this.GetAllContracts(1, 10);
                      }
                    }
                    catch (error) {
                      this.errorMsg = JSON.stringify(error);
                      Alert.toast(TYPE.ERROR, true, this.errorMsg);
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
                //uploading the Post Termination Notice 
                OnSavePostTermination(documentForm: NgForm) {
                  // console.log(documentForm.value);
                  // console.log(this.file);
              
                  if (!this.file || !documentForm.valid) {
                    this.addFile.nativeElement.value = "";
                    this.postTerm.file = null
                    this.postTerm.notice_Duration = 1;
                    this.postTerm.end_Date = new Date();
                    this.postTerm.Remark = "";
                    Alert.toast(TYPE.WARNING, true, "Please select a file and fill the Form Correctly");
                    return;
                  }
                  const allowedExtensions = ['.pdf', '.doc', '.docx'];
                  const fileExtension = this.file.name.substring(this.file.name.lastIndexOf('.')).toLowerCase();
              
                  if (!allowedExtensions.includes(fileExtension)) {
                    this.addFile.nativeElement.value = "";
                    this.postTerm.file = null;
                    this.postTerm.notice_Duration = 1;
                    this.postTerm.end_Date = new Date();
                    this.postTerm.Remark = "";
                    Alert.toast(TYPE.WARNING, true, "Unsupported file format. Allowed formats: .pdf, .doc, .docx ");
                    return;
                  }
                  if (this.file.size > 25 * 1048576) {
                    this.addFile.nativeElement.value = "";
                    this.postTerm.file = null;
                    Alert.toast(TYPE.WARNING, true, "File too large. Max 25MB allowed.");
                    return;
                  }
              
                  const formData = new FormData();
                  formData.append('file', this.file)
                  formData.append('contractId', String(this.contIdForPostTerm))
                  formData.append('notice_Duration', String(this.postTerm.notice_Duration))
                  formData.append('end_Date', String(this.postTerm.end_Date))
                  formData.append('Remark', String(this.postTerm.Remark))
                  this.postTermService.UploadClassifiedDoc(formData).subscribe({
                    next: (res) => {
                      this.file = null;
                      documentForm.reset();
                      // this.addFile.nativeElement.value = "";
                      //      this.postTerm.file=null;
              
                      Alert.bigToast(
                        'Success!',
                        'Posted Termination successfully.',
                        TYPE.SUCCESS,
                        'Ok'
                      );
                      this.GetPage(this.maxPage);
                    },
                    error: (error) => {
                      console.error('Error in creating Notice:', error);
                      Alert.bigToast(
                        'Error!',
                        'There was an error posting termination notice. ' + error.error.message,
                        TYPE.ERROR,
                        'Try Again'
                      );
                      // this.file = null;
                      // documentForm.reset();
                      // this.addFile.nativeElement.value = "";
                      // this.postTerm.file = null;
                      // this.document.status = 1;
                    },
                  });
                  // this.file = null;
                  // documentForm.reset();
                  // this.addFile.nativeElement.value = "";
                  // this.document.file = null;
                  // this.document.status = 1;
              
                }
                
                // Post Termination Notice
                postTermination: ClassifiedPostTermination = new ClassifiedPostTermination();
                statusTermOrReject?: number = 0;
                termStatus(status: number) {
                  if (status == 7) {
                    this.statusTermOrReject = 7;
                  }
                  if (status == 2) {
                    this.statusTermOrReject = 2;
                  }
                }
                postTerminationEmailForm = new FormGroup({
                  emailSubject: new FormControl('', [Validators.required]),
                  emailBody: new FormControl('', [Validators.required])
                })
                async approveTerminateContract(contractId?: string) {
                  if (this.postTerminationEmailForm.invalid) {
                    this.postTerminationEmailForm.markAllAsTouched();
                    return;
                  }
                  else {
                    this.loading = true;
                    const emailSubject = this.postTerminationEmailForm.value.emailSubject;
                    const emailBody = this.postTerminationEmailForm.value.emailBody;
                    console.log('ApproveTerminationClassifiedContract came here ')
                    let email = DecodeToken.email;
                    if (email) {
                      try {
                        this.postTermination.classifiedContractId = Number(contractId);
                        this.postTermination.changeToStatus = this.statusTermOrReject;
                        this.postTermination.emailSubject = emailSubject;
                        this.postTermination.emailBody = emailBody;
                        this.postTermination.employeeEmail = email;
                        // console.log(this.postTermination);
              
                        const response = await firstValueFrom(this.postTermService.ApproveTerminationClassifiedContract(this.postTermination))
                        if (response !== false) {
                          Alert.toast(TYPE.SUCCESS, true, 'Updated successfully');
                          this.GetAllContracts(1, 10);
                        }
                      }
                      catch (error) {
                        this.errorMsg = JSON.stringify(error);
                        Alert.toast(TYPE.ERROR, true, this.errorMsg);
                        console.error(error)
                      }
                      finally {
                        this.loading = false
                      }
                    }
                    else {
                      this.router.navigate(['/']);
                    }
                  }
                  this.loading = false;
                }
              
                //uploading the Withdrawal Notice 
                OnSaveWithdrawalNotice(documentForm: NgForm) {
                  console.log(documentForm.value);
                  console.log(this.file);
              
                  if (!this.file || !documentForm.valid) {
                    this.addFile.nativeElement.value = "";
                    this.withdrawNotice.file = null
                    this.withdrawNotice.Remark = "";
                    Alert.toast(TYPE.WARNING, true, "Please select a file and fill the Form Correctly");
                    return;
                  }
                  const allowedExtensions = ['.pdf', '.doc', '.docx'];
                  const fileExtension = this.file.name.substring(this.file.name.lastIndexOf('.')).toLowerCase();
              
                  if (!allowedExtensions.includes(fileExtension)) {
                    this.addFile.nativeElement.value = "";
                    this.withdrawNotice.file = null;
                    this.withdrawNotice.Remark = "";
                    Alert.toast(TYPE.WARNING, true, "Unsupported file format. Allowed formats: .pdf, .doc, .docx ");
                    return;
                  }
                  if (this.file.size > 25 * 1048576) {
                    this.addFile.nativeElement.value = "";
                    this.withdrawNotice.file = null;
                    Alert.toast(TYPE.WARNING, true, "File too large. Max 25MB allowed.");
                    return;
                  }
              
                  const formData = new FormData();
                  formData.append('file', this.file)
                  formData.append('contractId', String(this.contIdForPostTerm))
                  formData.append('postTermId', String(1))
                  formData.append('Remark', String(this.withdrawNotice.Remark))
                  this.noticeWithdrawalService.AddClassifiedWithdrawalNotice(formData).subscribe({
                    next: (res) => {
                      this.file = null;
                      documentForm.reset();
                      // this.addFile.nativeElement.value = "";
                      //      this.postTerm.file=null;
              
                      Alert.bigToast(
                        'Success!',
                        'Withdrawal Notice Added Successfully!',
                        TYPE.SUCCESS,
                        'Ok'
                      );
                      this.GetPage(this.maxPage);
                    },
                    error: (error) => {
                      console.error('Error in adding notice withdrawal:', error);
                      Alert.bigToast(
                        'Error!',
                        'There was an error adding notice withdrawal.',
                        TYPE.ERROR,
                        'Try Again'
                      );
                      // this.file = null;
                      // documentForm.reset();
                      // this.addFile.nativeElement.value = "";
                      // this.postTerm.file = null;
                      // this.document.status = 1;
                    },
                  });
                  // this.file = null;
                  // documentForm.reset();
                  // this.addFile.nativeElement.value = "";
                  // this.document.file = null;
                  // this.document.status = 1;
              
                }
              
                withdrawalNoticeEmailForm = new FormGroup({
                  emailSubject: new FormControl('', [Validators.required]),
                  emailBody: new FormControl('', [Validators.required])
                });
              
                withdrawNoticeSend:ClassifiedApproveRejectWithdrawalDTO = new ClassifiedApproveRejectWithdrawalDTO();
                async approveWithdrawalNotice(contractId?: string) {
                  if (this.withdrawalNoticeEmailForm.invalid) {
                    this.withdrawalNoticeEmailForm.markAllAsTouched();
                    return;
                  }
                  else {
                    this.loading = true;
                    const emailSubject = this.withdrawalNoticeEmailForm.value.emailSubject;
                    const emailBody = this.withdrawalNoticeEmailForm.value.emailBody;
                    console.log('approveWithdrawalNotice came here')
                    let email = DecodeToken.email;
                    if (email) {
                      try {
                        this.withdrawNoticeSend.classifiedContractId = Number(contractId);
                        this.withdrawNoticeSend.changeToStatus = this.statusTermOrReject;
                        this.withdrawNoticeSend.emailSubject = emailSubject;
                        this.withdrawNoticeSend.emailBody = emailBody;
                        this.withdrawNoticeSend.employeeEmail = email;
                        console.log(this.postTermination);
              
                        const response = await firstValueFrom(this.noticeWithdrawalService.ClassifiedApproveWithdrawalTermination(this.withdrawNoticeSend))
                        if (response !== false) {
                          Alert.toast(TYPE.SUCCESS, true, 'Updated successfully');
                          this.GetAllContracts(1, 10);
                        }
                      }
                      catch (error) {
                        this.errorMsg = JSON.stringify(error);
                        Alert.toast(TYPE.ERROR, true, this.errorMsg);
                        console.error(error)
                      }
                      finally {
                        this.loading = false
                      }
                    }
                    else {
                      this.router.navigate(['/']);
                    }
                  }
                  this.loading = false;
                }
              
                printToPDF(tableID: string, fileName: string) {
                  PDFExport.printToPDF(tableID, fileName);
                }

 getProgressType(status:number|undefined):string{
    if(status===undefined || status===null){
      return '';
    }

    switch(status){
      case 1: return 'Approval for'; 
      case 2: return 'Active'; 
      case 3: return 'Rejection for';
      case 4: return 'Termination of';
      case 5: return 'Expiration of';
      case 6: return 'Termination in progress for';
      case 7: return 'Termination approved for';
      case 8: return 'Notice withdrawal pending for';
      default: return 'Progress for ';
    }
  }

  phases=[
    'Classified Created',
    'L1 Approver Approval',
    'L2 Approver Approval',
    'L3 Approver Approval',
    'Classified Active',
  ]
}
