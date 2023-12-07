import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthenticationService } from '../auth-service/authentication.service'; // Update the path accordingly
import configurl from '../../../assets/config/config.json';

export interface SaleModel {
  saleId: number;
  supermarketName: string;
  saleDate: Date;
  saleDetailId: number;
  productId: number;
  productName: string;
  quantitySold: number;
  totalPrice: number; // Assuming decimal translates to number in TypeScript
}


export interface Product {
  title: string;
  quantity: number;
}

@Injectable({
  providedIn: 'root'
})
export class SaleService {
  private myAppUrl: string = configurl.apiServer.url;

  constructor(private http: HttpClient, private authService: AuthenticationService) {}

  getAllSales(supermarketTitle: string): Observable<SaleModel[]> {
    return this.http.get<SaleModel[]>(`${this.myAppUrl}api/sale/list/${supermarketTitle}`, { headers: this.createHeaders() });
  }

  insertSale(saleModel: SaleModel): Observable<any> {
    return this.http.post(`${this.myAppUrl}api/sale/insert`, saleModel, { headers: this.createHeaders() });
  }

  deleteSale(id: number): Observable<any> {
    return this.http.delete(`${this.myAppUrl}api/sale/delete/${id}`, { headers: this.createHeaders() });
  }

  getProducts(supermarketTitle: string): Observable<Product[]> {
    return this.http.get<Product[]>(`${this.myAppUrl}api/sale/products/${supermarketTitle}`, { headers: this.createHeaders() });
  }

  getSupermarketTitles(): Observable<string[]> {
    return this.http.get<string[]>(`${this.myAppUrl}api/sale/titles`, { headers: this.createHeaders() });
  }

  private createHeaders(): HttpHeaders {
    return new HttpHeaders({
      'Authorization': `Bearer ${this.authService.getToken()}`
    });
  }
}
