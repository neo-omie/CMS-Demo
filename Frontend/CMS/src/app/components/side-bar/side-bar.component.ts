import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { RouterModule } from '@angular/router';
import { RouterService } from '../../services/router.service';
import { Alert } from '../../utils/alert';
import { TYPE } from '../auth/login/values.constants';
import { NotificationService } from '../../services/notification.service';
import { CommonModule } from '@angular/common';
import { Subscription } from 'rxjs';
import { JwtClaims } from '../../models/jwt-claims';
import { jwtDecode } from 'jwt-decode';
import { DecodeToken } from '../../utils/decodeToken';

@Component({
  selector: 'app-side-bar',
  standalone: true,
  imports: [RouterModule, CommonModule],
  templateUrl: './side-bar.component.html',
  styleUrl: './side-bar.component.css'
})
export class SideBarComponent implements OnInit, OnDestroy {
  private subscription: Subscription = new Subscription();
  username: string | null = '';
  userRole: string | null = '';
  totalNotifications: number = 0;
  @ViewChild('navbar', { static: false }) navbar!: ElementRef;

  constructor(private notificationService: NotificationService, private route: RouterService) { }
  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
  ngOnInit(): void {
    this.GetAllNotifications();
    this.subscription = this.notificationService.trigger$.subscribe(() => {
      this.GetAllNotifications();
    });
  }

  checkLogin(): boolean {
    if (localStorage.getItem('token')) {
      this.username = DecodeToken.sub;
      this.userRole = DecodeToken.ERole;
      return true;
    }
    return false;
  }
  logoutUser() {
    if (localStorage.getItem('token') != null) {
      localStorage.clear();
      DecodeToken.clearUserCredentials();
      Alert.toast(TYPE.SUCCESS, true, "You've been logged out successfully!");
      this.route.goToLogin();
    }
  }
  GetAllNotifications() {
    let empCode: string | null = DecodeToken.ECode;
    this.notificationService.getUnreadNotificationCount(empCode).subscribe({
      next: (response: number) => {
        this.totalNotifications = response;
        console.log(response);
      }, error: (error) => {
        console.error(error.error);
        let errorMsg = JSON.stringify((error.message !== undefined) ? error.error.message : error.title);
        Alert.toast(TYPE.ERROR, true, errorMsg);
      }
    });
  }
}
