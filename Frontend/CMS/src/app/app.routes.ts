import { Routes } from '@angular/router';
import { LoginScreenComponent } from './components/auth/login-screen/login-screen.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { MastersScreenComponent } from './components/masters-screen/masters-screen.component';
import { ApprovalMatrixContractScreenComponent } from './components/approval-matrix-contract-screen/approval-matrix-contract-screen.component';
import { MasterDocumentComponent } from './components/master-document/master-document.component';

export const routes: Routes = [
    {path: '', component: LoginScreenComponent}, 
    {path: 'dashboard', component: DashboardComponent},
    {path: 'masters', component: MastersScreenComponent}, 
    {path: 'masters/approval-matrix-contract', component: ApprovalMatrixContractScreenComponent},
    {path: 'masters/documentMasters', component:MasterDocumentComponent}
];
