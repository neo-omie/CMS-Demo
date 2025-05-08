import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AddClassifiedContractDto, ClassifiedContracts, GetClassifiedContractByIdDto } from '../models/classified-contracts';
import { GetAllDepartmentsDto } from '../models/master-department';
import { ContractTypeMasterDTO } from '../models/contract-type-master';
import { CompanyMasterDto } from '../models/master-company';
import { MasterEmployee } from '../models/master-employee';

@Injectable({
  providedIn: 'root'
})
export class ClassifiedContractsService {

  private apiUrl = "https://localhost:7041/api/ClassifiedContract";
    constructor(private http:HttpClient) { }
  
    getContracts(pageNumber: number, pageSize: number) : Observable<ClassifiedContracts[]> {
      return this.http.get<ClassifiedContracts[]>(`${this.apiUrl}?pageNumber=${pageNumber}&pageSize=${pageSize}`);
    }
    getContractByID(classifiedContractID: number) : Observable<GetClassifiedContractByIdDto> {
      return this.http.get<GetClassifiedContractByIdDto>(`${this.apiUrl}/${classifiedContractID}`);
    }
    deleteContract(classifiedContractID: number) : Observable<boolean> {
      return this.http.delete<boolean>(`${this.apiUrl}/${classifiedContractID}`);
    }
    addContract(addContractDto: AddClassifiedContractDto) : Observable<boolean> {
      return this.http.post<boolean>(`${this.apiUrl}`,addContractDto);
    }

    editContract(contractID:number, contract:AddClassifiedContractDto) : Observable<boolean> {
        return this.http.put<boolean>(`${this.apiUrl}/${contractID}`, contract);
      }
    fetchContractData(contractID:number):Observable<AddClassifiedContractDto> {
        return this.http.get<AddClassifiedContractDto>(`${this.apiUrl}/${contractID}`);
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
