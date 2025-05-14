import { Injectable } from '@angular/core';
import { AddAddendumContract } from '../models/add-addendum-contract';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { AddContractDto } from '../models/contracts';

@Injectable({
  providedIn: 'root'
})
export class AddAddendumContractsService {
  private apiUrl=`${environment.apiUrl}`;

  constructor(private http:HttpClient) { }

  AddAddendum(id:number, addendum:AddAddendumContract):Observable<AddAddendumContract>{
    return this.http.put<AddAddendumContract>(`${this.apiUrl}/AddendumContract/${id}`,addendum);
  }

  fetchContractData(contractID:string):Observable<AddContractDto> {
      return this.http.get<AddContractDto>(`${this.apiUrl}/Contract/${contractID}`);
  }
}
