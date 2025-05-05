import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AddClasifiedContractDto, ClassifiedContracts, GetClassifiedContractByIdDto } from '../models/classified-contracts';

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
    addContract(addContractDto: AddClasifiedContractDto) : Observable<ClassifiedContracts> {
      return this.http.post<ClassifiedContracts>(`${this.apiUrl}`,addContractDto);
    }
}
