import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { RouterService } from '../../services/router.service';
import { Alert } from '../../utils/alert';
import { TYPE } from '../auth/login/values.constants';

@Component({
  selector: 'app-side-bar',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './side-bar.component.html',
  styleUrl: './side-bar.component.css'
})
export class SideBarComponent {
  username:string | null = '';
  constructor(private route:RouterService) {}
  checkLogin():boolean {
    if(localStorage.getItem('token') != null)
    {
      this.username = localStorage.getItem('name');
      return true;
    }
    return false;
    // return true;
  }
  logoutUser() {
    if(localStorage.getItem('token') != null)
    {
      localStorage.clear();
      Alert.toast(TYPE.SUCCESS, true, "You've been logged out successfully!");
      this.route.goToLogin();
    }
  }
}
