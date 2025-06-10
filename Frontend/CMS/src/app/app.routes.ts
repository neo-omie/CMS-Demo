import { Routes } from '@angular/router';
import { LoginScreenComponent } from './components/auth/login-screen/login-screen.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { MastersScreenComponent } from './components/Masters/masters-screen/masters-screen.component';
import { ApprovalMatrixContractScreenComponent } from './components/ApprovalMatrixContract/approval-matrix-contract-screen/approval-matrix-contract-screen.component';
import { MasterDocumentComponent } from './components/master-document/master-document.component';
import { RenewPasswordComponent } from './components/auth/renew-password/renew-password.component';
import { NotFoundComponent } from './components/UtilComponents/not-found/not-found.component';

import { MasterDepartmentComponent } from './components/master-department/master-department.component';
import { authGuard } from './auth.guard';
import { MasterEmployeeComponent } from './components/master-employee/master-employee.component';
import { MasterCompanyComponent } from './components/master-company/master-company.component';
import { ContractTypeMasterComponent } from './components/contract-type-master/contract-type-master.component';
import { ContractsScreenComponent } from './components/contracts/contracts-screen/contracts-screen.component';
import { AllContractsComponent } from './components/contracts/all-contracts/all-contracts.component';
import { AllClassifiedContractComponent } from './components/classifiedContracts/all-classified-contract/all-classified-contract.component';
import { MasterApostilleComponent } from './components/master-apostille/master-apostille.component';
import { EscalationMatrixMouScreenComponent } from './components/EscalationMatrixMou/escalation-matrix-mou-screen/escalation-matrix-mou-screen.component';
import { NotificationsComponent } from './components/notifications/notifications.component';
import { ApprovalMatrixMouScreenComponent } from './components/ApprovalMatrixMou/approval-matrix-mou-screen/approval-matrix-mou-screen.component';
import { EscalationMatrixContractScreenComponent } from './components/EscalationMarixContract/escalation-matrix-contract-screen/escalation-matrix-contract-screen.component';
import { AddendumContractsComponent } from './components/addendum-contracts/addendum-contracts.component';
import { roleGuard } from './role.guard';
import { AuditScreenComponent } from './components/audit-screen/audit-screen.component';

export const routes: Routes = [
    { path: '', component: LoginScreenComponent },
    { path: 'auth/renewPassword', component: RenewPasswordComponent },
    { path: 'dashboard', loadChildren: () => import('./dashboard.routes').then(m => m.routes), canActivate: [authGuard] },
    { path: 'masters', component: MastersScreenComponent, canActivate: [authGuard, roleGuard] },


    { path: 'masters/approval-matrix-contract', component: ApprovalMatrixContractScreenComponent, canActivate: [authGuard, roleGuard] },
    { path: 'masters/approval-matrix-mou', component: ApprovalMatrixMouScreenComponent, canActivate: [authGuard, roleGuard] },

    { path: 'masters/documentMasters', component: MasterDocumentComponent, canActivate: [authGuard, roleGuard] },
    { path: 'masters/employeeMasters', component: MasterEmployeeComponent, canActivate: [authGuard, roleGuard] },
    { path: 'masters/apostilleMasters', component: MasterApostilleComponent, canActivate: [authGuard, roleGuard] },
    { path: 'masters/departmentMasters', component: MasterDepartmentComponent, canActivate: [authGuard, roleGuard] },

    { path: 'masters/escalationContracts', component: EscalationMatrixContractScreenComponent, canActivate: [authGuard, roleGuard] },
    { path: 'masters/escalationMOUs', component: EscalationMatrixMouScreenComponent, canActivate: [authGuard, roleGuard] },


    { path: 'masters/companyMasters', component: MasterCompanyComponent, canActivate: [authGuard, roleGuard] },
    { path: 'masters/contractTypeMasters', component: ContractTypeMasterComponent, canActivate: [authGuard, roleGuard] },

    { path: 'contracts', component: ContractsScreenComponent, canActivate: [authGuard] },
    { path: 'contracts/allContracts', component: AllContractsComponent, canActivate: [authGuard] },
    
    {path: 'contracts/addendumContract', component: AddendumContractsComponent, canActivate:[authGuard]},
    {path: 'contracts/addendumContract/:contractId', component: AddendumContractsComponent, canActivate:[authGuard]},
    { path: 'classifiedContracts/allContracts', component: AllClassifiedContractComponent, canActivate: [authGuard, roleGuard] },
    { path: 'reports', component: AuditScreenComponent, canActivate: [authGuard, roleGuard] },


    { path: 'notifications', component: NotificationsComponent, canActivate: [authGuard] },
    { path: '**', component: NotFoundComponent }

];
