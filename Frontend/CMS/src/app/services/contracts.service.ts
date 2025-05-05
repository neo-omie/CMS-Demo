import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AddContractDto, ContractsEntity, GetContractByIdDto } from '../models/contracts';
import { MasterEmployee } from '../models/master-employee';
import { GetAllDepartmentsDto } from '../models/master-department';
import { ContractTypeMasterDTO } from '../models/contract-type-master';
import { CompanyMasterDto } from '../models/master-company';

@Injectable({
  providedIn: 'root'
})
export class ContractsService {
  private apiUrl = "https://localhost:7041/api/Contract";
  constructor(private http:HttpClient) { }

  getContracts(pageNumber: number, pageSize: number) : Observable<ContractsEntity[]> {
    return this.http.get<ContractsEntity[]>(`${this.apiUrl}?pageNumber=${pageNumber}&pageSize=${pageSize}`);
  }
  getContractByID(contractID: number) : Observable<GetContractByIdDto> {
    return this.http.get<GetContractByIdDto>(`${this.apiUrl}/${contractID}`);
  }
  deleteContract(contractID: number) : Observable<boolean> {
    return this.http.delete<boolean>(`${this.apiUrl}/${contractID}`);
  }
  addContract(addContractDto: AddContractDto) : Observable<boolean> {
    return this.http.post<boolean>(`${this.apiUrl}`,addContractDto);
  }

  // For dropdowns and inputs from other tables
  GetEmployeeForInputText(departmentId: number, inputText:string):Observable<MasterEmployee[]>{
      return this.http.get<MasterEmployee[]>(`https://localhost:7041/api/Employee/search/${departmentId}/${inputText}`)
  }
  GetDepartments():Observable<GetAllDepartmentsDto[]> {
    return this.http.get<GetAllDepartmentsDto[]>(`https://localhost:7041/api/Department?pageNumber=1&pageSize=100`);
  }
  GetContractTypes():Observable<ContractTypeMasterDTO[]> {
    return this.http.get<ContractTypeMasterDTO[]>(`https://localhost:7041/api/ContractTypeMaster?pageNumber=1&pageSize=100`);
  }
  GetCompanies():Observable<CompanyMasterDto[]> {
    return this.http.get<CompanyMasterDto[]>(`https://localhost:7041/api/MasterCompany/1/100`);
  }
}
