import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { RouterService } from '../../services/router.service';
import { Alert } from '../../utils/alert';
import { TYPE } from '../auth/login/values.constants';
import { NotificationService } from '../../services/notification.service';
import { CommonModule } from '@angular/common';
import { Subscription } from 'rxjs';
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
  notificationFlag = true;
  username: string | null = '';
  userRole: string[] | null = [];
  totalNotifications: number = 0;
  @ViewChild('navbar', { static: false }) navbar!: ElementRef;

  constructor(
    private notificationService: NotificationService, 
    private route: RouterService,
  ) { }
  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
  ngOnInit(): void {
    this.subscription = this.notificationService.trigger$.subscribe(() => {
      this.GetAllNotifications();
    });
  }

  checkLogin(): boolean {
    if (localStorage.getItem('token')) {
      this.username = DecodeToken.sub;
      this.userRole = DecodeToken.ERole;
      if(this.notificationFlag){
        this.GetAllNotifications();
        this.notificationFlag = false;
      }
      return true;
    }
    return false;
  }
  checkUserRole() : boolean{
    if(this.userRole == null || this.userRole == undefined) return false;
    return (this.userRole.includes("Admin") || this.userRole.includes("Management_User") || (this.userRole.includes("Super_Admin")));
  }
  GetAllNotifications() {
    let empCode: string | null = DecodeToken.ECode;
    if (empCode) {
      this.notificationService.getUnreadNotificationCount(empCode).subscribe({
        next: (response: number) => {
          this.totalNotifications = response;
          console.log("Notification number", response);
        }, error: (error) => {
          if(error.status == 401){
            let errmsg = error.error;
            Alert.toast(TYPE.ERROR, true, errmsg);
          }
        else{
          let errorMsg = JSON.stringify((error.message !== undefined) ? error.error.message : error.title);
          Alert.toast(TYPE.ERROR, true, errorMsg);
        }
        }
      });
    }
    else{
      localStorage.clear();
      DecodeToken.clearUserCredentials();
      this.route.goToLogin();
    }
  }
}
