import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ClassifiedPostTermination, PostTermination} from '../models/post-termination';

@Injectable({
  providedIn: 'root'
})
export class PostTerminationService {
private apiUrl='https://localhost:7041/api/PostTermination';
private apiUrl2='https://localhost:7041/api/ClassifiedPostTermination';

  constructor(private http:HttpClient) { }


  UploadDoc(PostUpload:FormData):Observable<any>{
    console.log(PostUpload);
    return this.http.post<any>(`${this.apiUrl}/upload`,PostUpload)
  }
  ApproveTerminationContract(postTermination:PostTermination) : Observable<boolean> {
    return this.http.post<boolean>(`${this.apiUrl}/approveTerminationContract/${postTermination.contractId}/${postTermination.employeeEmail}/${postTermination.changeToStatus}/${postTermination.emailSubject}/${postTermination.emailBody}`, null)
  }

  
  UploadClassifiedDoc(PostUpload:FormData):Observable<any>{
    console.log(PostUpload);
    return this.http.post<any>(`${this.apiUrl2}/upload`,PostUpload)
  }
  ApproveTerminationClassifiedContract(postTermination:ClassifiedPostTermination) : Observable<boolean> {
    return this.http.post<boolean>(`${this.apiUrl2}/approveTerminationContract/${postTermination.classifiedContractId}/${postTermination.employeeEmail}/${postTermination.changeToStatus}/${postTermination.emailSubject}/${postTermination.emailBody}`, null)
  }
}
