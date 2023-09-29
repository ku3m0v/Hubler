import { Component } from '@angular/core';
import {AuthenticationService} from "../authentication.service";

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent {
  showModal: boolean = false;
  showToast: boolean = false;

  constructor(private authService: AuthenticationService) {}

  get isSignedIn(): boolean {
    return this.authService.isUserSignedIn();
  }
  toggleModal() {
    this.showModal = !this.showModal;
  }
  signOut(): void {
    this.authService.signOut();
  }
  signIn(): void {
    this.authService.signIn();
    this.showModal = false;
    this.showToast = true;
    setTimeout(() => {
      this.showToast = false;
    }, 2000);
  }

}
