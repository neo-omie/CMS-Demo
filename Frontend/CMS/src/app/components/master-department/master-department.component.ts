declare var bootstrap: any;
import { CommonModule } from '@angular/common';
import { Component, ElementRef, Renderer2, ViewChild } from '@angular/core';
import { AddDepartmentDto, GetAllDepartmentsDto, MasterDepartment } from '../../models/master-department';
import { MasterDepartmentService } from '../../services/master-department.service';
import { Pagination } from '../../utils/pagination';
import { Alert } from '../../utils/alert';
import { TYPE } from '../auth/login/values.constants';
import { FormsModule, NgForm } from '@angular/forms';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { LoaderComponent } from '../UtilComponents/loader/loader.component';
import { DecodeToken } from '../../utils/decodeToken';

@Component({
  selector: 'app-master-department',
  standalone: true,
  imports: [CommonModule, LoaderComponent, FormsModule, MatTableModule, MatSortModule, MatFormFieldModule, MatInputModule],
  templateUrl: './master-department.component.html',
  styleUrl: './master-department.component.css'
})
export class MasterDepartmentComponent {
  loading: boolean = true
  displayedColumns: string[] = ['departmentName', 'action'];
    dataSource = new MatTableDataSource<GetAllDepartmentsDto>();
    @ViewChild(MatSort) sort!: MatSort;
    @ViewChild('departmentEditModal') departmentEditModal!: ElementRef;
    ngAfterViewInit() {
      this.dataSource.sort = this.sort;
    }
  pageNumbers: number[] = [];
  maxPage: number = 1;
  departments: GetAllDepartmentsDto[] = [];
  dept?: MasterDepartment;
  errorMsg?: string;
  @ViewChild('editDepartmentName') editDepartmentName!: ElementRef;
  constructor(private departmentService: MasterDepartmentService, private renderer: Renderer2) { }
  ngOnInit() {
    this.GetAllDepartments(1, 10);
  }
  closeEditApproverCollapses() {
    // this.renderer.removeClass(this.editDepartmentName.nativeElement, 'show');
    if (this.dept!=undefined) {
      this.dept.departmentName="";
    }

  }

  resetFrom(){
    this.addDept.departmentName="";
  }
  GetAllDepartments(pageNumber: number, pageSize: number) {
    this.departmentService.getAllDepartments(pageNumber, pageSize).subscribe({
      next: (response: GetAllDepartmentsDto[]) => {
        this.loading = false;
        this.dataSource.data = response;
          if (this.sort) {
            this.dataSource.sort = this.sort;
          }
        this.departments = response;
        if (this.departments != undefined && this.departments.length > 0) {
          let result = Pagination.paginator(pageNumber, this.departments[0].totalRecords, pageSize)
          this.maxPage = result.maxPage;
          this.pageNumbers = result.pageNumbers;
        }
      },
      error: (error) => {
        this.loading = false;
        console.error('Error :(', error);
         if (error.status == 401) {
            let errmsg = error.error;
            Alert.toast(TYPE.ERROR, true, errmsg);
          }
          else{

            this.errorMsg = JSON.stringify((error.message !== undefined) ? error.error.title : error.message);
            Alert.toast(TYPE.ERROR, true, this.errorMsg);
          }
      }
    });
  }
  GetPage(pgNumber: number) {
    if (this.maxPage >= pgNumber && pgNumber >= 1) {
      this.GetAllDepartments(pgNumber, 10);
    }
  }
  GetDepartment(id: number) {
    this.departmentService.getDepartmentById(id).subscribe({
      next: (response: MasterDepartment) => {
        this.dept = response;
        console.log(this.dept.departmentId);
        console.log(this.dept.departmentName);

      },
      error: (error) => {
        console.error('Error :(', error);
         if (error.status == 401) {
                    let errmsg = error.error;
                    Alert.toast(TYPE.ERROR, true, errmsg);
                  }
                  else{

                    this.errorMsg = JSON.stringify((error.message !== undefined) ? error.error.title : error.message);
                    Alert.toast(TYPE.ERROR, true, this.errorMsg);
                  }
      }
    });
  }
  editDepartment(id: number) {
    let empCode = DecodeToken.ECode;
    let deptName = this.editDepartmentName.nativeElement.value;
    if (deptName !== "") {
      console.log(deptName);
      this.departmentService.updateDepartment(id, deptName,empCode).subscribe({
        next: (response: boolean) => {
          if (response) {
            Alert.toast(TYPE.SUCCESS, true, "Updated successfully");
            this.GetAllDepartments(1, 10);
            const modalElement = this.departmentEditModal.nativeElement;
          const modalInstance = bootstrap.Modal.getInstance(modalElement) || new bootstrap.Modal(modalElement);
          modalInstance.hide();
          }

        },
        error: (error) => {
          console.error('Error :(', error);
           if (error.status == 401) {
            let errmsg = error.error;
            Alert.toast(TYPE.ERROR, true, errmsg);
          }
          else{
            this.errorMsg = JSON.stringify((error.message !== undefined) ? error.error.title : error.message);
            Alert.toast(TYPE.ERROR, true, this.errorMsg);
          }
        }
      });
    }
    else {
      Alert.toast(TYPE.ERROR, true, "Please enter the department name.");
    }
    this.closeEditApproverCollapses();
  }

 
  deleteDepartment(id: number) {
    let empCode = DecodeToken.ECode;
    // let askFirst:boolean = confirm("Are you sure you want to delete this department?");
    Alert.confirmToast("Are you sure you want to delete this department?",
      "You won't be able to revert this!", TYPE.WARNING,
      "Yes, delete it!",
      "Deleted successfully!",
      "Department has been deleted.", TYPE.SUCCESS, () => {
        this.departmentService.deleteDepartment(id,empCode).subscribe({
          next: (response: boolean) => {
            if (response) {
              // Alert.toast(TYPE.SUCCESS, true, "Deleted successfully");
              this.GetAllDepartments(1, 10);
            }

          },
          error: (error) => {
            console.error('Error :(', error);
             if (error.status == 401) {
            let errmsg = error.error;
            Alert.toast(TYPE.ERROR, true, errmsg);
          }
          else{

            this.errorMsg = JSON.stringify((error.message !== undefined) ? error.error.title : error.message);
            Alert.toast(TYPE.ERROR, true, this.errorMsg);
          }
          }
        });
      });
  }

  
  addDept: AddDepartmentDto = new AddDepartmentDto('');
  addDepartment(departmentForm: NgForm) {
    let empCode = DecodeToken.ECode;
    //form is invalid , then exiting early
    if (departmentForm.invalid) {
    return;
  }
    this.addDept = departmentForm.value;
    console.log(departmentForm);

    this.departmentService.addDepartment(this.addDept.departmentName,empCode).subscribe({
      next: (response) => {
        Alert.toast(TYPE.SUCCESS,true,'Added successfully');
        departmentForm.resetForm();
        this.GetPage(this.maxPage);
         const modalElement = document.getElementById('department-add');
        if (modalElement) {
          const modalInstance = bootstrap.Modal.getInstance(modalElement) || new bootstrap.Modal(modalElement);
          modalInstance.hide();
        }
      },
      error: (error) => {
        console.error('Error adding Department:', error);
         if (error.status == 401) {
            let errmsg = error.error;
            Alert.toast(TYPE.ERROR, true, errmsg);
          }
          else{

            Alert.bigToast('Error!', 'There was an error adding the Department.', TYPE.ERROR, 'Try Again.');
          }
      }
    });
  }
}
