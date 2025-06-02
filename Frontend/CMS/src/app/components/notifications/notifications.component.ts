import { Component, OnInit, Renderer2, ViewChild } from '@angular/core';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { Notification } from '../../models/notification';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { NotificationService } from '../../services/notification.service';
import { Alert } from '../../utils/alert';
import { TYPE } from '../auth/login/values.constants';
import { LoaderComponent } from '../UtilComponents/loader/loader.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { CommonModule } from '@angular/common';
import { DecodeToken } from '../../utils/decodeToken';
import { PaginationComponent } from '../UtilComponents/pagination/pagination.component';
import { Pagination } from '../../utils/pagination';

@Component({
  selector: 'app-notifications',
  standalone: true,
  imports: [CommonModule, LoaderComponent, MatTableModule, MatSortModule, MatFormFieldModule, MatInputModule,PaginationComponent],
  templateUrl: './notifications.component.html',
  styleUrl: './notifications.component.css'
})
export class NotificationsComponent implements OnInit {
  loading: boolean = true;
  currentPage = 1;
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
  totalNotifications: number = 0;
  ngOnInit() {
    this.GetAllNotifications(1,10);
  }

  
  constructor(private notificationService: NotificationService, private renderer: Renderer2) { }
  
  GetPage(pgNumber: number) {
   if (this.maxPage >= pgNumber && pgNumber >= 1) {
     this.GetAllNotifications(pgNumber, 10);
   }
 }
  GetAllNotifications(pageNumber:number,pageSize:number) {
    let empCode:string | null = DecodeToken.ECode;
    this.notificationService.getAllNotifications(empCode,pageNumber,pageSize).subscribe({
      next: (response: Notification[]) => {
        this.loading = false;
        this.notifications = response;
        this.totalNotifications = response.length;
        this.currentPage =pageNumber;
        console.log(this.notifications);
        
        this.dataSource.data = response;
          if (this.sort) {
            this.dataSource.sort = this.sort;
          }
           if (this.notifications != undefined && this.notifications.length > 0) {
                    let result = Pagination.paginator(
                      pageNumber,
                      this.notifications[0].totalRecords,
                      pageSize
                    );
                    this.maxPage = result.maxPage;
                    this.pageNumbers = result.pageNumbers;
                  }
        // console.log(this.notifications);
      }, error: (error) => {
        console.error(error.error);
        this.errorMsg = JSON.stringify((error.message !== undefined) ? error.error.message : error.title);
        Alert.toast(TYPE.ERROR, true, this.errorMsg);
      }
    });
  }
  SeeNotificationDetails(id:number) {
    // console.log(this.notifications);
    let empCode = DecodeToken.ECode;
    console.log(id,empCode);
    this.notificationService.getNotificationDetails(id, empCode).subscribe({
      next: (response: Notification) => {
        console.log(id,empCode);
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
