import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {AuthenticationService} from "../auth-service/authentication.service";
import configurl from "../../../assets/config/config.json";


@Injectable({
  providedIn: 'root'
})
export class PerishableService {
  private myAppUrl: string = "";

  constructor(private http: HttpClient, private authService: AuthenticationService) {
    this.myAppUrl = configurl.apiServer.url;
  }

  getAll(): Observable<PerishableProduct[]> {
    return this.http.get<PerishableProduct[]>(`${this.myAppUrl}api/perishable/list`);
  }

  getById(id: number): Observable<PerishableProduct> {
    return this.http.get<PerishableProduct>(`${this.myAppUrl}api/perishable/detail?id=${id}`);
  }

  insert(model: PerishableProduct): Observable<any> {
    return this.http.post(`${this.myAppUrl}api/perishable/insert`, model);
  }

  update(model: PerishableProduct): Observable<any> {
    return this.http.post(`${this.myAppUrl}api/perishable/update`, model);
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.myAppUrl}api/perishable/delete?id=${id}`);
  }
}

export interface PerishableProduct {
  productId: number;
  title: string;
  currentPrice: number;
  productType: string;
  expiryDate: Date | string;
  storageType: string;
}
