import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { MasterDocument, MasterDocumentDto } from '../models/master-document';

@Injectable({
  providedIn: 'root'
})
export class MasterDocumentService {
  private apiUrl = 'https://localhost:7041/api/Document'

  constructor(private http:HttpClient) { }

  getDocument(pageNumber : number, pageSize : number):Observable<MasterDocumentDto>{
    return this.http.get<MasterDocumentDto>(`${this.apiUrl}/${pageNumber}/${pageSize}`);
  }

}
