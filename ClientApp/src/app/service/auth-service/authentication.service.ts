import {Injectable} from '@angular/core';
import {JwtHelperService} from "@auth0/angular-jwt";
import {Router} from "@angular/router";

@Injectable({
    providedIn: 'root'
})
export class AuthenticationService {
    // This is just a mock example, replace with your actual authentication logic
    private isAuthenticated: boolean = false;

    constructor(private jwtHelper: JwtHelperService, private router: Router) {
    }

    isUserSignedIn() {
        const token = localStorage.getItem("jwt");
        if (token && !this.jwtHelper.isTokenExpired(token)) {
            return true;
        } else {
            return false;
        }
    }

    // Call this method when user signs in
    signIn(): void {
        this.isAuthenticated = true;
    }

    // Call this method when user signs out
    signOut(): void {
        localStorage.removeItem("jwt");
        this.router.navigate(["/landing"]);
    }


}
