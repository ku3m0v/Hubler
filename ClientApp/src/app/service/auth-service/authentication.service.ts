import { Injectable } from '@angular/core';
import { JwtHelperService } from "@auth0/angular-jwt";
import { Router } from "@angular/router";
import { BehaviorSubject, Observable } from 'rxjs';

const JWT_TOKEN = 'jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private isAuthenticated = new BehaviorSubject<boolean>(this.hasToken());

  constructor(private jwtHelper: JwtHelperService, private router: Router) {}

  isUserSignedIn(): Observable<boolean> {
    return this.isAuthenticated.asObservable();
  }

  private hasToken(): boolean {
    const token = localStorage.getItem(JWT_TOKEN);
    return !!token && !this.jwtHelper.isTokenExpired(token);
  }

  signOut(): void {
    localStorage.removeItem(JWT_TOKEN);
    this.isAuthenticated.next(false);
    this.router.navigate(['/landing']);
  }

  // This method should be called when user signs in successfully
  onSignIn(token: string): void {
    localStorage.setItem(JWT_TOKEN, token);
    this.isAuthenticated.next(true);
  }
}
