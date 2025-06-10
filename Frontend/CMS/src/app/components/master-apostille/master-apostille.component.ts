declare var bootstrap: any;
import { Component, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AddApostilleDto, EditApostilleDto, MasterApostille, MasterApostilleDto } from '../../models/master-apostille';
import { MasterApostilleService } from '../../services/master-apostille.service';
import { Router } from '@angular/router';
import { Alert } from '../../utils/alert';
import { TYPE } from '../auth/login/values.constants';
import { Pagination } from '../../utils/pagination';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { LoaderComponent } from '../UtilComponents/loader/loader.component';
import { DecodeToken } from '../../utils/decodeToken';

@Component({
  selector: 'app-master-apostille',
  standalone: true,
  imports: [LoaderComponent,CommonModule,ReactiveFormsModule,FormsModule,MatTableModule,MatSortModule,MatFormFieldModule,MatInputModule],
  templateUrl: './master-apostille.component.html',
  styleUrl: './master-apostille.component.css'
})
export class MasterApostilleComponent {
loading=true;
searchTerm: string = '';
errorMsg?:string;
apostilles:MasterApostille[]=[];
totalApostilles:number=0;
totalPages:number=0;
currentPage:number=1;
pageSize:number=2;
maxPage:number=1; //used now 
pageNumbers:number[] = []; //used now
mode?:string; 
formsValue:any;
empId:number = 0;
addApostilleForm: FormGroup= new FormGroup({
  apostilleName:new FormControl('',[Validators.required,Validators.maxLength(30),Validators.pattern('^[a-zA-Z ]+$')]),
  status:new FormControl('',Validators.required)
})

constructor(private apostilleService: MasterApostilleService,private router: Router){}

ngOnInit(): void {
  this.fetchApostille();
}

get apostilleName(){
  return this.addApostilleForm.get('apostilleName');
}

resetForm() {
  this.addApostilleForm.reset({
    status:''
  });
  console.log(this.mode);
  this.mode='';
}

fetchApostille(){
  this.apostilleService.getApostilles(this.currentPage,this.pageSize,this.searchTerm)
  .subscribe({
    next:(response:MasterApostilleDto)=>{
      this.loading=false;
      console.log(response);
      this.dataSource.data = response.data;
      this.apostilles = response.data;
      this.totalApostilles=response.totalCount;
      console.log(this.totalApostilles); 
      if(this.apostilles && this.totalApostilles > 0){
        let result=Pagination.paginator(this.currentPage,this.totalApostilles,this.pageSize);
        this.maxPage = result.maxPage;
        console.log(this.maxPage);
        this.pageNumbers = result.pageNumbers;
      }
    },
    error:(error)=>{
      this.loading = false;
        console.error('Error :(', error);
        this.errorMsg = JSON.stringify((error.message !== undefined)?error.error.title: error.message);
        Alert.toast(TYPE.ERROR,true,this.errorMsg);
    }
  });
}

GetPage(pgNumber:number){
  if(this.maxPage >= pgNumber && pgNumber >= 1){
    this.currentPage=pgNumber;
    this.fetchApostille();
  }
}

getPageNumbers():number[]{
  const pageNumbers=[];
  for(let i=1; i<=this.totalPages;i++){
    pageNumbers.push(i)
  }
  return pageNumbers
}

onFilterChange(){
  this.currentPage=1;
  this.fetchApostille();
}

deleteApostille(apostille:MasterApostille){
  let empCode =DecodeToken.ECode;
  Alert.confirmToast("Are you sure you want to delete this Apostille?",
    "You won't be able to revert this!", TYPE.WARNING,
    "Yes, delete it!",
    "Deleted successfully!",
    "Company has been deleted.", TYPE.SUCCESS,() => {
      this.apostilleService.deleteApostille(apostille.valueId,empCode).subscribe({
        next:(response:boolean)=>{
          if(response){
            // Alert.toast(TYPE.SUCCESS,true,"Deleted successfully");
            if (this.apostilles.length === 1 && this.currentPage > 1) {
              this.currentPage = 1; 
            }
            this.fetchApostille();
          }
        }
      });
    });
}
  
addApostille(){
  this.mode='add';
}
viewApostille(valueId:number){
  if (valueId !== undefined) {
    this.apostilleService.getApostilleById(valueId).subscribe({
      next: (apostille) => {
        this.addApostilleForm.patchValue(apostille); 
        this.apoID = apostille.valueId;
        console.log('Fetched Apostille:', apostille);
        this.fetchApostille();
      },
      error: (error) => {
        console.error('Error fetching apostille data:', error);
        Alert.toast(TYPE.ERROR, true, 'Failed to load apostille data.');
        this.router.navigate(['/masters/apostilleMasters']);
      }
    });
  } else {
    console.error('Invalid valueId:', valueId);
    Alert.toast(TYPE.ERROR, true, 'Invalid employee ID.');
  }
}

apoID:number = 0;
editApostille(apostille:MasterApostille){
  this.apoID = apostille.valueId;

  this.mode = 'edit';
  if(apostille.valueId){
    this.apostilleService.getApostilleById(apostille.valueId).subscribe({
      next: (apostilleData) => {
        
        this.addApostilleForm.patchValue({
          status: String(Number(apostilleData.status)),
          apostilleName: apostilleData.apostilleName
        });
        console.log('Fetched apostille for Edit:', apostilleData);
      },
      error: (error) => {
        console.error('Error fetching apostille data:', error);
        Alert.toast(TYPE.ERROR, true, 'Failed to load apostille data.');
        this.router.navigate(['/masters/apostilleMasters']);
      }
    });
  } else {
    console.error('Invalid valueId:', apostille.valueId);
    Alert.toast(TYPE.ERROR, true, 'Invalid employee ID.');
  }
}

displayedColumns: string[] = ['apostilleName', 'status', 'action'];
  dataSource = new MatTableDataSource<MasterApostille>();
  @ViewChild(MatSort) sort!: MatSort;
  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
}

onSubmit(){

  this.formsValue=this.addApostilleForm.value;
  if(this.addApostilleForm.invalid){
        this.addApostilleForm.markAllAsTouched();
        return;
  }

  const formValues=this.addApostilleForm.value;
  if(this.mode==='add'){
    let empCode =DecodeToken.ECode;
    const apostilleName = this.addApostilleForm.value.apostilleName;
    const status = this.addApostilleForm.value.status;
      const addFormValues:AddApostilleDto = new AddApostilleDto();
      addFormValues.apostilleName = this.addApostilleForm.value.apostilleName;
      addFormValues.status = Number(status) == 1 ? true : false;
      console.log(addFormValues);
      this.apostilleService.addApostille(addFormValues,empCode).subscribe({
        next:(response:AddApostilleDto) => {
            Alert.toast(TYPE.SUCCESS,true,'Added successfully');
            this.resetForm()
            this.fetchApostille();
            const modalElement = document.getElementById('apostille-add');
                if (modalElement) {
                  const modalInstance = bootstrap.Modal.getInstance(modalElement) || new bootstrap.Modal(modalElement);
                  modalInstance.hide();
                }
            this.router.navigate(['masters/apostilleMasters']);
        }, 
        error:(error) => {
          console.error('Error :(', error);
          this.errorMsg = JSON.stringify((error.message !== undefined)?error.error.title: error.message);
          Alert.toast(TYPE.ERROR,true,this.errorMsg);
        }
      });
  }
  else if(this.mode === 'edit'){
    let empCode =DecodeToken.ECode;
    console.log(this.addApostilleForm);
    const apostilleName = this.addApostilleForm.value.apostilleName;
    const status = this.addApostilleForm.value.status;
      const addFormValues:EditApostilleDto = new EditApostilleDto();
      addFormValues.apostilleName = this.addApostilleForm.value.apostilleName;
      addFormValues.status = Number(status) == 1 ? true : false;
      console.log(addFormValues);
      this.apostilleService.updateApostille(this.apoID, addFormValues,empCode).subscribe({
        next: (res: EditApostilleDto) => {
          Alert.toast(TYPE.SUCCESS, true, 'Updated Successfully')
          this.resetForm()
           const modalElement = document.getElementById('apostille-add');
                if (modalElement) {
                  const modalInstance = bootstrap.Modal.getInstance(modalElement) || new bootstrap.Modal(modalElement);
                  modalInstance.hide();
                }
          this.fetchApostille();
        }, error: (error) => {
          Alert.toast(TYPE.ERROR, true, error.error.message);
          console.error(error.error);
        }
      })
  }
}

 get status(){
      return this.addApostilleForm.get('status');
    }

}
