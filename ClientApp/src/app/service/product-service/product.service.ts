import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { AuthenticationService } from "../auth-service/authentication.service";
import configurl from "../../../assets/config/config.json";

export interface LkProduct {
  lk_Product_Id: number;
  title: string;
  currentPrice: number;
}

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private myAppUrl: string = "";

  constructor(private http: HttpClient, private authService: AuthenticationService) {
    this.myAppUrl = configurl.apiServer.url;
  }

  getAll(): Observable<LkProduct[]> {
    return this.http.get<LkProduct[]>(`${this.myAppUrl}api/lkproduct/list`);
  }

  getDetails(title: string): Observable<LkProduct> {
    return this.http.get<LkProduct>(`${this.myAppUrl}api/lkproduct/get/${title}`);
  }

  insert(model: LkProduct): Observable<any> {
    return this.http.post(`${this.myAppUrl}api/lkproduct/insert`, model);
  }

  update(model: LkProduct): Observable<any> {
    return this.http.post(`${this.myAppUrl}api/lkproduct/update`, model);
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.myAppUrl}api/lkproduct/delete/${id}`);
  }
}
