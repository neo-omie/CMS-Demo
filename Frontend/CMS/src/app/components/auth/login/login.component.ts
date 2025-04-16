import { Component } from '@angular/core';
import { AuthResponse, Login } from '../../../models/auth/login';
import { UserService } from '../../../services/auth/user.service';
import { FormsModule, NgForm } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  loginModel:Login = new Login('','');
  errorMsg = '';
  constructor(private userService:UserService) {}
  ngonInit() {}
  loginUser(loginForm:NgForm) {
    this.loginModel = loginForm.value;
    this.userService.login(this.loginModel).subscribe({
      next:(response:AuthResponse) => {
        console.log('Login Successful!', response);
        localStorage.setItem('token', response.token);
        localStorage.setItem('email', response.email);
        localStorage.setItem('name', response.name);
        alert('You are successfully logged in!');
        
      }, error:(error) => {
        console.error('Login Failed :(', error);
        this.errorMsg = JSON.stringify(error.error.message);
        alert(this.errorMsg);
      }
    });
  }
}
