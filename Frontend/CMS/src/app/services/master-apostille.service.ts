import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { MasterApostille, MasterApostilleDto } from '../models/master-apostille';

@Injectable({
  providedIn: 'root'
})
export class MasterApostilleService {

  private apiUrl=`${environment.apiUrl}/Apostille`;
  constructor(private http: HttpClient) { }

  getApostilles(
    pageNumber:number,
    pageSize:number
  ):Observable<MasterApostille[]>{

    let params=new HttpParams()
    .set('pageNumber', pageNumber.toString())
    .set('pageSize', pageSize.toString())

    return this.http.get<MasterApostille[]>(`${this.apiUrl}/${pageNumber}/${pageSize}`,{params})
    .pipe(
      catchError((error)=>{
        console.log("Error fetching Apostille: ",error);
        return throwError(()=>new Error('Failed to fetch employees'));
      })
    );
  }

  getApostilleById(){

  }

  addApostille(){

  }

  updateApostille(){

  }

  deleteApostille(id:number){
    return this.http.delete<boolean>(`${this.apiUrl}/${id}`);
  }
}
