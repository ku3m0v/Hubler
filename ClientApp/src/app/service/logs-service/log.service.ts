import {HttpClient, HttpHeaders} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {AuthenticationService} from "../auth-service/authentication.service";
import configurl from "../../../assets/config/config.json";

export interface Log {
  tabulka: string;
  operace: string;
  cas: Date;
  uzivatel: string;
}

@Injectable({
  providedIn: 'root'
})
export class LogService {
  private myAppUrl: string = "";

  constructor(private http: HttpClient, private authService: AuthenticationService) {
    this.myAppUrl = configurl.apiServer.url;
  }

  getAllLogs(): Observable<Log[]> {
    return this.http.get<Log[]>(`${this.myAppUrl}api/logs/list`, {headers: this.createHeaders()});
  }

  downloadCsv(): Observable<Blob> {
    return this.http.get(`${this.myAppUrl}api/logs/download`, { responseType: 'blob' });
  }

  private createHeaders(): HttpHeaders {
    return new HttpHeaders({
      'Authorization': `Bearer ${this.authService.getToken()}`
    });
  }
}
