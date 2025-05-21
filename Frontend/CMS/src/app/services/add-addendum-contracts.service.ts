import { Injectable } from '@angular/core';
import { AddAddendumContract } from '../models/add-addendum-contract';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { AddContractDto } from '../models/contracts';
import { AddendumContract } from '../models/addendum-contract';

@Injectable({
  providedIn: 'root'
})
export class AddAddendumContractsService {
  private apiUrl=`${environment.apiUrl}`;

  constructor(private http:HttpClient) { }

  fetchContractData(contractID:string):Observable<AddContractDto> {
      return this.http.get<AddContractDto>(`${this.apiUrl}/Contract/${contractID}`);
  }

  AddAddendum(id:number, addendum:AddAddendumContract):Observable<AddAddendumContract>{
    return this.http.post<AddAddendumContract>(`${this.apiUrl}/AddendumContract/${id}`,addendum);
  }

  GetAddenduByAddendumId(id:number){
    return this.http.get<AddAddendumContract>(`${this.apiUrl}/AddendumContract/${id}`);
  }

  approveRejectContract(contractId?:number, id?:number, email?:string, status?:number) : Observable<boolean> {
    return this.http.post<boolean>(`${this.apiUrl}/AddendumContract/${id}/approveRejectAddendum/${email}/${status}?contractId=${contractId}`,{});
  }

  // GetAddendumByContractId(pageNumber:number, pageSize:number, id:number){
  //   return this.http.get<AddAddendumContract>(`${this.apiUrl}/AddendumContract/${pageNumber}/${pageSize}/${id}`);
  // }

  GetAllAddendum(pageNumber:number, pageSize:number, id:number){
    if(id === 0){
      return this.http.get<AddendumContract>(`${this.apiUrl}/AddendumContract/${pageNumber}/${pageSize}`);
    }
    return this.http.get<AddendumContract>(`${this.apiUrl}/AddendumContract/${pageNumber}/${pageSize}/${id}`);
  }

  DeleteAddendum(id:number){
    return this.http.delete<AddAddendumContract>(`${this.apiUrl}/AddendumContract/${id}`);
  }
}
