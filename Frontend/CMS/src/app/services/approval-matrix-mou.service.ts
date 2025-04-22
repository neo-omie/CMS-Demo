import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ApprovalMatrixMou } from '../models/approval-matrix-mou';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApprovalMatrixMouService {
private apiUrl = "https://localhost:7041/api/ApprovalMatrixMOU";
    constructor(private http:HttpClient) { }
    GetApprovalMatrixMOU(pageNumber : number, pageSize : number):Observable<ApprovalMatrixMou[]> {
      return this.http.get<ApprovalMatrixMou[]>(`${this.apiUrl}/${pageNumber}/${pageSize}`);
    }
    GetApprovalMatrixMOUById(id : number):Observable<ApprovalMatrixMou> {
      return this.http.get<ApprovalMatrixMou>(`${this.apiUrl}/${id}`);
    }
}
