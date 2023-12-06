import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { AuthenticationService } from '../auth-service/authentication.service'; // Update the path accordingly
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import configurl from '../../../assets/config/config.json';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private myAppUrl: string = configurl.apiServer.url;

  constructor(private http: HttpClient, private authService: AuthenticationService) {}

  getAllOrders(): Observable<ProductOrderModel[]> {
    return this.http.get<ProductOrderModel[]>(`${this.myAppUrl}api/product_order/list`, { headers: this.createHeaders() })
      .pipe(catchError(this.errorHandler));
  }

  insertOrder(model: ProductOrderModel, type: string | null): Observable<any> {
    return this.http.post(`${this.myAppUrl}api/product_order/insert/${type}`, model, { headers: this.createHeaders() })
      .pipe(catchError(this.errorHandler));
  }

  deleteOrder(id: number): Observable<any> {
    return this.http.delete(`${this.myAppUrl}api/product_order/delete`, { headers: this.createHeaders() })
      .pipe(catchError(this.errorHandler));
  }

  getProducts(): Observable<any[]> {
    return this.http.get<any[]>(`${this.myAppUrl}api/product_order/products`, { headers: this.createHeaders() })
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

export interface ProductOrderModel {
  id: number;
  supermarketName: string;
  productName: string;
  expireDate: Date;
  storageType?: string;
  shelfLife?: number;
  quantity: number;
  orderDate: Date;
}

