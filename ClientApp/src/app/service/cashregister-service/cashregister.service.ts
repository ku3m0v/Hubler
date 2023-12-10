import { Injectable } from '@angular/core';
import {HttpClient, HttpErrorResponse, HttpHeaders} from '@angular/common/http';
import {Observable, throwError} from 'rxjs';
import { AuthenticationService } from "../auth-service/authentication.service";
import configurl from "../../../assets/config/config.json";
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class CashRegisterService {
  private readonly apiUrl: string = "";

  constructor(private http: HttpClient, private authService: AuthenticationService) {
    this.apiUrl = configurl.apiServer.url;
  }

  getAll(): Observable<CashRegisterData[]> {
    const headers = this.getAuthorizationHeaders();
    return this.http.get<CashRegisterData[]>(`${this.apiUrl}api/cashregister/list`, { headers })
      .pipe(catchError(this.errorHandler));
  }

  getById(id: number): Observable<CashRegisterData> {
    const headers = this.getAuthorizationHeaders();
    return this.http.get<CashRegisterData>(`${this.apiUrl}api/cashregister/get?id=${id}`, { headers })
      .pipe(catchError(this.errorHandler));
  }

  update(cashRegister: CashRegisterData): Observable<any> {
    return this.http.post(`${this.apiUrl}api/cashregister/edit`, cashRegister)
      .pipe(catchError(this.errorHandler));
  }

  insert(cashRegister: CashRegisterData): Observable<any> {
    return this.http.post(`${this.apiUrl}api/cashregister/insert`, cashRegister)
      .pipe(catchError(this.errorHandler));
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}api/cashregister?id=${id}`)
      .pipe(catchError(this.errorHandler));
  }

  getStatuses(): Observable<string[]> {
    return this.http.get<string[]>(`${this.apiUrl}api/cashregister/statuses`)
      .pipe(catchError(this.errorHandler));
  }

  getEmployees(): Observable<Employee[]> {
    const headers = this.getAuthorizationHeaders();
    return this.http.get<Employee[]>(`${this.apiUrl}api/cashregister/employees`, { headers })
      .pipe(catchError(this.errorHandler));
  }

  getEmployeeById(id: number): Observable<Employee> {
    return this.http.get<Employee>(`${this.apiUrl}api/cashregister/employee?id=${id}`)
      .pipe(catchError(this.errorHandler));
  }

  getBySupermarketNameAndRegisterNumber(supermarketName: string, registerNumber: number): Observable<CashRegisterData> {
    const headers = this.getAuthorizationHeaders();
    return this.http.get<CashRegisterData>(
      `${this.apiUrl}api/cashregister/getDetails/${supermarketName}/${registerNumber}`,
      { headers }
    ).pipe(catchError(this.errorHandler));
  }

  getSupermarketTitles(): Observable<string[]> {
    return this.http.get<string[]>(`${this.apiUrl}api/cashregister/titles`, {headers: this.createHeaders()})
      .pipe(catchError(this.errorHandler));
  }

  private getAuthorizationHeaders(): HttpHeaders {
    return new HttpHeaders({
      'Authorization': `Bearer ${this.authService.getToken()}`
    });
  }

  private createHeaders(): HttpHeaders {
    return new HttpHeaders({
      'Authorization': `Bearer ${this.authService.getToken()}`
    });
  }

  private errorHandler(error: HttpErrorResponse) {
    console.error('An error occurred:', error.message);
    return throwError(error);
  }

}

export interface CashRegisterData {
    id: number;
    supermarketName: string;
    registerNumber: number;
    statusName: string;
    employeeId?: number; // Optional as denoted by the '?' symbol
    employeeName?: string; // Optional
}


export interface Employee {
  id: number;
  email: string;
  password?: string; // Optional since it's marked with '?' in the backend model
  firstName: string;
  lastName: string;
  createdDate: Date; // or string, depending on how dates are handled in your application
  supermarketName?: string; // Optional
  roleName: string;
  adminId?: number; // Optional
}
