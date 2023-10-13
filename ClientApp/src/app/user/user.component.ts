import { Component, ViewChild, ElementRef } from '@angular/core';
import { FormBuilder, FormGroup } from "@angular/forms";

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent {
  public profileForm: FormGroup;
  public isEditing = false;

  @ViewChild('imageInput', { static: false }) imageInput!: ElementRef;
  constructor(private fb: FormBuilder) {
    this.profileForm = this.fb.group({
      name: ['John Doe'],
      jobTitle: ['Software Engineer'],
      bio: ['John is a software engineer with over 10 years of experience...']
    });
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
}
