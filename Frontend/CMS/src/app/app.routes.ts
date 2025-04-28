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
import { MasterEmployeeComponent } from './components/master-employee/master-employee.component';
import { AddEmployeeComponent } from './components/add-employee/add-employee.component';
import { ViewEmployeeComponent } from './components/view-employee/view-employee.component';
import { EditEmployeeComponent } from './components/edit-employee/edit-employee.component';

export const routes: Routes = [
    {path: '', component: LoginScreenComponent}, 
    {path: 'dashboard', component: DashboardComponent},
    {path: 'masters', component: MastersScreenComponent}, 
    {path: 'masters/approval-matrix-contract', component: ApprovalMatrixContractScreenComponent},
    {path: 'masters/approval-matrix-mou', component: ApprovalMatrixMouScreenComponent},
    {path: 'masters/documentMasters', component:MasterDocumentComponent},
    {path: 'masters/escalationContracts', component:EscalationMatrixContractComponent},
    {path: 'masters/employeeMasters', component:MasterEmployeeComponent},
    {path:'employeeMaster/add', component:AddEmployeeComponent},
    {path:'employeeMaster/edit', component:EditEmployeeComponent},
    {path:'employeeMaster/view', component:ViewEmployeeComponent},
    {path: 'auth/renewPassword', component:RenewPasswordComponent},
    {path: '404NotFound', component: NotFoundComponent}

];
