import { Component } from '@angular/core';
import {AuthenticationService} from "../../authentication.service";

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.css']
})
export class SignUpComponent {
  showModal: boolean = false
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

  closeModalAndNavigate() {
    this.showModal = false;
  }

}
