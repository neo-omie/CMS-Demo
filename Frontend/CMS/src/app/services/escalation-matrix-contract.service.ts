import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { EscalationMatrixContract, GetMasterEscalationMatrixContractByIdDto, MasterEscalationMatrixContractDto } from '../models/escalation-matrix-contract';

@Injectable({
  providedIn: 'root',
})
export class EscalationMatrixContractService {
  private apiUrl = 'https://localhost:7041/api/EscalationMatrixContract';

  constructor(private http: HttpClient) {}

  getAllMatrixContract(
    pageNumber: number,
    pageSize: number
  ): Observable<MasterEscalationMatrixContractDto> {
    return this.http.get<MasterEscalationMatrixContractDto>(
      `${this.apiUrl}/${pageNumber}/${pageSize}`
    );
  }
  getMatrixContractById(
    valueId: number
  ): Observable<GetMasterEscalationMatrixContractByIdDto> {
    return this.http.get<GetMasterEscalationMatrixContractByIdDto>(
      `${this.apiUrl}/${valueId}`
    );
  }

}
