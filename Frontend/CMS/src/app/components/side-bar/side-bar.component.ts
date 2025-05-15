import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { RouterService } from '../../services/router.service';
import { Alert } from '../../utils/alert';
import { TYPE } from '../auth/login/values.constants';
import { NotificationService } from '../../services/notification.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-side-bar',
  standalone: true,
  imports: [RouterModule, CommonModule],
  templateUrl: './side-bar.component.html',
  styleUrl: './side-bar.component.css'
})
export class SideBarComponent implements OnInit {
  username: string | null = '';
  totalNotifications: number = 0;
  constructor(private notificationService: NotificationService, private route: RouterService) { }
  ngOnInit(): void {
    this.GetAllNotifications();
  }

  checkLogin(): boolean {
    if (localStorage.getItem('token') != null) {
      this.username = localStorage.getItem('name');
      return true;
    }
    return false;
    // return true;
  }
  logoutUser() {
    if (localStorage.getItem('token') != null) {
      localStorage.clear();
      Alert.toast(TYPE.SUCCESS, true, "You've been logged out successfully!");
      this.route.goToLogin();
    }
  }
  GetAllNotifications() {
    let empCode: string = String(localStorage.getItem('empCode'));
    this.notificationService.getUnreadNotificationCount(empCode).subscribe({
      next: (response: number) => {
        this.totalNotifications = response;
      }, error: (error) => {
        console.error(error.error);
        let errorMsg = JSON.stringify((error.message !== undefined) ? error.error.message : error.title);
        Alert.toast(TYPE.ERROR, true, errorMsg);
      }
    });
  }
}
