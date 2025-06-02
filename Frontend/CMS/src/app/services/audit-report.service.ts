import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Audit } from '../models/audit';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuditReportService {
private apiUrl = `${environment.apiUrl}/AuditTrail`
  constructor(private http :HttpClient) { }

  getAllAudits(pageNumber:Number,pageSize:Number):Observable<Audit[]>{
    return this.http.get<Audit[]>(`${this.apiUrl}/${pageNumber}/${pageSize}`)
  }
}
