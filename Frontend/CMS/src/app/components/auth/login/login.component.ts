import { Component } from '@angular/core';
import { AuthResponse, Login } from '../../../models/auth/login';
import { UserService } from '../../../services/auth/user.service';
import { FormControl, FormGroup, FormsModule, NgForm, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { TYPE } from './values.constants';;
import { RouterService } from '../../../services/router.service';
import { Alert } from '../../../utils/alert';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule,ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  loginModel:Login = new Login('','');
  loginForm = new FormGroup({
    email: new FormControl('',[Validators.required,Validators.email]),
    password: new FormControl('',[Validators.required,Validators.pattern('^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&-+=()])(?=\\S+$).{4,10}$')])
  })
  errorMsg = '';
  loginPasswordEyeOpen = false;
  constructor(private userService:UserService, private route:RouterService, private title:Title) {
    this.title.setTitle("Login - CMS");
  }
  ngonInit() {}
  loginUser() {
    if(this.loginForm.invalid){
      this.loginForm.markAllAsTouched();
      return;
    }
    else{
      const email = this.loginForm.get('email')?.value;
      const password = this.loginForm.get('password')?.value;
      if(email && password){
        this.loginModel.email = this.loginForm.get('email')?.value;
        this.loginModel.password = this.loginForm.get('password')?.value;
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
    }
  }
  loginPasswordEyeToggle(){
    this.loginPasswordEyeOpen = !this.loginPasswordEyeOpen;
  }
  get email(){
    return this.loginForm.get('email');
  }
  get password(){
    return this.loginForm.get('password');
  }
}
