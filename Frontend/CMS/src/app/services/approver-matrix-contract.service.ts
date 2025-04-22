import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ApprovalMatrixContract } from '../models/approval-matrix-contract';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApproverMatrixContractService {
  private apiUrl = "https://localhost:7041/api/ApprovalMatrixContract";
    constructor(private http:HttpClient) { }
    GetApprovalMatrixContract(pageNumber : number, pageSize : number):Observable<ApprovalMatrixContract[]> {
      return this.http.get<ApprovalMatrixContract[]>(`${this.apiUrl}/${pageNumber}/${pageSize}`);
    }
    GetApprovalMatrixContractById(id : number):Observable<ApprovalMatrixContract> {
      return this.http.get<ApprovalMatrixContract>(`${this.apiUrl}/${id}`);
    }
}
