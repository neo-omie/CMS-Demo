import { Component } from '@angular/core';
import { AuthResponse, Login } from '../../../models/auth/login';
import { UserService } from '../../../services/auth/user.service';
import { FormsModule, NgForm } from '@angular/forms';
import { CommonModule } from '@angular/common';
import Swal from 'sweetalert2';
import { TYPE } from './values.constants';

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
  constructor(private userService:UserService) {}
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
        // alert('You are successfully logged in!');
        this.toast(TYPE.SUCCESS, true, 'Signed in successfully');
        
      }, error:(error) => {
        console.error('Login Failed :(', error);
<<<<<<< Updated upstream
        if(error.error !== undefined){
          this.errorMsg = JSON.stringify(error.error.message);
          
        }
        else{
          this.errorMsg = JSON.stringify(error.message);
=======
        if(error.message !== undefined){
          this.errorMsg = JSON.stringify(error.error.message);
          this.toast(TYPE.ERROR, true, error.error.message);
        }
        else{
          this.errorMsg = JSON.stringify(error.message);
          this.toast(TYPE.ERROR, true, error.message);
>>>>>>> Stashed changes
        }
        // alert(this.errorMsg);
      }
    });
  }
<<<<<<< Updated upstream
  loginPasswordEyeToggle(){
    this.loginPasswordEyeOpen = !this.loginPasswordEyeOpen;
=======
  toast(typeIcon = TYPE.SUCCESS, timerProgressBar: boolean = false, op:string = "") {
    Swal.fire({
      toast: true,
      position: 'top',
      showConfirmButton: false,
      icon: typeIcon,
      timerProgressBar,
      timer: 5000,
      title: op
    })
>>>>>>> Stashed changes
  }
}
