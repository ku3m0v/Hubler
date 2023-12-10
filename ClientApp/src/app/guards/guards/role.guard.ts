import { Injectable } from '@angular/core';
import {CanActivate, ActivatedRouteSnapshot, Router, RouterStateSnapshot} from '@angular/router';
import { AuthenticationService } from '../../service/auth-service/authentication.service';

@Injectable({
  providedIn: 'root'
})
export class RoleGuard implements CanActivate {

  constructor(private authService: AuthenticationService, private router: Router) {}

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): boolean {
    const allowedRoles = next.data['roles'] as string[];

    if (this.authService.isUserSignedIn() && allowedRoles.includes(this.authService.getRole())) {
      return true;
    }

    // Navigate to a default route if unauthorized
    this.router.navigate(['**']); // Consider redirecting to a more appropriate route
    return false;
  }
}
