import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthResponse, Login, PasswordRenewal } from '../../models/auth/login';
import { Observable } from 'rxjs';
import { DecodeToken } from '../../utils/decodeToken';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = `${environment.apiUrl}/Auth`;
  constructor(private http:HttpClient) { }
  login(authReq:Login):Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, authReq);
  }
  refreshPassword(refPswd:PasswordRenewal):Observable<string> {
    return this.http.post<string>(`${this.apiUrl}/refreshPassword`, refPswd);
  }
  isLoggedIn():boolean {
    return !!sessionStorage.getItem('token');
  }
  checkUserRole():string[] | null {
    return DecodeToken.ERole;
  }
}
