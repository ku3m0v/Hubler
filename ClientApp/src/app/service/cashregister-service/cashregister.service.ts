import { Injectable } from '@angular/core';
import {HttpClient, HttpErrorResponse, HttpHeaders} from '@angular/common/http';
import {Observable, throwError} from 'rxjs';
import {AuthenticationService} from "../auth-service/authentication.service";
import configurl from "../../../assets/config/config.json";
import {catchError} from "rxjs/operators";
import {SupermarketWithAddress} from "../store-service/store.service";

@Injectable({
  providedIn: 'root'
})
export class CashRegisterService {
  private readonly apiUrl: string = "";

  constructor(private http: HttpClient, private authService: AuthenticationService) {
    this.apiUrl = configurl.apiServer.url;
  }

  getAllCashRegisters(): Observable<CashRegisterData[]> {
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${this.authService.getToken()}`
    });
    return this.http.get<CashRegisterData[]>(`${this.apiUrl}api/cashregister/list`, { headers: headers })
      .pipe(catchError(this.errorHandler));
  }

  updateCashRegister(cashRegister: CashRegisterData): Observable<any> {
    return this.http.put(`${this.apiUrl}api/cashregister`, cashRegister)
      .pipe(
        catchError(this.errorHandler)
      );
  }

  deleteCashRegister(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}api/cashregister/?id=${id}`)
      .pipe(
        catchError(this.errorHandler)
      );
  }

  insertCashRegister(cashRegister: CashRegisterData): Observable<any> {
    return this.http.post(`${this.apiUrl}api/cashregister/insert`, cashRegister)
      .pipe(
        catchError(this.errorHandler)
      );
  }

  private errorHandler(error: HttpErrorResponse) {
    console.error('An error occurred:', error.message);
    return throwError(error);
  }

}

export interface CashRegisterData {
  id?: number;
  supermarketName: string;
  registerNumber: number;
  statusName: string;
  employeeId: number;
}
