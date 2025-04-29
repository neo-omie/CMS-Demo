import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { AddEmployeeDto, EditEmployeeDto, MasterEmployee, MasterEmployeeDto } from '../models/master-employee';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MasterEmployeeService {
  private apiUrl=`${environment.apiUrl}/Employee`;
  
  constructor(private http: HttpClient) { }

  getEmployees(
    pageNumber:number,
    pageSize:number,
    unit?:string,
    searchTerm?:string
  ):Observable<MasterEmployeeDto>{
    
    let params=new HttpParams()
    .set('pageNumber', pageNumber.toString())
    .set('pageSize', pageSize.toString())

    if (unit) params = params.set('unit', unit);
    if (searchTerm) params = params.set('searchTerm', searchTerm);

    return this.http.get<MasterEmployeeDto>(`${this.apiUrl}/${pageNumber}/${pageSize}`,{params})
    .pipe(
      catchError((error)=>{
        console.log('Error fetching employees: ', error);
        return throwError(()=>new Error('Failed to fetch employees'));
      })
    );
  }

  getEmployeeById(id:number):Observable<MasterEmployee>{
    return this.http.get<MasterEmployee>(`${this.apiUrl}/${id}`);
  }

  addEmployee(employee:AddEmployeeDto):Observable<AddEmployeeDto>{
    return this.http.post<AddEmployeeDto>(`${this.apiUrl}`,employee);
  }

  updateEmployee(id:number, employee:EditEmployeeDto):Observable<EditEmployeeDto>{
    return this.http.put<EditEmployeeDto>(`${this.apiUrl}/${id}`, employee);
  }

  deleteEmployee(id:number):Observable<any>{
    return this.http.delete<MasterEmployee>(`${this.apiUrl}/${id}`);
  }
}
