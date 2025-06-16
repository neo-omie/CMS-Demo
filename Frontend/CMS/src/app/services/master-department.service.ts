import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { GetAllDepartmentsDto, MasterDepartment } from '../models/master-department';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MasterDepartmentService {
  private apiUrl = `${environment.apiUrl}/Department`;
  constructor(private http:HttpClient) { }

  getAllDepartments(pageNumber:number, pageSize:number, eCode:string):Observable<GetAllDepartmentsDto[]>{
    return this.http.get<GetAllDepartmentsDto[]>(`${this.apiUrl}?pageNumber=${pageNumber}&pageSize=${pageSize}&eCode=${eCode}`);
  }
  getDepartmentById(departmentId:number):Observable<MasterDepartment>{
    return this.http.get<MasterDepartment>(`${this.apiUrl}/${departmentId}`);
  }
  
  //https://localhost:7041/api/Department/NEO1?departmentName=check
  addDepartment(departmentName:string,empCode:string |null):Observable<MasterDepartment>{
    return this.http.post<MasterDepartment>(`${this.apiUrl}/${empCode}?departmentName=${departmentName}`, null);
  }

  //https://localhost:7041/api/Department/8/d?departmentName=d
  updateDepartment(departmentId:number, departmentName:string,empCode:string|null):Observable<boolean>{
    return this.http.put<boolean>(`${this.apiUrl}/${departmentId}/${empCode}?departmentName=${departmentName}`, null);
  }

  //https://localhost:7041/api/Department/8/NEO1
  deleteDepartment(departmentId:number,empCode:string |null):Observable<boolean>{
    return this.http.delete<boolean>(`${this.apiUrl}/${departmentId}/${empCode}`);
  }
}
