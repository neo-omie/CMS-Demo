import { Routes } from '@angular/router';
import { LoginScreenComponent } from './components/auth/login-screen/login-screen.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { MastersScreenComponent } from './components/masters-screen/masters-screen.component';
import { ApprovalMatrixContractScreenComponent } from './components/ApprovalMatrixContract/approval-matrix-contract-screen/approval-matrix-contract-screen.component';
import { MasterDocumentComponent } from './components/master-document/master-document.component';
import { RenewPasswordComponent } from './components/auth/renew-password/renew-password.component';
import { NotFoundComponent } from './components/misc/not-found/not-found.component';
import { ApprovalMatrixMouScreenComponent } from './components/approval-matrix-mou-screen/approval-matrix-mou-screen.component';
import { EscalationMatrixContractComponent } from './components/escalation-matrix-contract/escalation-matrix-contract.component';

import { MasterDepartmentComponent } from './components/master-department/master-department.component';
import { authGuard } from './auth.guard';
import { MasterEmployeeComponent } from './components/master-employee/master-employee.component';
import { MasterCompanyComponent } from './components/master-company/master-company.component';
//import { MasterCompanyComponent } from './components/master-company/master-company.component';
import { EscalationMatrixMouComponent } from './components/escalation-matrix-mou/escalation-matrix-mou.component';
import { ContractTypeMasterComponent } from './components/contract-type-master/contract-type-master.component';
import { ContractsScreenComponent } from './components/contracts/contracts-screen/contracts-screen.component';
import { AllContractsComponent } from './components/contracts/all-contracts/all-contracts.component';
import { ClassifiedContractsScreenComponent } from './components/classifiedContracts/classified-contracts-screen/classified-contracts-screen.component';
import { AllClassifiedContractComponent } from './components/classifiedContracts/all-classified-contract/all-classified-contract.component';
import { CreateClassifiedContractComponent } from './components/classifiedContracts/create-classified-contract/create-classified-contract.component';
import { MasterApostilleComponent } from './components/master-apostille/master-apostille.component';

import { ActiveContractsComponent } from './components/contracts/active-contracts/active-contracts.component';
import { PendingApprovalContractsComponent } from './components/contracts/pending-approval-contracts/pending-approval-contracts.component';
import { TerminatedContractsComponent } from './components/contracts/terminated-contracts/terminated-contracts.component';
import { NotificationsComponent } from './components/notifications/notifications.component';

export const routes: Routes = [
    {path: '', component: LoginScreenComponent}, 
    {path: 'auth/renewPassword', component: RenewPasswordComponent},
    {path: 'dashboard', component: DashboardComponent, canActivate:[authGuard]},
    {path: 'masters', component: MastersScreenComponent, canActivate:[authGuard]},

    {path: 'masters/approval-matrix-contract', component: ApprovalMatrixContractScreenComponent, canActivate:[authGuard]},
    {path: 'masters/approval-matrix-mou', component: ApprovalMatrixMouScreenComponent, canActivate:[authGuard]},

    {path: 'masters/documentMasters', component: MasterDocumentComponent, canActivate:[authGuard]},
    {path:'masters/employeeMasters', component:MasterEmployeeComponent, canActivate:[authGuard]},
    {path:'masters/apostilleMasters', component:MasterApostilleComponent,canActivate:[authGuard]},
    {path: 'masters/departmentMasters', component: MasterDepartmentComponent, canActivate:[authGuard]},
    
    {path: 'masters/escalationContracts', component: EscalationMatrixContractComponent, canActivate:[authGuard]},
    {path:'masters/escalationMOUs', component: EscalationMatrixMouComponent, canActivate:[authGuard]},
    
    
    {path: 'masters/companyMasters', component: MasterCompanyComponent, canActivate:[authGuard]},
    {path: 'masters/contractTypeMasters', component: ContractTypeMasterComponent, canActivate:[authGuard]},

    {path: 'contracts', component: ContractsScreenComponent, canActivate:[authGuard]},
    {path: 'contracts/allContracts', component: AllContractsComponent, canActivate:[authGuard]},
    {path: 'contracts/activeContracts', component: ActiveContractsComponent, canActivate:[authGuard]},
    {path: 'contracts/pendingApprovalContracts', component: PendingApprovalContractsComponent, canActivate:[authGuard]},
    {path: 'contracts/terminatedContracts', component: TerminatedContractsComponent, canActivate:[authGuard]},
    {path: 'classifiedContracts', component: ClassifiedContractsScreenComponent, canActivate:[authGuard]},
    {path: 'classifiedContracts/allContracts', component: AllClassifiedContractComponent, canActivate:[authGuard]},
    
    {path: 'classifiedContract/createNewClassifiedContract', component: CreateClassifiedContractComponent, canActivate:[authGuard]},

    {path: 'notifications', component: NotificationsComponent, canActivate:[authGuard]},
    {path: '**', component: NotFoundComponent}

];
