import { Injectable } from '@angular/core';
import {HttpClient, HttpErrorResponse, HttpHeaders} from "@angular/common/http";
import {AuthenticationService} from "../auth-service/authentication.service";
import {Observable, throwError} from "rxjs";
import {catchError} from "rxjs/operators";
import configurl from "../../../assets/config/config.json";

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  private myAppUrl: string = "";

  constructor(private http: HttpClient, private authService: AuthenticationService) {
    this.myAppUrl = configurl.apiServer.url;
  }

  private createHeaders(): HttpHeaders {
    return new HttpHeaders({
      'Authorization': `Bearer ${this.authService.getToken()}`
    });
  }

  getAllEmployees(): Observable<Employee[]> {
    return this.http.get<Employee[]>(`${this.myAppUrl}api/employee/list`, { headers: this.createHeaders() })
      .pipe(catchError(this.errorHandler));
  }

  getEmployeeDetails(id: number): Observable<Employee> {
    return this.http.get<Employee>(`${this.myAppUrl}api/employee/${id}`, { headers: this.createHeaders() })
      .pipe(catchError(this.errorHandler));
  }

  addEmployee(employee: Employee): Observable<any> {
    return this.http.post(`${this.myAppUrl}api/employee/insert`, employee, { headers: this.createHeaders() })
      .pipe(catchError(this.errorHandler));
  }

  updateEmployee(employee: Employee): Observable<any> {
    return this.http.put(`${this.myAppUrl}api/employee/edit`, employee, { headers: this.createHeaders() })
      .pipe(catchError(this.errorHandler));
  }

  deleteEmployee(id: number): Observable<any> {
    return this.http.delete(`${this.myAppUrl}api/employee/${id}`)
      .pipe(
        catchError(this.errorHandler)
      );
  }

  private errorHandler(error: HttpErrorResponse) {
    console.error('An error occurred:', error.message);
    return throwError(error);
  }
}

export interface Employee {
  id?: number;
  password?: string;
  email: string;
  firstName: string;
  lastName: string;
  createdDate: Date;
  supermarketName: string;
  roleName: string;
  adminId?: number;
}
