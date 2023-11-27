import { Injectable } from '@angular/core';
import {Observable} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {AuthenticationService} from "../auth-service/authentication.service";
import configurl from "../../../assets/config/config.json";

@Injectable({
  providedIn: 'root'
})


export class StatusService {
  private readonly apiUrl: string = "";

  constructor(private http: HttpClient, private authService: AuthenticationService) {
    this.apiUrl = configurl.apiServer.url;
  }
  getDetails(statusName: string): Observable<StatusModel> {
    return this.http.get<StatusModel>(`${this.apiUrl}api/status/detail/${statusName}`);
  }

  create(status: StatusModel): Observable<any> {
    return this.http.post(`${this.apiUrl}api/status/insert`, status);
  }

  update(status: StatusModel): Observable<any> {
    return this.http.post(`${this.apiUrl}api/status/edit`, status);
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}api/status/?id=${id}`, { params: { id } });
  }

  GetAll(): Observable<StatusModel[]> {
    return this.http.get<StatusModel[]>(`${this.apiUrl}api/status`);
  }

}

export interface StatusModel {
  id: number;
  statusName: string;
}
