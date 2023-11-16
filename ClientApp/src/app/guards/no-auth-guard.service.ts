import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import {AuthenticationService} from "../service/auth-service/authentication.service";

@Injectable({
    providedIn: 'root'
})
export class NoAuthGuard implements CanActivate {
    constructor(private authService: AuthenticationService, private router: Router) {}

    canActivate(): boolean {
        if (this.authService.isUserSignedIn()) {
            this.router.navigate(['/chart']);
            return false;
        }
        return true;
    }
}
