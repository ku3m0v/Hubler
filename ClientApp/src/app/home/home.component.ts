import { Component } from '@angular/core';
import {AuthenticationService} from "../authentication.service";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  showLoading: boolean = true;
  showGallery: boolean = false;

  ngOnInit() {
    setTimeout(() => {
      this.showLoading = false;
      this.showGallery = true;
    }, 1000);
  }

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
