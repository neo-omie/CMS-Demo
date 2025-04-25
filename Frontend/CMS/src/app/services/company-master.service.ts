import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AddCompanyDto, CompanyListResponse, CompanyMasterDto, MasterCompany } from '../models/master-company';
import { AddDocumentDto } from '../models/master-document';

@Injectable({
  providedIn: 'root'
})
export class CompanyMasterService {
private apiUrl='https://localhost:7041/api/MasterCompany'
  constructor(private http:HttpClient) { }

  getCompany(pageNumber:number,pageSize:number):Observable<CompanyListResponse>{
    return this.http.get<CompanyListResponse>(`${this.apiUrl}/${pageNumber}/${pageSize}`);
  }

  addCompany(addCompanyDto:AddCompanyDto):Observable<MasterCompany>{
    return this.http.post<MasterCompany>(`${this.apiUrl}`,addCompanyDto)
  }
}
