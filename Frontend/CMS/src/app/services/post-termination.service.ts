import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PostTerminationNoticeUploadDTO } from '../models/post-termination-notice';

@Injectable({
  providedIn: 'root'
})
export class PostTerminationService {
private apiUrl='https://localhost:7041/api/PostTermination';
  constructor(private http:HttpClient) { }


  UploadDoc(PostUpload:FormData):Observable<any>{
    console.log(PostUpload);
    return this.http.post<any>(`${this.apiUrl}/upload`,PostUpload)
  }
}
