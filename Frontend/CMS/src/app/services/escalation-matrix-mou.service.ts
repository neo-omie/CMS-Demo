import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MasterEscalationMatrixMouDto } from '../models/master-escalation-matrix-mou-dto';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EscalationMatrixMouService {
 private apiUrl = 'https://localhost:7041/api/EscalationMatrixMou';

  constructor(private http: HttpClient) {}

  getAllMatrixMou(
    pageNumber: number,
    pageSize: number
  ): Observable<MasterEscalationMatrixMouDto[]> {
    return this.http.get<MasterEscalationMatrixMouDto[]>(
      `${this.apiUrl}/${pageNumber}/${pageSize}`
    );
  }
  getMatrixMouById(
    valueId: number
  ): Observable<MasterEscalationMatrixMouDto> {
    return this.http.get<MasterEscalationMatrixMouDto>(
      `${this.apiUrl}/${valueId}`
    );
  }
}
