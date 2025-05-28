import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MasterEscalationMatrixMouDto, UpdateMatrixMouDto } from '../models/master-escalation-matrix-mou-dto';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class EscalationMatrixMouService {
 private apiUrl = `${environment.apiUrl}/EscalationMatrixMou`;

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
  postMatrixMouById(
    valueId: number,
    updateMatrixMouDto: UpdateMatrixMouDto,
    empCode : string
  ): Observable<any> {
    return this.http.post<any>(
      `${this.apiUrl}/empCode/${empCode}/${valueId}`,updateMatrixMouDto
    );
  }
}
