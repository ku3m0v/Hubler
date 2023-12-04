import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {AuthenticationService} from "../auth-service/authentication.service";
import configurl from "../../../assets/config/config.json";


@Injectable({
  providedIn: 'root'
})
export class NonperishableService {
  private myAppUrl: string = "";

  constructor(private http: HttpClient, private authService: AuthenticationService) {
    this.myAppUrl = configurl.apiServer.url;
  }

  getAll(): Observable<NonPerishableProductModel[]> {
    return this.http.get<NonPerishableProductModel[]>(`${this.myAppUrl}api/non-perishable/list`);
  }

  getById(id: number): Observable<NonPerishableProductModel> {
    return this.http.get<NonPerishableProductModel>(`${this.myAppUrl}api/non-perishable/detail?id=${id}`);
  }

  insert(model: NonPerishableProductModel): Observable<any> {
    return this.http.post(`${this.myAppUrl}api/non-perishable/insert`, model);
  }

  update(model: NonPerishableProductModel): Observable<any> {
    return this.http.post(`${this.myAppUrl}api/non-perishable/update`, model);
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.myAppUrl}api/non-perishable/delete?id=${id}`);
  }
}

export interface NonPerishableProductModel {
  productId: number;
  title: string;
  currentPrice: number;
  productType: string;
  shelfLife: number;
}

