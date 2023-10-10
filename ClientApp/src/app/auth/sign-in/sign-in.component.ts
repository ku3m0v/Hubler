import { Component } from '@angular/core';
import {AuthenticationService} from "../../authentication.service";

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.css']
})
export class SignInComponent {
  showToast: boolean = false;
  showModal: boolean = false

  constructor(private authService: AuthenticationService) {
  }

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
    this.showToast = true;
    setTimeout(() => {
      this.showToast = false;
    }, 2000);
  }
}
