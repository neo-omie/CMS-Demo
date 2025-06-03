import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, Subject, tap } from 'rxjs';
import { Notification } from '../models/notification';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private triggerSubject = new Subject<void>();

  // Observable to subscribe to
  trigger$ = this.triggerSubject.asObservable();

  private apiUrl = `${environment.apiUrl}/Notification`;
  constructor(private http:HttpClient) { }
  getAllNotifications(employeeCode:string | null,pageNumber:number,pageSize:number) : Observable<Notification[]> {
    return this.http.get<Notification[]>(`${this.apiUrl}/${pageNumber}/${pageSize}/${employeeCode}`);
  }
  getNotificationDetails(id:number, employeeCode:string | null) : Observable<Notification> {
    return this.http.get<Notification>(`${this.apiUrl}/${id}/${employeeCode}`).pipe(
    tap(() => {
      this.triggerSubject.next(); // Emits only after the HTTP call succeeds
    }));
  }
  getUnreadNotificationCount(employeeCode:string | null) : Observable<number> {
    return this.http.get<number>(`${this.apiUrl}/UnreadNotifications/${employeeCode}`);
  }
}
