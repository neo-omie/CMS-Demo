import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { AddDocumentDto, GetDocumentById, MasterDocument, MasterDocumentDto } from '../models/master-document';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MasterDocumentService {
  private apiUrl = `${environment.apiUrl}/Document`;

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

  addDocument(masterDocument:FormData,empCode:string|null):Observable<any>{
    console.log(masterDocument);
    return this.http.post<any>(`${this.apiUrl}/upload/${empCode}`,masterDocument);
  }

  // uploadDocument()


  updateDocument(docId?:number, data?:FormData):Observable<any>{
    return this.http.put<any>(`${this.apiUrl}/${docId}`, data)
  }

  updateDocumentWithoutFille(docId?:number, data?:any):Observable<any>{
    const headers = new HttpHeaders({
      'Content-Type': 'application/json' // Set the content type
    });
    return this.http.put<any>(`${this.apiUrl}/${docId}/updateWithoutFile`, data, {headers})
  }

  checkDocumentExist(data?:FormData):Observable<boolean>{
    
    return this.http.put<boolean>(`${this.apiUrl}/checkFileExists`,data);
  }
  
  deleteDocument(documentId:number,empCode :string|null):Observable<boolean>{
    return this.http.delete<boolean>(`${this.apiUrl}/${documentId}/${empCode}`);
  }

  getById(id:number):Observable<GetDocumentById>{
    return this.http.get<GetDocumentById>(`${this.apiUrl}/${id}`);
  }
}
