import { Injectable } from '@angular/core';
import {Observable} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {AuthenticationService} from "../auth-service/authentication.service";
import configurl from "../../../assets/config/config.json";

@Injectable({
  providedIn: 'root'
})


export class RoleService {
  private readonly apiUrl: string = "";

  constructor(private http: HttpClient, private authService: AuthenticationService) {
    this.apiUrl = configurl.apiServer.url;
  }
  getRoleDetails(roleName: string): Observable<RoleData> {
    return this.http.get<RoleData>(`${this.apiUrl}api/role/detail/${roleName}`);
  }

  createRole(role: RoleData): Observable<any> {
    return this.http.post(`${this.apiUrl}api/role/insert`, role);
  }

  updateRole(role: RoleData): Observable<any> {
    return this.http.post(`${this.apiUrl}api/role/edit`, role);
  }

  deleteRole(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}api/role/?id=${id}`, { params: { id } });
  }

  getAllRoles(): Observable<RoleData[]> {
    return this.http.get<RoleData[]>(`${this.apiUrl}api/role`);
  }

}

export interface RoleData {
  id: number;
  roleName: string;
}
