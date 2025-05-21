import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ApprovalMatrixContract, EditApprovalMatrixContractDto } from '../models/approval-matrix-contract';
import { Observable } from 'rxjs';
import { MasterEmployee } from '../models/master-employee';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ApproverMatrixContractService {
  private apiUrl = `${environment.apiUrl}/ApprovalMatrixContract`;
    constructor(private http:HttpClient) { }
    GetApprovalMatrixContract(pageNumber : number, pageSize : number):Observable<ApprovalMatrixContract[]> {
      return this.http.get<ApprovalMatrixContract[]>(`${this.apiUrl}/${pageNumber}/${pageSize}`);
    }
    GetApprovalMatrixContractById(id : number):Observable<ApprovalMatrixContract> {
      return this.http.get<ApprovalMatrixContract>(`${this.apiUrl}/${id}`);
    }
    GetApproversForInputText(departmentId: number, inputText:string):Observable<MasterEmployee[]>{
      return this.http.get<MasterEmployee[]>(`${environment.apiUrl}/Employee/search/${departmentId}/${inputText}`)
    }
    EditApproverMatrixContract(id:number,editApprovalMatrixContractDto:EditApprovalMatrixContractDto,empCode:string):Observable<boolean>{
      return this.http.post<boolean>(`${this.apiUrl}/empCode/${empCode}/${id}`,editApprovalMatrixContractDto);
    }
}