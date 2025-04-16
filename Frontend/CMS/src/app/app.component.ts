import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { LoginScreenComponent } from './components/auth/login-screen/login-screen.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet,LoginScreenComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'CMS';
}
