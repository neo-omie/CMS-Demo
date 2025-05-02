import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ContractTypeMaster, ContractTypeMasterDTO } from '../models/contract-type-master';

@Injectable({
  providedIn: 'root'
})

export class ContractTypeMasterService {
  private apiUrl='https://localhost:7041/api/ContractTypeMaster'
  constructor(private http:HttpClient) { }

  getContract(pageNumber:number,pageSize:number):Observable<ContractTypeMasterDTO[]>{
      return this.http.get<ContractTypeMasterDTO[]>(`${this.apiUrl}?pageNumber=${pageNumber}&pageSize=${pageSize}`);
  }
  
  getContractById(valueId:number):Observable<ContractTypeMaster>{
        return this.http.get<ContractTypeMaster>(`${this.apiUrl}/${valueId}`);
      }
  
    addContract(addContractDto:ContractTypeMaster):Observable<ContractTypeMaster>{
      return this.http.post<ContractTypeMaster>(`${this.apiUrl}`,addContractDto)
    }
  
    updateContract(valueId:number, ContractName:string):Observable<boolean>{
      return this.http.put<boolean>(`${this.apiUrl}/${valueId}?contractName=${ContractName}`, null);
    }
  
    deleteContract(valueId:number):Observable<boolean>{
      return this.http.delete<boolean>(`${this.apiUrl}/${valueId}`);
    }
}
