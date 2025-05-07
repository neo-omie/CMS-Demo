import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { AddApostilleDto, EditApostilleDto, MasterApostille, MasterApostilleDto } from '../models/master-apostille';

@Injectable({
  providedIn: 'root'
})
export class MasterApostilleService {

  private apiUrl=`${environment.apiUrl}/Apostille`;
  constructor(private http: HttpClient) { }

  getApostilles(
    pageNumber:number,
    pageSize:number,
    searchTerm?:string
  ):Observable<MasterApostilleDto>{

    let params=new HttpParams()
    .set('pageNumber', pageNumber.toString())
    .set('pageSize', pageSize.toString())

    if (searchTerm) params = params.set('searchTerm', searchTerm);

    return this.http.get<MasterApostilleDto>(`${this.apiUrl}/${pageNumber}/${pageSize}`,{params})
    .pipe(
      catchError((error)=>{
        console.log("Error fetching Apostille: ",error);
        return throwError(()=>new Error('Failed to fetch employees'));
      })
    );
  }

  getApostilleById(id:number):Observable<MasterApostille>{
    return this.http.get<MasterApostille>(`${this.apiUrl}/${id}`);
  }

  addApostille(apostille:AddApostilleDto):Observable<AddApostilleDto>{
    return this.http.post<AddApostilleDto>(`${this.apiUrl}`,apostille);
  }

  updateApostille(id:number,apostille:EditApostilleDto):Observable<EditApostilleDto>{
    return this.http.put<EditApostilleDto>(`${this.apiUrl}/${id}`,apostille);
  }

  deleteApostille(id:number){
    return this.http.delete<boolean>(`${this.apiUrl}/${id}`);
  }
}
