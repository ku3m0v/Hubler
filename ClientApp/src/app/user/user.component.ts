import {Component, ElementRef, OnInit, ViewChild} from '@angular/core';
import {FormBuilder, FormGroup} from "@angular/forms";
import {AuthenticationService} from "../service/auth-service/authentication.service";
import {ProfileData, ProfileService} from "../service/profile-service/profile.service";

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {
  profile: ProfileData | null = null;
  error: string | null = null;
  public profileForm!: FormGroup;


  @ViewChild('imageInput', {static: false}) imageInput!: ElementRef;

  constructor(
    private fb: FormBuilder,
    private authService: AuthenticationService,
    private profileService: ProfileService) {
  }

  public isUserAuthenticated(): boolean {
    return this.authService.isUserSignedIn();
  }

  ngOnInit(): void {
    this.profileForm = this.fb.group({
      email: [{value: '', disabled: true}],
      firstName: [''],
      lastName: [''],
      createdDate: [{value: '', disabled: true}],
      supermarketName: [{value: '', disabled: true}]
    });
    this.loadProfile();
  }

  saveProfile() {
    if (this.profileForm.valid) {
      const updatedProfile = this.profileForm.getRawValue();
      this.profileService.updateProfile(updatedProfile).subscribe({
        next: () => {
          this.loadProfile();
        },
        error: (err) => this.error = err.message
      });
    }
  }

  changeImage() {
    this.imageInput.nativeElement.click();
  }

  onImageChange(event: Event) {
    const fileInput = event.target as HTMLInputElement;
    if (fileInput.files && fileInput.files[0]) {
      const file = fileInput.files[0];

      this.profileService.uploadPhoto(file).subscribe({
        next: (response) => {
          if (response.imageUrl) {
            const imageElement: HTMLImageElement = document.querySelector('img[alt="Profile picture"]') as HTMLImageElement;
            imageElement.src = response.imageUrl;
          }
        },
        error: (err) => {
          this.error = err.message;
        }
      });
    }
  }

  loadProfile(): void {
    this.profileService.getProfile().subscribe({
      next: (data) => {
        this.profile = data;
        this.profileForm.setValue({
          email: data.email,
          firstName: data.firstName,
          lastName: data.lastName,
          createdDate: this.formatDate(data.createdDate),
          supermarketName: data.supermarketName
        });
      },
      error: (err) => {
        this.error = err.message;
      }
    });
  }

  private formatDate(dateString: Date): string {
    const date = new Date(dateString);
    return date.toISOString().split('T')[0];
  }

}
