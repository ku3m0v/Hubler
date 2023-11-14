import { Injectable, Inject } from '@angular/core';
import {HttpClient, HttpErrorResponse} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import configurl from "../../../assets/config/config.json";

export interface SupermarketWithAddress {
  title: string;
  phone: string;
  street: string;
  house: string;
  city: string;
  postalCode: string;
  country: string;
}

@Injectable({
  providedIn: 'root'
})
export class SupermarketService {
  myAppUrl: string = "";

  constructor(private http: HttpClient) {
    this.myAppUrl = configurl.apiServer.url;
  }

  getAllSupermarkets(): Observable<SupermarketWithAddress[]> {
    return this.http.get<SupermarketWithAddress[]>(this.myAppUrl + 'api/supermarket/list')
      .pipe(
        catchError(this.errorHandler)
      );
  }

  updateSupermarket(supermarket: SupermarketWithAddress): Observable<any> {
    return this.http.post(this.myAppUrl + 'api/supermarket/update', supermarket)
      .pipe(
        catchError(this.errorHandler)
      );
  }

  deleteSupermarket(title: string): Observable<any> {
    return this.http.delete(this.myAppUrl + 'api/supermarket/delete?title=' + title)
      .pipe(
        catchError(this.errorHandler)
      );
  }

  errorHandler(error: HttpErrorResponse) {
    console.error('An error occurred:', error.message);
    return throwError(error);
  }
}
