import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ApprovalMatrixContract, EditApprovalMatrixContractDto } from '../models/approval-matrix-contract';
import { Observable } from 'rxjs';
import { MasterEmployee } from '../models/master-employee';

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
    GetApproversForInputText(departmentId: number, inputText:string):Observable<MasterEmployee[]>{
      return this.http.get<MasterEmployee[]>(`https://localhost:7041/api/Employee/search/${departmentId}/${inputText}`)
    }
    EditApproverMatrixContract(id:number,editApprovalMatrixContractDto:EditApprovalMatrixContractDto):Observable<boolean>{
      return this.http.post<boolean>(`${this.apiUrl}/${id}`,editApprovalMatrixContractDto);
    }
}