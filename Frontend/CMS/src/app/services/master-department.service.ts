import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { GetAllDepartmentsDto, MasterDepartment } from '../models/master-department';

@Injectable({
  providedIn: 'root'
})
export class MasterDepartmentService {
  private apiUrl = 'https://localhost:7041/api/Department'
  constructor(private http:HttpClient) { }

  getAllDepartments(pageNumber:number, pageSize:number):Observable<GetAllDepartmentsDto[]>{
    return this.http.get<GetAllDepartmentsDto[]>(`${this.apiUrl}?pageNumber=${pageNumber}&pageSize=${pageSize}`);
  }
  getDepartmentById(departmentId:number):Observable<MasterDepartment>{
    return this.http.get<MasterDepartment>(`${this.apiUrl}/${departmentId}`);
  }
  
  addDepartment(departmentName:string):Observable<MasterDepartment>{
    return this.http.post<MasterDepartment>(`${this.apiUrl}?departmentName=${departmentName}`, null);
  }
  updateDepartment(departmentId:number, departmentName:string):Observable<boolean>{
    return this.http.put<boolean>(`${this.apiUrl}/${departmentId}?departmentName=${departmentName}`, null);
  }
  deleteDepartment(departmentId:number):Observable<boolean>{
    return this.http.delete<boolean>(`${this.apiUrl}/${departmentId}`);
  }
}
