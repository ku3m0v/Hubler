import { Injectable } from '@angular/core';
import {HttpClient, HttpErrorResponse, HttpHeaders} from '@angular/common/http';
import {Observable, throwError} from 'rxjs';
import {AuthenticationService} from "../auth-service/authentication.service";
import configurl from "../../../assets/config/config.json";
import {catchError} from "rxjs/operators";

export interface ExpiredInventory {
  inventoryId: number;
  productId: number;
  productTitle: string;
  quantity: number;
  supermarketId: number;
  supermarketTitle: string;
  expiryDate: Date;
}

export interface ExpiredWarehouse {
  warehouseId: number;
  productId: number;
  productTitle: string;
  quantity: number;
  supermarketId: number;
  supermarketTitle: string;
  expiryDate: Date;
}

export interface Top5ProductsBySupermarket {
  supermarketName: string;
  productName: string;
  totalQuantitySold: number;
}

export interface SupermarketSalesSummary {
  supermarketId: number;
  title: string;
  month: number;
  year: number;
  totalSales: number;
}

@Injectable({
  providedIn: 'root'
})
export class ViewsService {
  private myAppUrl: string = "";

  constructor(private http: HttpClient, private authService: AuthenticationService) {
    this.myAppUrl = configurl.apiServer.url;
  }

  getExpiredInventory(): Observable<ExpiredInventory[]> {
    return this.http.get<ExpiredInventory[]>(`${this.myAppUrl}api/view/ExpiredInventory`, { headers: this.createHeaders() })
      .pipe(catchError(this.errorHandler));
  }

  getExpiredWarehouse(): Observable<ExpiredWarehouse[]> {
    return this.http.get<ExpiredWarehouse[]>(`${this.myAppUrl}api/view/ExpiredWarehouse`, { headers: this.createHeaders() })
      .pipe(catchError(this.errorHandler));
  }

  getTop5ProductsBySupermarket(): Observable<Top5ProductsBySupermarket[]> {
    return this.http.get<Top5ProductsBySupermarket[]>(`${this.myAppUrl}api/view/Top5ProductsBySupermarket`, { headers: this.createHeaders() })
      .pipe(catchError(this.errorHandler));
  }

  getSupermarketSalesSummary(): Observable<SupermarketSalesSummary[]> {
    return this.http.get<SupermarketSalesSummary[]>(`${this.myAppUrl}api/view/SupermarketSalesSummary`, { headers: this.createHeaders() })
      .pipe(catchError(this.errorHandler));
  }

  private createHeaders(): HttpHeaders {
    return new HttpHeaders({
      'Authorization': `Bearer ${this.authService.getToken()}`
    });
  }

  private errorHandler(error: HttpErrorResponse) {
    console.error('An error occurred:', error.message);
    return throwError(() => error);
  }
}
