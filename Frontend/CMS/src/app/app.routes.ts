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
import { MasterCompanyComponent } from './components/master-company/master-company.component';

export const routes: Routes = [
    {path: '', component: LoginScreenComponent}, 
    {path: 'dashboard', component: DashboardComponent},
    {path: 'masters', component: MastersScreenComponent}, 
    {path: 'masters/approval-matrix-contract', component: ApprovalMatrixContractScreenComponent},
    {path: 'masters/approval-matrix-mou', component: ApprovalMatrixMouScreenComponent},
    {path: 'masters/documentMasters', component: MasterDocumentComponent},
    {path: 'masters/departmentMasters', component: MasterDepartmentComponent},
    {path: 'masters/escalationContracts', component: EscalationMatrixContractComponent},
    {path: 'masters/companyMasters/addCompany', component: MasterCompanyAddFormComponent},
    {path: 'auth/renewPassword', component: RenewPasswordComponent},
    {path: 'masters/companyMasters', component: MasterCompanyComponent},
    {path: '**', component: NotFoundComponent}

];
