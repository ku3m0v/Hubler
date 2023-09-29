import {Injectable} from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  // This is just a mock example, replace with your actual authentication logic
  private isAuthenticated: boolean = false;

  constructor() {
  }

  // Call this method when user signs in
  signIn(): void {
    this.isAuthenticated = true;
  }

  // Call this method when user signs out
  signOut(): void {
    this.isAuthenticated = false;
  }

  // Method to check authentication status
  isUserSignedIn(): boolean {
    return this.isAuthenticated;
  }

}
