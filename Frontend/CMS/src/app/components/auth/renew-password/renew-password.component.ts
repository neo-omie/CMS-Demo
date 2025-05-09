import { Component } from '@angular/core';
import { UserService } from '../../../services/auth/user.service';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { PasswordRenewal } from '../../../models/auth/login';
import { TYPE } from '../login/values.constants';
import Swal from 'sweetalert2';
import { RouterService } from '../../../services/router.service';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-renew-password',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './renew-password.component.html',
  styleUrl: './renew-password.component.css'
})
export class RenewPasswordComponent {
  renewModel:PasswordRenewal = new PasswordRenewal('', '', '', '');
  errorMsg = '';
  loginPasswordEyeOpen = false;
  constructor(private userService:UserService, private route:RouterService, private title:Title) {
    this.title.setTitle("Renew Password - CMS");
  }
  ngonInit() {}
  loginPasswordEyeToggle(){
    this.loginPasswordEyeOpen = !this.loginPasswordEyeOpen;
  }
  loginUser(renewForm:NgForm) {
      this.renewModel = renewForm.value;
      console.log(renewForm.value)
      if(this.renewModel.reenterNewPassword == this.renewModel.newPassword)
      {  
        this.userService.refreshPassword(this.renewModel).subscribe({
          next:(response:string) => {
            console.log('Password renewal Successful!', response);
            this.toast(TYPE.SUCCESS, true, 'Password renewed successfully');
            this.route.goToLogin();
            
          }, error:(error) => {
            console.error('Password renewal Failed :(', error);
            if(error.message !== undefined){
              this.errorMsg = JSON.stringify(error.error.message);
              this.toast(TYPE.ERROR, true, error.error.message);
            }
            else{
              this.errorMsg = JSON.stringify(error.message);
              this.toast(TYPE.ERROR, true, error.message);
            }
          }
        }); 
      }
      else {
        this.toast(TYPE.ERROR, true, "Password did not matched with above");
      }
    }
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
    }
}
