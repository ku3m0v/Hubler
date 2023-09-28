import { Component } from '@angular/core';
import {AuthenticationService} from "../authentication.service";

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent {
  constructor(private authService: AuthenticationService) {}

  get isSignedIn(): boolean {
    return this.authService.isUserSignedIn();
  }

  signOut(): void {
    this.authService.signOut();
  }

  signIn(): void {
    this.authService.signIn();
  }

}
