import { Component } from '@angular/core';
import {FormBuilder, FormGroup} from "@angular/forms";

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent {
  public profileForm: FormGroup;
  public isEditing = false;

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
      // API call or other logic can go here
      this.toggleEdit();
    }
  }

  cancelEdit() {
    this.profileForm.reset(this.profileForm.value);
    this.toggleEdit();
  }
}
