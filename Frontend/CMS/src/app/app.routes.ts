import { Routes } from '@angular/router';
import { LoginScreenComponent } from './components/auth/login-screen/login-screen.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { MastersScreenComponent } from './components/masters-screen/masters-screen.component';
import { ApprovalMatrixContractScreenComponent } from './components/approval-matrix-contract-screen/approval-matrix-contract-screen.component';
import { MasterDocumentComponent } from './components/master-document/master-document.component';
import { RenewPasswordComponent } from './components/auth/renew-password/renew-password.component';
import { NotFoundComponent } from './components/misc/not-found/not-found.component';
import { ApprovalMatrixMouScreenComponent } from './components/approval-matrix-mou-screen/approval-matrix-mou-screen.component';
import { EscalationMatrixContractComponent } from './components/escalation-matrix-contract/escalation-matrix-contract.component';
import { MasterCompanyAddFormComponent } from './components/master-company-add-form/master-company-add-form.component';
import { MasterDepartmentComponent } from './components/master-department/master-department.component';
import { authGuard } from './auth.guard';
import { MasterEmployeeComponent } from './components/master-employee/master-employee.component';
import { AddEmployeeComponent } from './components/add-employee/add-employee.component';
import { EditEmployeeComponent } from './components/edit-employee/edit-employee.component';
import { ViewEmployeeComponent } from './components/view-employee/view-employee.component';

export const routes: Routes = [
    {path: '', component: LoginScreenComponent}, 
    {path: 'dashboard', component: DashboardComponent, canActivate:[authGuard]},
    {path: 'masters', component: MastersScreenComponent, canActivate:[authGuard]}, 
    {path: 'masters/approval-matrix-contract', component: ApprovalMatrixContractScreenComponent, canActivate:[authGuard]},
    {path: 'masters/approval-matrix-mou', component: ApprovalMatrixMouScreenComponent, canActivate:[authGuard]},
    {path: 'masters/documentMasters', component: MasterDocumentComponent, canActivate:[authGuard]},
    {path:'masters/employeeMasters', component:MasterEmployeeComponent, canActivate:[authGuard]},
    {path:'masters/employeeMasters/addEmpoyee', component:AddEmployeeComponent, canActivate:[authGuard]},
    // {path:'masters/employeeMasters/editEmpoyee', component:EditEmployeeComponent, canActivate:[authGuard]},
    // {path:'masters/employeeMasters/viewEmpoyee', component:ViewEmployeeComponent, canActivate:[authGuard]},
    {path: 'masters/departmentMasters', component: MasterDepartmentComponent, canActivate:[authGuard]},
    {path: 'masters/escalationContracts', component: EscalationMatrixContractComponent, canActivate:[authGuard]},
    {path: 'masters/companyMasters/addCompany', component: MasterCompanyAddFormComponent, canActivate:[authGuard]},
    {path: 'auth/renewPassword', component: RenewPasswordComponent},
    {path: '**', component: NotFoundComponent}

];
