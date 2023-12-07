import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { AuthenticationService } from '../auth-service/authentication.service'; // Update the path accordingly
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import configurl from '../../../assets/config/config.json';

@Injectable({
  providedIn: 'root'
})
export class InventoryService {
  private myAppUrl: string = configurl.apiServer.url;

  constructor(private http: HttpClient, private authService: AuthenticationService) {}

// Updated to take supermarketTitle as a parameter
  getAllInventory(supermarketTitle: string): Observable<InventoryModel[]> {
    return this.http.get<InventoryModel[]>(`${this.myAppUrl}api/inventory/list/${supermarketTitle}`, { headers: this.createHeaders() })
      .pipe(catchError(this.errorHandler));
  }

  // Delete an inventory item
  deleteInventory(id: number): Observable<any> {
    return this.http.delete(`${this.myAppUrl}api/inventory/delete/${id}`, {headers: this.createHeaders()})
      .pipe(catchError(this.errorHandler));
  }

  // Order products for a specific supermarket
  orderProducts(supermarketTitle: string): Observable<any> {
    return this.http.post(`${this.myAppUrl}api/order_products/${supermarketTitle}`, { headers: this.createHeaders() })
      .pipe(catchError(this.errorHandler));
  }


  getSupermarketTitles(): Observable<string[]> {
    return this.http.get<string[]>(`${this.myAppUrl}api/inventory/titles`, {headers: this.createHeaders()})
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

export interface InventoryModel {
  id: number;
  supermarketTitle: string;
  productId: number;
  quantity: number;
  title: string;
  currentPrice: number;
  productType: string;
  expiryDate?: Date;
  storageType?: string;
  shelfLife?: number;
}
