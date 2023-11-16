import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Observable } from 'rxjs';
import configurl from '../../../assets/config/config.json';

interface LoginCredentials {
    username: string;
    password: string;
}

@Injectable({
    providedIn: 'root'
})
export class AuthenticationService {
    private url = configurl.apiServer.url;

    constructor(private http: HttpClient, private router: Router, private jwtHelper: JwtHelperService ) {}

    signIn(credentials: LoginCredentials): Observable<any> {
        return this.http.post(this.url + 'api/authentication/login', credentials, {
            headers: new HttpHeaders({ 'Content-Type': 'application/json' })
        });
    }

    register(registrationData: RegistrationData): Observable<any> {
        return this.http.post(this.url + 'api/authentication/register', registrationData, {
            headers: new HttpHeaders({ 'Content-Type': 'application/json' })
        });
    }

    isUserSignedIn() {
        const token = localStorage.getItem("jwt");
        if (token && !this.jwtHelper.isTokenExpired(token)) {
            return true;
        } else {
            return false;
        }
    }

    signOut(): void {
        localStorage.removeItem("jwt");
        this.router.navigate(["/landing"]);
    }


}

export interface RegistrationData {
    email: string;
    password: string;
    firstName: string;
    lastName: string;
    supermarketTitle: string;
}
