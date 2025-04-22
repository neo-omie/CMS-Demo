import { Component } from '@angular/core';
import { AuthResponse, Login } from '../../../models/auth/login';
import { UserService } from '../../../services/auth/user.service';
import { FormsModule, NgForm } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { TYPE } from './values.constants';;
import { RouterService } from '../../../services/router.service';
import { Alert } from '../../../utils/alert';

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
  loginPasswordEyeOpen = false;
  constructor(private userService:UserService, private route:RouterService) {}
  ngonInit() {}
  loginUser(loginForm:NgForm) {
    this.loginModel = loginForm.value;
    console.log(loginForm.value)
    this.userService.login(this.loginModel).subscribe({
      next:(response:AuthResponse) => {
        console.log('Login Successful!', response);
        localStorage.setItem('token', response.token);
        localStorage.setItem('email', response.email);
        localStorage.setItem('name', response.name);
        Alert.toast(TYPE.SUCCESS, true, 'Signed in successfully');
        this.route.goToDashboard();
      }, error:(error) => {
        console.error('Login Failed :(', error);
        if(error.message !== undefined && error.status != 410){
          this.errorMsg = JSON.stringify(error.error.message);
          Alert.toast(TYPE.ERROR, true, error.error.message);
        }
        if(error.status == 410) {
          Alert.toast(TYPE.ERROR, true, error.error.message + `<a href="/auth/renewPassword">Click here to renew your password</a>`);
        }
        else{
          this.errorMsg = JSON.stringify(error.message);
          Alert.toast(TYPE.ERROR, true, error.error.message);
        }
      }
    });
  }
  loginPasswordEyeToggle(){
    this.loginPasswordEyeOpen = !this.loginPasswordEyeOpen;
  }
}
