import { Routes } from '@angular/router';
import { LoginScreenComponent } from './components/auth/login-screen/login-screen.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';

export const routes: Routes = [
    {path: '', component: LoginScreenComponent}, 
    {path: 'dashboard', component: DashboardComponent}, 
];
