import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, NgForm, Validators} from '@angular/forms';
import {Router} from '@angular/router';
import {AuthenticationService, RegistrationData} from '../../service/auth-service/authentication.service';
import {SupermarketService, SupermarketWithAddress} from '../../service/store-service/store.service';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.css']
})
export class SignUpComponent implements OnInit {
  signUpForm: FormGroup;
  supermarketTitles: string[] = [];
  errorMessage: string = '';
  isDropdownVisible = false;
  selectedOption: string | null = null;

  constructor(
    private authService: AuthenticationService,
    private storeService: SupermarketService,
    private fb: FormBuilder,
    private router: Router
  ) {
    this.signUpForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      supermarketTitle: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.authService.getSupermarketTitles().subscribe(
      titles => {
        this.supermarketTitles = titles;
      },
      error => {
        this.errorMessage = 'Failed to load supermarket titles';
        console.error(error);
      }
    );
  }

  updateSelection(option: string) {
    this.selectedOption = option;
    this.isDropdownVisible = false;
  }

  toggleDropdown() {
    this.isDropdownVisible = !this.isDropdownVisible;
  }

  register() {
    if (this.signUpForm.valid) {
      const registrationData: RegistrationData = {
        email: this.signUpForm.value.email,
        password: this.signUpForm.value.password,
        firstName: this.signUpForm.value.firstName,
        lastName: this.signUpForm.value.lastName,
        supermarketTitle: this.signUpForm.value.supermarketTitle
      };

      this.authService.register(registrationData).subscribe(
        response => {
          console.log('Registration successful', response);
          this.router.navigate(['/sign-in']);
        },
        error => {
          this.errorMessage = 'Registration failed: ' + error.message;
          console.error(error);
        }
      );
    } else {
      Object.keys(this.signUpForm.controls).forEach(field => {
        const control = this.signUpForm.get(field);
        control?.markAsTouched({ onlySelf: true });
      });
    }
  }
}
