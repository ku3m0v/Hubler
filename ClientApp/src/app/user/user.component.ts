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
  profileImageUrl: string | null = null;
  messageContent: string = '';
  showMessage: boolean = false;

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
    this.loadProfilePicture();
  }

  showMessageWithTimeout(message: string) {
    this.messageContent = message;
    this.showMessage = true;
    setTimeout(() => {
      this.showMessage = false;
    }, 1500); // Hide the message after 1.5 seconds
  }

  saveProfile() {
    if (this.profileForm.valid) {
      const updatedProfile = this.profileForm.getRawValue();
      this.profileService.updateProfile(updatedProfile).subscribe({
        next: () => {
          this.showMessageWithTimeout('Changes been submitted successfully');
          console.log('Changes been submitted successfully');
          // Additional logic after successful order
        },
        error: (error) => {
          this.showMessageWithTimeout('There was an error processing your changes.');
          console.error('There was an error!', error);
        }
      });
    }
  }

  onImageChange(event: Event) {
    const fileInput = event.target as HTMLInputElement;
    if (fileInput.files && fileInput.files[0]) {
      const file = fileInput.files[0];

      this.profileService.uploadPhoto(file).subscribe({
        next: (response) => {
          console.log(response); // The response from the backend
          this.loadProfilePicture(); // Reload the profile picture
        },
        error: (err) => {
          this.error = err.message; // Handle the error
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

  loadProfilePicture(): void {
    this.profileService.getProfilePicture().subscribe({
      next: (data) => {
        const reader = new FileReader();
        reader.addEventListener("load", () => {
          this.profileImageUrl = reader.result as string;
        }, false);
        if (data) {
          reader.readAsDataURL(data);
        }
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
