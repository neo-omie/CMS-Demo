import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApproveRejectWithdrawalDTO, ClassifiedApproveRejectWithdrawalDTO } from '../models/notice-withdrawal';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class NoticeWithdrawalService {
  
  private apiUrl2=`${environment.apiUrl}/ClassifiedNoticeWithdrawal`;
  private apiUrl = `${environment.apiUrl}/NoticeWithdrawal`;
    constructor(private http:HttpClient) { }
  
  
    AddWithdrawalNotice(PostUpload:FormData):Observable<any>{
      console.log(PostUpload);
      return this.http.post<any>(`${this.apiUrl}/WithdrawalUpload`,PostUpload)
    }
    ApproveWithdrawalTermination(postTermination:ApproveRejectWithdrawalDTO) : Observable<boolean> {
      return this.http.post<boolean>(`${this.apiUrl}/approveWithdrawalNotice/${postTermination.contractId}/${postTermination.employeeEmail}/${postTermination.changeToStatus}/${postTermination.emailSubject}/${postTermination.emailBody}`, null)
    }

    AddClassifiedWithdrawalNotice(PostUpload:FormData):Observable<any>{
      console.log(PostUpload);
      return this.http.post<any>(`${this.apiUrl2}/WithdrawalUpload`,PostUpload)
    }
    ClassifiedApproveWithdrawalTermination(postTermination:ClassifiedApproveRejectWithdrawalDTO) : Observable<boolean> {
      return this.http.post<boolean>(`${this.apiUrl2}/approveWithdrawalNotice/${postTermination.classifiedContractId}/${postTermination.employeeEmail}/${postTermination.changeToStatus}/${postTermination.emailSubject}/${postTermination.emailBody}`, null)
    }
}
