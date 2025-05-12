import { Component, OnInit, Renderer2, ViewChild } from '@angular/core';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { Notification } from '../../models/notification';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { NotificationService } from '../../services/notification.service';
import { Alert } from '../../utils/alert';
import { TYPE } from '../auth/login/values.constants';
import { LoaderComponent } from '../loader/loader.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-notifications',
  standalone: true,
  imports: [CommonModule, LoaderComponent, MatTableModule, MatSortModule, MatFormFieldModule, MatInputModule],
  templateUrl: './notifications.component.html',
  styleUrl: './notifications.component.css'
})
export class NotificationsComponent implements OnInit {
  loading: boolean = true
  displayedColumns: string[] = ['notificationDate', 'notificationSubject', 'action'];
  dataSource = new MatTableDataSource<Notification>();
  @ViewChild(MatSort) sort!: MatSort;
  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
  }
  pageNumbers: number[] = [];
  maxPage: number = 1;
  notifications: Notification[] = [];
  notification?: Notification;
  errorMsg?: string;
  ngOnInit() {
    this.GetAllNotifications();
  }
  constructor(private notificationService: NotificationService, private renderer: Renderer2) { }
  GetAllNotifications() {
    let empCode:string = String(localStorage.getItem('empCode'));
    this.notificationService.getAllNotifications(empCode).subscribe({
      next: (response: Notification[]) => {
        this.loading = false;
        this.notifications = response;
        this.dataSource.data = response;
          if (this.sort) {
            this.dataSource.sort = this.sort;
          }
        // console.log(this.notifications);
      }, error: (error) => {
        console.error(error.error);
        this.errorMsg = JSON.stringify((error.message !== undefined) ? error.error.message : error.title);
        Alert.toast(TYPE.ERROR, true, this.errorMsg);
      }
    });
  }
  SeeNotificationDetails(id:number, empCode:string) {
    this.notificationService.getNotificationDetails(id, empCode).subscribe({
      next: (response: Notification) => {
        this.notification = response;
        this.notification.notificationDate = this.formatDate(String(this.notification.notificationDate))
        console.log(this.notification);
      }, error: (error) => {
        console.error(error.error);
        this.errorMsg = JSON.stringify((error.message !== undefined) ? error.error.message : error.title);
        Alert.toast(TYPE.ERROR, true, this.errorMsg);
      }
    });
  }
  private formatDate(date:string) {
    const d = new Date(date);
    let month = '' + (d.getMonth() + 1);
    let day = '' + d.getDate();
    const year = d.getFullYear();
    let hour = d.getHours();
    let ampm = 'AM';
    if(hour >= 12 && hour <= 23)
      ampm = 'PM'
    let minute = d.getMinutes();
    let seconds = d.getSeconds();
    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;
    return [[year, month, day].join('-'),[hour, minute, seconds].join(':'), ampm].join(' ');
    }
}
