import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AddContractDto, ContractsEntity, GetContractByIdDto } from '../models/contracts';
import { MasterEmployee } from '../models/master-employee';
import { GetAllDepartmentsDto } from '../models/master-department';
import { ContractTypeMasterDTO } from '../models/contract-type-master';
import { CompanyMasterDto } from '../models/master-company';
import { MasterApostille, MasterApostilleDto } from '../models/master-apostille';
import { environment } from '../../environments/environment';
import { ContractsCount } from '../models/contracts-count';

@Injectable({
  providedIn: 'root'
})
export class ContractsService {
  private apiUrl = `${environment.apiUrl}/Contract`;
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
  getExpiredContracts(pageNumber: number, pageSize: number) : Observable<ContractsEntity[]> {
    return this.http.get<ContractsEntity[]>(`${this.apiUrl}/GetExpiredContracts?pageNumber=${pageNumber}&pageSize=${pageSize}`);
  }

  getContractByID(contractID: number) : Observable<GetContractByIdDto> {
    return this.http.get<GetContractByIdDto>(`${this.apiUrl}/${contractID}`);
  }
  getContractCounts() : Observable<ContractsCount> {
    return this.http.get<ContractsCount>(`${this.apiUrl}/GetContractsCount`);
  }

  deleteContract(contractID: number,empName:string |null ) : Observable<boolean> {
    return this.http.delete<boolean>(`${this.apiUrl}/${contractID}/${empName}`);
  }
  addContract(addContractDto: AddContractDto,empName:string|null) : Observable<boolean> {
    return this.http.post<boolean>(`${this.apiUrl}/${empName}`,addContractDto);
  }
  editContract(contractID:number, contract:AddContractDto) : Observable<boolean> {
    return this.http.put<boolean>(`${this.apiUrl}/${contractID}`, contract);
  }
  fetchContractData(contractID:number):Observable<AddContractDto> {
    return this.http.get<AddContractDto>(`${this.apiUrl}/${contractID}`);
  }
  approveRejectContract(id?:number, empCode?:string, status?:number) : Observable<boolean> {
    return this.http.post<boolean>(`${this.apiUrl}/${id}/approveRejectContract/${empCode}/${status}`,{});
  }

  // For dropdowns and inputs from other tables
  GetEmployeeForInputText(departmentId: number, inputText:string):Observable<MasterEmployee[]>{
      return this.http.get<MasterEmployee[]>(`${environment.apiUrl}/Employee/search/${departmentId}/${inputText}`)
  }
  GetDepartments():Observable<GetAllDepartmentsDto[]> {
    return this.http.get<GetAllDepartmentsDto[]>(`${environment.apiUrl}/Department?pageNumber=1&pageSize=100`);
  }
  GetContractTypes():Observable<ContractTypeMasterDTO[]> {
    return this.http.get<ContractTypeMasterDTO[]>(`${environment.apiUrl}/ContractTypeMaster?pageNumber=1&pageSize=100`);
  }
  GetCompanies():Observable<CompanyMasterDto[]> {
    return this.http.get<CompanyMasterDto[]>(`${environment.apiUrl}/MasterCompany/1/100`);
  }
  GetApostilleTypes() : Observable<MasterApostilleDto> {
    return this.http.get<MasterApostilleDto>(`${environment.apiUrl}/Apostille/1/100`);
  }
}
