import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApproveRejectWithdrawalDTO } from '../models/notice-withdrawal';

@Injectable({
  providedIn: 'root'
})
export class NoticeWithdrawalService {
  private apiUrl='https://localhost:7041/api/NoticeWithdrawal';
    constructor(private http:HttpClient) { }
  
  
    AddWithdrawalNotice(PostUpload:FormData):Observable<any>{
      console.log(PostUpload);
      return this.http.post<any>(`${this.apiUrl}/WithdrawalUpload`,PostUpload)
    }
    ApproveWithdrawalTermination(postTermination:ApproveRejectWithdrawalDTO) : Observable<boolean> {
      return this.http.post<boolean>(`${this.apiUrl}/approveWithdrawalNotice/${postTermination.contractId}/${postTermination.employeeEmail}/${postTermination.changeToStatus}/${postTermination.emailSubject}/${postTermination.emailBody}`, null)
    }
}
