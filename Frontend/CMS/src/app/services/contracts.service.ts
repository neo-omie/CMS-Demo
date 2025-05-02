import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AddContractDto, ContractsEntity, GetContractByIdDto } from '../models/contracts';

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
  addContract(addContractDto: AddContractDto) : Observable<ContractsEntity> {
    return this.http.post<ContractsEntity>(`${this.apiUrl}`,addContractDto);
  }
}
