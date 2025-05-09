import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Notification } from '../models/notification';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private apiUrl = 'https://localhost:7041/api/Notification';
  constructor(private http:HttpClient) { }
  getAllNotifications(employeeCode:string) : Observable<Notification[]> {
    return this.http.get<Notification[]>(`${this.apiUrl}/${employeeCode}`);
  }
  getNotificationDetails(id:number, employeeCode:string) : Observable<Notification> {
    return this.http.get<Notification>(`${this.apiUrl}/${id}/${employeeCode}`);
  }
}
