import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AddCompanyDto, CompanyListResponse, CompanyMasterDto, MasterCompany } from '../models/master-company';

@Injectable({
  providedIn: 'root'
})
export class CompanyMasterService {
private apiUrl='https://localhost:7041/api/MasterCompany'
  constructor(private http:HttpClient) { }

  getCompany(searchTerm:string, pageNumber:number,pageSize:number):Observable<CompanyMasterDto[]>{
    return this.http.get<CompanyMasterDto[]>(`${this.apiUrl}/${pageNumber}/${pageSize}?searchTerm=${searchTerm}`);
  }

   getCompanyById(valueId:number):Observable<MasterCompany>{
      return this.http.get<MasterCompany>(`${this.apiUrl}/${valueId}`);
    }

  addCompany(addCompanyDto:AddCompanyDto):Observable<MasterCompany>{
    return this.http.post<MasterCompany>(`${this.apiUrl}`,addCompanyDto)
  }



  updateCompany(valueId:number, addCompanyDto:AddCompanyDto):Observable<boolean>{
    return this.http.put<boolean>(`${this.apiUrl}/${valueId}`,addCompanyDto );
  }

  deleteCompany(valueId:number):Observable<boolean>{
    return this.http.delete<boolean>(`${this.apiUrl}/${valueId}`);
  }
}
