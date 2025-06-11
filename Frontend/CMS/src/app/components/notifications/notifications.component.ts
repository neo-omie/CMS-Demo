import {
  Component,
  OnInit,
  Renderer2,
  TemplateRef,
  ViewChild,
} from '@angular/core';
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
import { TableComponent } from '../UtilComponents/table/table.component';
import { ErrorHandler } from '../../utils/errorHandler';

@Component({
  selector: 'app-notifications',
  standalone: true,
  imports: [
    CommonModule,
    LoaderComponent,
    MatTableModule,
    MatSortModule,
    MatFormFieldModule,
    MatInputModule,
    PaginationComponent,
    TableComponent,
  ],
  templateUrl: './notifications.component.html',
  styleUrl: './notifications.component.css',
})
export class NotificationsComponent implements OnInit {
  loading: boolean = true;
  currentPage = 1;
  displayedColumns: string[] = [
    'notificationDate',
    'notficationSubject',
    'action',
  ];
  
  pageNumbers: number[] = [];
  maxPage: number = 1;
  notifications: Notification[] = [];
  notification?: Notification;
  errorMsg?: string;
  totalNotifications: number = 0;
  columnsInfo: {
    [key: string]: {
      title?: string;
      isSort?: boolean;
      templateRef: TemplateRef<any> | null;
    };
  } = {};
@ViewChild('actionRef',{static:true}) actionRef !: TemplateRef<any>; 
@ViewChild('notifDate',{static:true}) notifDate !: TemplateRef<any>; 
  ngOnInit() {
    this.GetAllNotifications(1, 10);
    this.columnsInfo = {
      notificationDate: { isSort: false, title: 'Date', templateRef: this.notifDate },
      notficationSubject: {
        isSort: false,
        title: 'Notification',
        templateRef: null,
      },
      action: { isSort: false, title: 'Action', templateRef: this.actionRef },
    };
  }

  constructor(
    private notificationService: NotificationService,
    private renderer: Renderer2
  ) {}

  GetPage(pgNumber: number) {
    if (this.maxPage >= pgNumber && pgNumber >= 1) {
      this.currentPage=pgNumber;
      this.GetAllNotifications(pgNumber, 10);

    }
  }
  GetAllNotifications(pageNumber: number, pageSize: number) {
    let empCode: string | null = DecodeToken.ECode;
    this.notificationService
      .getAllNotifications(empCode, pageNumber, pageSize)
      .subscribe({
        next: (response: Notification[]) => {
          this.loading = false;
          this.notifications = response;

          if (
            this.notifications != undefined &&
            this.notifications.length > 0
          ) {
            let result = Pagination.paginator(
              pageNumber,
              this.notifications[0].totalRecords,
              pageSize
            );
            this.maxPage = result.maxPage;
            this.pageNumbers = result.pageNumbers;
          }
        },
        error: (error) => {
          ErrorHandler.handle(error);
        },
      });
  }
  SeeNotificationDetails(id: number) {
    // console.log(this.notifications);
    let empCode = DecodeToken.ECode;
    console.log(id, empCode);
    this.notificationService.getNotificationDetails(id, empCode).subscribe({
      next: (response: Notification) => {
        console.log(id, empCode);
        this.notification = response;
        this.notification.notificationDate = this.formatDate(
          String(this.notification.notificationDate)
        );
        console.log(this.notification);
      },
      error: (error) => {
        ErrorHandler.handle(error);
      },
    });
  }
  private formatDate(date: string) {
    const d = new Date(date);
    let month = '' + (d.getMonth() + 1);
    let day = '' + d.getDate();
    const year = d.getFullYear();
    let hour = d.getHours();
    let ampm = 'AM';
    if (hour >= 12 && hour <= 23) ampm = 'PM';
    let minute = d.getMinutes();
    let seconds = d.getSeconds();
    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;
    return [
      [year, month, day].join('-'),
      [hour, minute, seconds].join(':'),
      ampm,
    ].join(' ');
  }

  DeleteNotification(id:number ){
    this.notificationService.deleteNotification(id).subscribe({
      next:(res:boolean)=>{
          if (res) {
            Alert.toast(TYPE.SUCCESS,true,"message Deleted Successfully");
          }
          if (this.notifications.length == 1 ) {
           this.currentPage = (this.currentPage-1 > 0  )? this.currentPage : 1;
          }
          this.GetPage(this.currentPage);
        },
        error:(error)=>{
           ErrorHandler.handle(error);
      }
    })
  }
}
