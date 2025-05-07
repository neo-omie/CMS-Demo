import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Cities, Countriess, States } from '../models/company-cascade';

@Injectable({
  providedIn: 'root'
})
export class CompanyCascadeService {
  private apiUrl="https://localhost:7041/api/MasterCompanyCascade"
  constructor(private http:HttpClient) {
  
   }

   getCountries():Observable<Countriess[]>{
      return this.http.get<Countriess[]>(`${this.apiUrl}/GetCountries`);
   }

   getStates(countryId:number):Observable<States[]>{
    return this.http.get<States[]>(`${this.apiUrl}/GetStates?countryId=${countryId}`);
 }

 getCities(stateId:number):Observable<Cities[]>{
  return this.http.get<Cities[]>(`${this.apiUrl}/GetCities?stateId=${stateId}`);
}

// Get By ID
getCountryById(id?:number):Observable<Countriess> {
  return this.http.get<Countriess>(`${this.apiUrl}/GetCountryById?id=${id}`);
}
getStateById(id?:number):Observable<States> {
  return this.http.get<States>(`${this.apiUrl}/GetStateById?id=${id}`);
}
getCityById(id?:number):Observable<Cities> {
  return this.http.get<Cities>(`${this.apiUrl}/GetCityById?id=${id}`);
}

}
