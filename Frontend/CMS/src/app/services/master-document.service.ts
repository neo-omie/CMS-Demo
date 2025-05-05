import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { AddDocumentDto, GetDocumentById, MasterDocument, MasterDocumentDto } from '../models/master-document';

@Injectable({
  providedIn: 'root'
})
export class MasterDocumentService {
  private apiUrl = 'https://localhost:7041/api/Document'

  constructor(private http:HttpClient) { }

  getDocument(pageNumber : number, pageSize : number):Observable<MasterDocumentDto>{
    return this.http.get<MasterDocumentDto>(`${this.apiUrl}/${pageNumber}/${pageSize}`).pipe(
      tap(() => console.log('HTTP request triggered')),
      catchError(err => {
        console.error('Error inside service:', err);
        return throwError(() => err); // rethrow
      })
    );
  }

  addDocument(masterDocument:FormData):Observable<any>{
    return this.http.post<any>(this.apiUrl,masterDocument);
  }

  // uploadDocument()

  updateDocument(docId?:number, data?:AddDocumentDto):Observable<string>{
    return this.http.put<string>(`${this.apiUrl}/${docId}`, data)
  }

  deleteDocument(documentId:number):Observable<boolean>{
    return this.http.delete<boolean>(`${this.apiUrl}/${documentId}`);
  }

  getById(id:number):Observable<GetDocumentById>{
    return this.http.get<GetDocumentById>(`${this.apiUrl}/${id}`);
  }
}
