import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AddDocumentDto, GetDocumentById, MasterDocument, MasterDocumentDto } from '../models/master-document';

@Injectable({
  providedIn: 'root'
})
export class MasterDocumentService {
  private apiUrl = 'https://localhost:7041/api/Document'

  constructor(private http:HttpClient) { }

  getDocument(pageNumber : number, pageSize : number):Observable<MasterDocumentDto>{
    return this.http.get<MasterDocumentDto>(`${this.apiUrl}/${pageNumber}/${pageSize}`);
  }

  addDocument(masterDocument:AddDocumentDto):Observable<AddDocumentDto>{
    return this.http.post<AddDocumentDto>(this.apiUrl,masterDocument);
  }

  updateDocument(updateMasterDocument?:GetDocumentById):Observable<GetDocumentById>{
    return this.http.put<GetDocumentById>(`${this.apiUrl}/`,updateMasterDocument)
  }

  deleteDocument(documentId:number):Observable<boolean>{
    return this.http.delete<boolean>(`${this.apiUrl}/${documentId}`);
  }

  getById(id:number):Observable<GetDocumentById>{
    return this.http.get<GetDocumentById>(`${this.apiUrl}/${id}`);
  }
}
