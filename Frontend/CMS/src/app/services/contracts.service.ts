import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AddContractDto, ContractsEntity, GetContractByIdDto } from '../models/contracts';
import { MasterEmployee } from '../models/master-employee';
import { GetAllDepartmentsDto } from '../models/master-department';
import { ContractTypeMasterDTO } from '../models/contract-type-master';
import { CompanyMasterDto } from '../models/master-company';
import { MasterApostille, MasterApostilleDto } from '../models/master-apostille';

@Injectable({
  providedIn: 'root'
})
export class ContractsService {
  private apiUrl = "https://localhost:7041/api/Contract";
  constructor(private http:HttpClient) { }

  getContracts(pageNumber: number, pageSize: number) : Observable<ContractsEntity[]> {
    return this.http.get<ContractsEntity[]>(`${this.apiUrl}?pageNumber=${pageNumber}&pageSize=${pageSize}`);
  }
  getActiveContracts(pageNumber: number, pageSize: number) : Observable<ContractsEntity[]> {
    return this.http.get<ContractsEntity[]>(`${this.apiUrl}/GetActiveContracts?pageNumber=${pageNumber}&pageSize=${pageSize}`);
  }
  getTerminatedContracts(pageNumber: number, pageSize: number) : Observable<ContractsEntity[]> {
    return this.http.get<ContractsEntity[]>(`${this.apiUrl}/GetTerminatedContracts?pageNumber=${pageNumber}&pageSize=${pageSize}`);
  }
  getPendingApprovalContracts(pageNumber: number, pageSize: number) : Observable<ContractsEntity[]> {
    return this.http.get<ContractsEntity[]>(`${this.apiUrl}/GetPendingApprovalContracts?pageNumber=${pageNumber}&pageSize=${pageSize}`);
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
  editContract(contractID:number, contract:AddContractDto) : Observable<boolean> {
    return this.http.put<boolean>(`${this.apiUrl}/${contractID}`, contract);
  }
  fetchContractData(contractID:number):Observable<AddContractDto> {
    return this.http.get<AddContractDto>(`${this.apiUrl}/${contractID}`);
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
  GetApostilleTypes() : Observable<MasterApostilleDto> {
    return this.http.get<MasterApostilleDto>(`https://localhost:7041/api/Apostille/1/100`);
  }
}
