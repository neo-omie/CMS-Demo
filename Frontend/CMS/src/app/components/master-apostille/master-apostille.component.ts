declare var bootstrap: any;
import { Component, TemplateRef, ViewChild } from '@angular/core';
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
import { TableComponent } from "../UtilComponents/table/table.component";
import { PaginationComponent } from "../UtilComponents/pagination/pagination.component";
import { ErrorHandler } from '../../utils/errorHandler';
 
@Component({
  selector: 'app-master-apostille',
  standalone: true,
  imports: [LoaderComponent, CommonModule, ReactiveFormsModule, FormsModule, MatTableModule, MatSortModule, MatFormFieldModule, MatInputModule, TableComponent, PaginationComponent],
  templateUrl: './master-apostille.component.html',
  styleUrl: './master-apostille.component.css'
})
export class MasterApostilleComponent {
  loading = true;
  searchTerm: string = '';
  errorMsg?: string;
  apostilles: MasterApostille[] = [];
  totalApostilles: number = 0;
  totalPages: number = 0;
  currentPage: number = 1;
  pageSize: number = 2;
  maxPage: number = 1; //used now
  pageNumbers: number[] = []; //used now
  mode?: string;
  formsValue: any;
  empId: number = 0;
  columnsInfo: {
    [key: string]: {
      'title'?: string,
      'isSort'?: boolean,
      'templateRef': TemplateRef<any> | null,
    }
  } = {};
  addApostilleForm: FormGroup = new FormGroup({
    apostilleName: new FormControl('', [Validators.required, Validators.maxLength(30), Validators.pattern('^[a-zA-Z ]+$')]),
    status: new FormControl('', Validators.required)
  })
 
  @ViewChild('statusRef', { static: true }) statusRef!: TemplateRef<any>;
  @ViewChild('actionRef', { static: true }) actionRef!: TemplateRef<any>;
 
  constructor(private apostilleService: MasterApostilleService, private router: Router) { }
 
  ngOnInit(): void {
    this.columnsInfo = {
      'apostilleName': {
        'title': 'Apostille Name',
        'isSort': true,
        'templateRef': null
      },
      'status': {
        'title': 'Status',
        'isSort': true,
        'templateRef': this.statusRef
      },
      'action': {
        'title': 'Action',
        'templateRef': this.actionRef
      }
    };
    this.fetchApostille();
  }
 
  get apostilleName() {
    return this.addApostilleForm.get('apostilleName');
  }
 
  resetForm() {
    this.addApostilleForm.reset({
      status: ''
    });
    console.log(this.mode);
    this.mode = '';
  }
 
  fetchApostille() {
    this.apostilleService.getApostilles(this.currentPage, this.pageSize, this.searchTerm)
      .subscribe({
        next: (response: MasterApostilleDto) => {
          this.loading = false;
          this.apostilles = response.data;
          this.totalApostilles = response.totalCount;
          if (this.apostilles && this.totalApostilles > 0) {
            let result = Pagination.paginator(this.currentPage, this.totalApostilles, this.pageSize);
            this.maxPage = result.maxPage;
            console.log(this.maxPage);
            this.pageNumbers = result.pageNumbers;
          }
        },
        error: (err) => {
          this.loading = false;
          ErrorHandler.handle(err);
        }
      });
  }
 
  GetPage(pgNumber: number) {
    if (this.maxPage >= pgNumber && pgNumber >= 1) {
      this.currentPage = pgNumber;
      this.fetchApostille();
    }
  }
 
  onFilterChange() {
    this.currentPage = 1;
    this.fetchApostille();
  }
 
  deleteApostille(valueId: number) {
    let empCode = DecodeToken.ECode;
    Alert.confirmToast("Are you sure you want to delete this Apostille?",
      "You won't be able to revert this!", TYPE.WARNING,
      "Yes, delete it!",
      "Deleted successfully!",
      "Company has been deleted.", TYPE.SUCCESS, () => {
        this.apostilleService.deleteApostille(valueId, empCode).subscribe({
          next: (response: boolean) => {
            if (response) {
              Alert.toast(TYPE.SUCCESS, true, "Deleted successfully");
              if (this.apostilles.length === 1 && this.currentPage > 1) {
                this.currentPage = 1;
              }
              this.fetchApostille();
            }
          }
        });
      });
  }
 
  addApostille() {
    this.mode = 'add';
  }
 
  apoID: number = 0;
  editApostille(valueId: number) {
    this.apoID = valueId;
 
    this.mode = 'edit';
    if (valueId) {
      this.apostilleService.getApostilleById(valueId).subscribe({
        next: (apostilleData) => {
 
          this.addApostilleForm.patchValue({
            status: String(Number(apostilleData.status)),
            apostilleName: apostilleData.apostilleName
          });
          console.log('Fetched apostille for Edit:', apostilleData);
        },
        error: (error) => {
          this.loading = false;
         ErrorHandler.handle(error);
          this.router.navigate(['/masters/apostilleMasters']);
        }
      });
    } else {
      console.error('Invalid valueId:', valueId);
      Alert.toast(TYPE.ERROR, true, 'Invalid employee ID.');
    }
  }
 
  displayedColumns: string[] = ['apostilleName', 'status', 'action'];
  onSubmit() {
 
    this.formsValue = this.addApostilleForm.value;
    if (this.addApostilleForm.invalid) {
      this.addApostilleForm.markAllAsTouched();
      return;
    }
 
    const formValues = this.addApostilleForm.value;
    if (this.mode === 'add') {
      let empCode = DecodeToken.ECode;
      const apostilleName = this.addApostilleForm.value.apostilleName;
      const status = this.addApostilleForm.value.status;
      const addFormValues: AddApostilleDto = new AddApostilleDto();
      addFormValues.apostilleName = this.addApostilleForm.value.apostilleName;
      addFormValues.status = Number(status) == 1 ? true : false;
      console.log(addFormValues);
      this.apostilleService.addApostille(addFormValues, empCode).subscribe({
        next: (response: AddApostilleDto) => {
          Alert.toast(TYPE.SUCCESS, true, 'Added successfully');
          this.resetForm()
          this.fetchApostille();
          const modalElement = document.getElementById('apostille-add');
          if (modalElement) {
            const modalInstance = bootstrap.Modal.getInstance(modalElement) || new bootstrap.Modal(modalElement);
            modalInstance.hide();
          }
          this.router.navigate(['masters/apostilleMasters']);
        },
        error: (error) => {
          this.loading = false;
         ErrorHandler.handle(error);
        }
      });
    }
    else if (this.mode === 'edit') {
      let empCode = DecodeToken.ECode;
      console.log(this.addApostilleForm);
      const apostilleName = this.addApostilleForm.value.apostilleName;
      const status = this.addApostilleForm.value.status;
      const addFormValues: EditApostilleDto = new EditApostilleDto();
      addFormValues.apostilleName = this.addApostilleForm.value.apostilleName;
      addFormValues.status = Number(status) == 1 ? true : false;
      console.log(addFormValues);
      this.apostilleService.updateApostille(this.apoID, addFormValues, empCode).subscribe({
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
          this.loading = false;
         ErrorHandler.handle(error);
        }
      })
    }
  }
 
  get status() {
    return this.addApostilleForm.get('status');
  }
 
}