import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PostTermination} from '../models/post-termination';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PostTerminationService {
private apiUrl = `${environment.apiUrl}/PostTermination`;
  constructor(private http:HttpClient) { }


  UploadDoc(PostUpload:FormData):Observable<any>{
    console.log(PostUpload);
    return this.http.post<any>(`${this.apiUrl}/upload`,PostUpload)
  }
  ApproveTerminationContract(postTermination:PostTermination) : Observable<boolean> {
    return this.http.post<boolean>(`${this.apiUrl}/approveTerminationContract/${postTermination.contractId}/${postTermination.employeeEmail}/${postTermination.changeToStatus}/${postTermination.emailSubject}/${postTermination.emailBody}`, null)
  }
}
