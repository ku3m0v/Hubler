import {Component, ElementRef, OnInit, ViewChild} from '@angular/core';
import {FormBuilder} from "@angular/forms";
import {Router} from "@angular/router";
import {AuthenticationService} from "../service/auth-service/authentication.service";
import {ProfileData, ProfileService} from "../service/profile-service/profile.service";

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {
  profile: ProfileData | null = null;
  public isEditing = false;
  error: string | null = null;


  @ViewChild('imageInput', {static: false}) imageInput!: ElementRef;

  constructor(
    private fb: FormBuilder,
    private authService: AuthenticationService,
    private router: Router,
    private profileService: ProfileService) {
  }

  public isUserAuthenticated(): boolean {
    return this.authService.isUserSignedIn();
  }

  ngOnInit(): void {
    this.loadProfile();
  }

  loadProfile(): void {
    this.profileService.getProfile().subscribe({
      next: (data) => {
        this.profile = data;
      },
      error: (err) => {
        this.error = err.message;
      }
    });
  }

}
