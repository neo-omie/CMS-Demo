declare var bootstrap: any;
import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import {
  AddEmployeeDto,
  EditEmployeeDto,
  MasterEmployee,
  MasterEmployeeDto,
} from '../../models/master-employee';
import { MasterEmployeeService } from '../../services/master-employee.service';
import { Pagination } from '../../utils/pagination';
import { Alert } from '../../utils/alert';
import { TYPE } from '../auth/login/values.constants';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { MasterDepartmentService } from '../../services/master-department.service';
import { MasterDepartment } from '../../models/master-department';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { LoaderComponent } from '../UtilComponents/loader/loader.component';
import { DecodeToken } from '../../utils/decodeToken';
import { TableComponent } from '../UtilComponents/table/table.component';
import { PaginationComponent } from '../UtilComponents/pagination/pagination.component';
import { ErrorHandler } from '../../utils/errorHandler';
import { Title } from '@angular/platform-browser';
@Component({
  selector: 'app-master-employee',
  standalone: true,
  imports: [
    CommonModule,
    LoaderComponent,
    FormsModule,
    RouterModule,
    ReactiveFormsModule,
    MatTableModule,
    MatSortModule,
    MatFormFieldModule,
    MatInputModule,
    TableComponent,
    PaginationComponent,
  ],
  templateUrl: './master-employee.component.html',
  styleUrl: './master-employee.component.css',
})
export class MasterEmployeeComponent implements OnInit {
  loading = true;
  columnsInfo: {
    [key: string]: {
      title?: string;
      isSort?: boolean;
      templateRef: TemplateRef<any> | null;
    };
  } = {};
  employees: MasterEmployee[] = [];
  totalEmployees: number = 0;
  totalPages: number = 0;
  currentPage: number = 1;
  pageSize: number = 10;
  maxPage: number = 1; //used now
  pageNumbers: number[] = []; //used now
  selectedUnit: string = 'All';
  searchTerm: string = '';
  errorMsg?: string;
  formsValue: any;
  departments: MasterDepartment[] = [];
  mode?: string;
  valueId?: number;

  @ViewChild('actionRef', { static: true }) actionRef!: TemplateRef<any>;
  // constructor(private router: Router){}

constructor(
  private employeeService: MasterEmployeeService,
  private route:ActivatedRoute,
  private router:Router, 
  private departmentService:MasterDepartmentService,
  private title:Title
){this.title.setTitle("Employee Type Master - CMS");}

  ngOnInit(): void {
    this.columnsInfo = {
      employeeCode: {
        title: 'Employee Code',
        isSort: true,
        templateRef: null,
      },
      employeeName: {
        title: 'Employee Name',
        isSort: true,
        templateRef: null,
      },
      unit: {
        title: 'Employee Location',
        isSort: true,
        templateRef: null,
      },
      action: {
        title: 'Action',
        templateRef: this.actionRef,
      },
    };
    this.fetchEmployees();
    this.getDepartmentName();
    this.route.params.subscribe((params) => {
      console.log('Route Params:', params);
      const paramValueId = params['valueId'];
      if (paramValueId) {
        this.valueId = +paramValueId;
        console.log('Dynamic valueId:', this.valueId);
        console.log(this.mode);
      }
    });
  }

  resetForm() {
    this.addEmployeeForm.reset({
      role: '',
      unit: '',
      departmentId: '',
    });
    console.log(this.mode);
    this.mode = '';
  }

  addEmployeeForm: FormGroup = new FormGroup({
    employeeName: new FormControl('', [
      Validators.required,
      Validators.maxLength(40),
      Validators.pattern('^[a-zA-Z ]+$'),
    ]),
    password: new FormControl('', [Validators.required]),
    role: new FormControl('', Validators.required),
    employeeCode: new FormControl('', [
      Validators.required,
      Validators.pattern('^[A-Z0-9]+$'),
      Validators.maxLength(8),
    ]),
    unit: new FormControl('', Validators.required),
    departmentId: new FormControl('', Validators.required),
    departmentName: new FormControl(''),
    employeeMobile: new FormControl('', [
      Validators.required,
      Validators.pattern('^[0-9]{10}$'),
    ]),
    email: new FormControl('', [Validators.required, Validators.email]),
    employeeExtension: new FormControl('', Validators.required),
  });

  fetchEmployees() {
    this.employeeService
      .getEmployees(
        this.currentPage,
        this.pageSize,
        this?.selectedUnit,
        this?.searchTerm
      )
      .subscribe({
        next: (response: MasterEmployeeDto) => {
          this.loading = false;
          console.log(response);
          this.dataSource.data = response.data;
          this.employees = response.data;
          this.totalEmployees = response.totalCount;
          if (this.employees != undefined && this.employees.length > 0) {
            let result = Pagination.paginator(
              this.currentPage,
              this.totalEmployees,
              this.pageSize
            );
            this.maxPage = result.maxPage;
            console.log(this.maxPage);
            this.pageNumbers = result.pageNumbers;
          }
        },
        error: (error) => {
          this.loading = false;
          ErrorHandler.handle(error);
        },
      });
  }

  GetPage(pgNumber: number) {
    if (this.maxPage >= pgNumber && pgNumber >= 1) {
      this.currentPage = pgNumber;
      this.fetchEmployees();
    }
  }

  onFilterChange() {
    this.currentPage = 1;
    this.fetchEmployees();
  }

  getPageNumbers(): number[] {
    const pageNumbers = [];
    for (let i = 1; i <= this.totalPages; i++) {
      pageNumbers.push(i);
    }
    return pageNumbers;
  }

  deleteEmployee(employee: MasterEmployee) {
    const loggedInEmpCode = DecodeToken.ECode;
    Alert.confirmToast(
      'Are you sure you want to delete this Employee?',
      "You won't be able to revert this!",
      TYPE.WARNING,
      'Yes, delete it!',
      'Deleted successfully!',
      'Company has been deleted.',
      TYPE.SUCCESS,
      () => {
        this.employeeService
          .deleteEmployee(employee.valueId, loggedInEmpCode)
          .subscribe({
            next: (response: boolean) => {
              if (response) {
                Alert.toast(TYPE.SUCCESS, true, 'Deleted successfully');
                this.fetchEmployees();
              }
            },
          });
      }
    );
  }

  addEmployee() {
    const formValues = this.addEmployeeForm.value;
    this.mode = 'add';
  }

  viewEmployee(valueId?: number) {
    if (valueId !== undefined) {
      this.employeeService.getEmployeeById(valueId).subscribe({
        next: (employee) => {
          this.addEmployeeForm.patchValue(employee);
          console.log('Fetched Employee:', employee);
          this.fetchEmployees();
        },
        error: (error) => {
          this.loading = false;
          ErrorHandler.handle(error);
          this.router.navigate(['/masters/employeeMasters']);
        },
      });
    } else {
      console.error('Invalid valueId:', valueId);
      Alert.toast(TYPE.ERROR, true, 'Invalid employee ID.');
    }
  }

  empId: number = 0;
  editEmployee(employee: MasterEmployee) {
    this.empId = employee.valueId;
    console.log(this.empId);
    const formValues = this.addEmployeeForm.value;
    this.mode = 'edit';
    if (employee.valueId) {
      this.employeeService.getEmployeeById(employee.valueId).subscribe({
        next: (employeeData) => {
          this.addEmployeeForm.patchValue(employeeData);
          console.log('Fetched Employee for Edit:', employeeData);
        },
        error: (error) => {
          this.loading = false;
          ErrorHandler.handle(error);

          this.router.navigate(['/masters/employeeMasters']);
        },
      });
    } else {
      console.error('Invalid valueId:', employee.valueId);
      Alert.toast(TYPE.ERROR, true, 'Invalid employee ID.');
    }
  }

  getDepartmentName() {
    const eCode = DecodeToken.ECode;
    if (eCode) {
      this.departmentService
        .getAllDepartments(1, 100, eCode)
        .subscribe((res) => {
          this.departments = res;
          console.log(this.departments);
        });
    }
  }

  onSubmit() {
    this.formsValue = this.addEmployeeForm.value;
    if (this.addEmployeeForm.invalid) {
      console.log('invalie emp form : ', this.formsValue);
      this.addEmployeeForm.markAllAsTouched();
      return;
    }

    const formValues = this.addEmployeeForm.value;
    if (this.mode === 'add') {
      const loggedInEmpCode = DecodeToken.ECode;

      const employeeName = this.addEmployeeForm.value.employeeName;
      const password = this.addEmployeeForm.value.password;
      const role = this.addEmployeeForm.value.role;
      const employeeCode = this.addEmployeeForm.value.employeeCode;
      const unit = this.addEmployeeForm.value.unit;
      const departmentId = this.addEmployeeForm.value.departmentId;
      const employeeMobile = this.addEmployeeForm.value.employeeMobile;
      const email = this.addEmployeeForm.value.email;
      const employeeExtension = this.addEmployeeForm.value.employeeExtension;
      if (employeeMobile && Number(employeeMobile) && Number(departmentId)) {
        const addFormValues: AddEmployeeDto = new AddEmployeeDto();
        addFormValues.employeeName = this.addEmployeeForm.value.employeeName;
        addFormValues.password = this.addEmployeeForm.value.password;
        addFormValues.role = this.addEmployeeForm.value.role;
        addFormValues.employeeCode = this.addEmployeeForm.value.employeeCode;
        addFormValues.unit = this.addEmployeeForm.value.unit;
        addFormValues.departmentId = Number(departmentId);
        addFormValues.employeeMobile = Number(employeeMobile);

        addFormValues.email = this.addEmployeeForm.value.email;
        addFormValues.employeeExtension =
          this.addEmployeeForm.value.employeeExtension;
        console.log(addFormValues);
        this.employeeService
          .addEmployee(addFormValues, loggedInEmpCode)
          .subscribe({
            next: (response: AddEmployeeDto) => {
              Alert.toast(TYPE.SUCCESS, true, 'Added successfully');
              const modalElement = document.getElementById('employee-add');
              if (modalElement) {
                const modalInstance =
                  bootstrap.Modal.getInstance(modalElement) ||
                  new bootstrap.Modal(modalElement);
                modalInstance.hide();
              }
              this.resetForm();
              this.router.navigate(['masters/employeeMasters']);
            },
            error: (error) => {
              this.loading = false;
              ErrorHandler.handle(error);
            },
          });
      } else {
        console.log('should not come here ', this.addEmployeeForm.value);
      }
    } else if (this.mode === 'edit') {
      const employeeName = this.addEmployeeForm.value.employeeName;
      const password = this.addEmployeeForm.value.password;
      const role = this.addEmployeeForm.value.role;
      const employeeCode = this.addEmployeeForm.value.employeeCode;
      const unit = this.addEmployeeForm.value.unit;
      const departmentId = this.addEmployeeForm.value.departmentId;
      const employeeMobile = this.addEmployeeForm.value.employeeMobile;
      const email = this.addEmployeeForm.value.email;
      const employeeExtension = this.addEmployeeForm.value.employeeExtension;
      if (employeeMobile && Number(employeeMobile) && Number(departmentId)) {
        const addFormValues: EditEmployeeDto = new EditEmployeeDto();
        addFormValues.employeeName = this.addEmployeeForm.value.employeeName;
        addFormValues.password = this.addEmployeeForm.value.password;
        addFormValues.role = this.addEmployeeForm.value.role;
        addFormValues.employeeCode = this.addEmployeeForm.value.employeeCode;
        addFormValues.unit = this.addEmployeeForm.value.unit;
        addFormValues.departmentId = Number(departmentId);
        addFormValues.employeeMobile = Number(employeeMobile);

        addFormValues.email = this.addEmployeeForm.value.email;
        addFormValues.employeeExtension =
          this.addEmployeeForm.value.employeeExtension;
        //addFormValues.loggedBy =this.addEmployeeForm.value.loggedBy;

        const loggedInEmpCode = DecodeToken.ECode;
        if (!loggedInEmpCode) {
          console.log('LoggedInUser EmpCode is not found in sessionStorage');
          Alert.toast(
            TYPE.ERROR,
            true,
            'Unable to retrieve logged-in user information.'
          );
          return;
        }
        addFormValues.loggedBy = loggedInEmpCode;
        console.log(addFormValues);

        if (!this.empId) {
          console.error('valueId is undefined. Cannot update employee.');
          Alert.toast(TYPE.ERROR, true, 'Invalid employee ID.');
          return;
        }
        this.employeeService
          .updateEmployee(this.empId, addFormValues, loggedInEmpCode)
          .subscribe({
            next: (response: EditEmployeeDto) => {
              Alert.toast(TYPE.SUCCESS, true, 'Updated successfully');
              this.router.navigate(['masters/employeeMasters']);
            },
            error: (error) => {
              this.loading = false;
              ErrorHandler.handle(error);
            },
          });
      } else {
        console.log('should not come here ', this.addEmployeeForm.value);
      }
    }
  }

  get employeeName() {
    return this.addEmployeeForm.get('employeeName');
  }
  get password() {
    return this.addEmployeeForm.get('password');
  }
  get role() {
    return this.addEmployeeForm.get('role');
  }
  get employeeCode() {
    return this.addEmployeeForm.get('employeeCode');
  }
  get unit() {
    return this.addEmployeeForm.get('unit');
  }
  get departmentId() {
    return this.addEmployeeForm.get('departmentId');
  }
  get departmentName() {
    return this.addEmployeeForm.get('departmentName');
  }
  get employeeMobile() {
    return this.addEmployeeForm.get('employeeMobile');
  }
  get email() {
    return this.addEmployeeForm.get('email');
  }
  get employeeExtension() {
    return this.addEmployeeForm.get('employeeExtension');
  }

  // sakthish table ts

  displayedColumns: string[] = [
    'employeeCode',
    'employeeName',
    'unit',
    'action',
  ];
  dataSource = new MatTableDataSource<MasterEmployee>();
  @ViewChild(MatSort) sort!: MatSort;
  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
  }
}
