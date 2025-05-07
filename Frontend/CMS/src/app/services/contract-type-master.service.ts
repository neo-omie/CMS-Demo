import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AddContractDTO, ContractTypeMaster, ContractTypeMasterDTO } from '../models/contract-type-master';

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
  
    addContract(addContractDTO:AddContractDTO):Observable<ContractTypeMaster>{
      return this.http.post<ContractTypeMaster>(`${this.apiUrl}`,addContractDTO)
    }
  
    updateContract(valueId:number, contract:AddContractDTO):Observable<ContractTypeMaster>{
      return this.http.put<ContractTypeMaster>(`${this.apiUrl}/${valueId}`, contract);
    }
  
    deleteContract(valueId:number):Observable<boolean>{
      return this.http.delete<boolean>(`${this.apiUrl}/${valueId}`);
    }
}
