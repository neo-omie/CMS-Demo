import { Component, ElementRef, EventEmitter, Input, OnDestroy, OnInit, Output, ViewChild } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { RouterService } from '../../services/router.service';
import { Alert } from '../../utils/alert';
import { TYPE } from '../auth/login/values.constants';
import { NotificationService } from '../../services/notification.service';
import { CommonModule } from '@angular/common';
import { Subscription } from 'rxjs';
import { DecodeToken } from '../../utils/decodeToken';
import { ErrorHandler } from '../../utils/errorHandler';

@Component({
  selector: 'app-side-bar',
  standalone: true,
  imports: [RouterModule, CommonModule],
  templateUrl: './side-bar.component.html',
  styleUrl: './side-bar.component.css'
})
export class SideBarComponent implements OnInit, OnDestroy {
  lightMode: boolean = true;
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
    let mode = sessionStorage.getItem('lightMode');
    if(mode !== null){
      this.lightMode = (mode == 'true') ? true : false;
      this.toggleBulb(this.lightMode);
    }
    this.subscription = this.notificationService.trigger$.subscribe(() => {
      this.GetAllNotifications();
    });
  }

  toggleBulb = (mode:boolean) => {
    this.lightMode = mode;
    sessionStorage.setItem('lightMode',mode+'');
    if(mode){
      document.documentElement.style.setProperty('--icon-color', '#8888f3');
      document.documentElement.style.setProperty('--pagenation-hover', '#8888f3');
      document.documentElement.style.setProperty('--filter-theme-toggle-color', 'white');
      document.documentElement.style.setProperty('--btn-theme-toggle-color', '#5f5fee');
      document.documentElement.style.setProperty('--table-header-color', '#5f5fee');
      document.documentElement.style.setProperty('--table-side-border-color', '#5f5fee');
      document.documentElement.style.setProperty('--table-strip-odd-color', '#ffffff');
      document.documentElement.style.setProperty('--table-strip-even-color', '#f2f2f2');
      document.documentElement.style.setProperty('--table-font-color', '#000000');
      document.documentElement.style.setProperty('--nav-footer-bg-color', '#f8f9fa');
      document.documentElement.style.setProperty('--bg-color', '#e9e7e5');
    }
    else{
      document.documentElement.style.setProperty('--icon-color', '#969494');
      document.documentElement.style.setProperty('--pagenation-hover', '#222024');
      document.documentElement.style.setProperty('--filter-theme-toggle-color', '#f5f5f5');
      document.documentElement.style.setProperty('--btn-theme-toggle-color', '#000000');//120321
      document.documentElement.style.setProperty('--table-header-color', '#000000');
      document.documentElement.style.setProperty('--table-side-border-color', '#ffffff');
      document.documentElement.style.setProperty('--table-strip-odd-color', '#3d3c3a');
      document.documentElement.style.setProperty('--table-strip-even-color', '#222024');
      document.documentElement.style.setProperty('--table-font-color', '#ffffff');
      document.documentElement.style.setProperty('--nav-footer-bg-color', '#222024');//010127
      document.documentElement.style.setProperty('--bg-color', '#424242');
    }
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
        }, error: (error) => {
          ErrorHandler.handle(error);
          
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
