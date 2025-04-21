import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthResponse, Login, PasswordRenewal } from '../../models/auth/login';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = "https://localhost:7041/api/Auth";
  constructor(private http:HttpClient) { }
  login(authReq:Login):Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, authReq);
  }
  refreshPassword(refPswd:PasswordRenewal):Observable<string> {
    return this.http.post<string>(`${this.apiUrl}/refreshPassword`, refPswd);
  }
}
