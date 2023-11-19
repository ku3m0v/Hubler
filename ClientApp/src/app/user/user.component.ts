import {Component, ElementRef, OnInit, ViewChild} from '@angular/core';
import {FormBuilder, FormGroup} from "@angular/forms";
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
  public profileForm!: FormGroup;


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
        this.profileForm = this.fb.group({
            email: [{value: '', disabled: true}],  // Assuming email is not editable
            firstName: [''],
            lastName: [''],
            createdDate: [{value: '', disabled: true}], // Assuming date is not editable
            supermarketName: ['']
        });
        this.loadProfile();
    }


  toggleEdit() {
    this.isEditing = !this.isEditing;
  }

  saveProfile() {
    if (this.profileForm.valid) {
      this.toggleEdit();
    }
  }

  cancelEdit() {
    this.profileForm.reset(this.profileForm.value);
    this.toggleEdit();
  }

  changeImage() {
    this.imageInput.nativeElement.click();
  }

  onImageChange(event: Event) {
    const file = (event.target as HTMLInputElement).files![0];
    if (file) {
      const reader = new FileReader();
      reader.onload = (e: any) => {
        const imageElement: HTMLImageElement = document.querySelector('img[alt="Profile picture"]') as HTMLImageElement;
        imageElement.src = e.target.result;
      };
      reader.readAsDataURL(file);
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
