import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthenticationService } from '../auth-service/authentication.service';
import configurl from '../../../assets/config/config.json';

export interface WarehouseModel {
  id: number;
  supermarketTitle: string;
  productId: number;
  quantity: number;
  title: string;
  currentPrice: number; // Assuming decimal translates to number in TypeScript
  productType: string;
  expiryDate?: Date; // Nullable
  storageType?: string; // Nullable
  shelfLife?: number; // Nullable
}


@Injectable({
  providedIn: 'root'
})
export class WarehouseService {
  private myAppUrl: string = configurl.apiServer.url;

  constructor(private http: HttpClient, private authService: AuthenticationService) {}

  getAll(supermarketTitle: string): Observable<WarehouseModel[]> {
    return this.http.get<WarehouseModel[]>(`${this.myAppUrl}api/warehouse/list/${supermarketTitle}`, { headers: this.createHeaders() })
      .pipe(catchError(this.errorHandler));
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.myAppUrl}api/warehouse/delete/${id}`, { headers: this.createHeaders() })
      .pipe(catchError(this.errorHandler));
  }

  transferProduct(model: WarehouseModel): Observable<any> {
    return this.http.post(`${this.myAppUrl}api/warehouse/transfer_product`, model, { headers: this.createHeaders() })
      .pipe(catchError(this.errorHandler));
  }

  orderProducts(supermarketTitle: string): Observable<any> {
    return this.http.post(`${this.myAppUrl}api/warehouse/order_products/${supermarketTitle}`, null, { headers: this.createHeaders() })
      .pipe(catchError(this.errorHandler));
  }

  getSupermarketTitles(): Observable<string[]> {
    return this.http.get<string[]>(`${this.myAppUrl}api/warehouse/titles`, { headers: this.createHeaders() })
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
