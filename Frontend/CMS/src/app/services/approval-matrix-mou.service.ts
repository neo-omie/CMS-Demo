import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ApprovalMatrixMou, EditApprovalMatrixMOUDto } from '../models/approval-matrix-mou';
import { Observable } from 'rxjs';
import { MasterEmployee } from '../models/master-employee';

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
    GetApproversForInputText(departmentId: number, inputText:string):Observable<MasterEmployee[]>{
      return this.http.get<MasterEmployee[]>(`https://localhost:7041/api/Employee/search/${departmentId}/${inputText}`)
    }
    EditApproverMatrixMOU(id:number,editApprovalMatrixMouDto:EditApprovalMatrixMOUDto):Observable<boolean>{
      return this.http.put<boolean>(`${this.apiUrl}/UpdateApprovalMatrixMOU?id=${id}`,editApprovalMatrixMouDto);
    }
}
