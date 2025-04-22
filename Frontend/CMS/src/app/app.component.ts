import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { LoginScreenComponent } from './components/auth/login-screen/login-screen.component';
import { SideBarComponent } from './components/side-bar/side-bar.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, LoginScreenComponent, DashboardComponent, SideBarComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'CMS';
}
