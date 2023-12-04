import {Injectable} from '@angular/core';
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

  getAll(): Observable<EmployeeModel[]> {
    return this.http.get<EmployeeModel[]>(`${this.myAppUrl}api/employee/list`, {headers: this.createHeaders()})
      .pipe(catchError(this.errorHandler));
  }

  getDetails(email: string): Observable<EmployeeModel> {
    return this.http.get<EmployeeModel>(`${this.myAppUrl}api/employee/details/${email}`, {headers: this.createHeaders()})
      .pipe(catchError(this.errorHandler));
  }

  getById(id: number): Observable<EmployeeModel> {
    return this.http.get<EmployeeModel>(`${this.myAppUrl}api/employee/${id}`, {headers: this.createHeaders()});
  }

  insert(employee: EmployeeModel): Observable<any> {
    return this.http.post(`${this.myAppUrl}api/employee/insert`, employee, {
      headers: this.createHeaders(),
      responseType: 'text'})
      .pipe(catchError(this.errorHandler));
  }

  edit(employee: EmployeeModel): Observable<any> {
    return this.http.post(`${this.myAppUrl}api/employee/edit`, employee, {
      headers: this.createHeaders(),
      responseType: 'text'
    }).pipe(catchError(this.errorHandler));
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.myAppUrl}api/employee/${id}`, {headers: this.createHeaders()})
      .pipe(catchError(this.errorHandler));
  }

  getManagers(): Observable<EmployeeModel[]> {
    return this.http.get<EmployeeModel[]>(`${this.myAppUrl}api/employee/managers`, { headers: this.createHeaders() })
      .pipe(catchError(this.errorHandler));
  }

  getSupermarketTitles(): Observable<string[]> {
    return this.http.get<string[]>(`${this.myAppUrl}api/employee/titles`, {headers: this.createHeaders()})
      .pipe(catchError(this.errorHandler));
  }

  getRoles(): Observable<string[]> {
    return this.http.get<string[]>(`${this.myAppUrl}api/employee/roles`, {headers: this.createHeaders()})
      .pipe(catchError(this.errorHandler));

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

export interface EmployeeModel {
  id?: number;
  email: string;
  password?: string;
  firstName: string;
  lastName: string;
  createdDate: Date;
  supermarketName?: string;
  roleName: string;
  adminId?: number;
  adminName?: string;
}



