import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthenticationService, RegistrationData } from '../../service/auth-service/authentication.service';
import { SupermarketService, SupermarketWithAddress } from '../../service/store-service/store.service';

@Component({
    selector: 'app-sign-up',
    templateUrl: './sign-up.component.html',
    styleUrls: ['./sign-up.component.css']
})
export class SignUpComponent implements OnInit {
    supermarkets: SupermarketWithAddress[] = [];
    errorMessage: string = '';
    isDropdownVisible = false;
    selectedOption: string | null = null;

    constructor(
        private authService: AuthenticationService,
        private storeService: SupermarketService,
        private router: Router
    ) {}

    ngOnInit(): void {
        this.storeService.getAllSupermarkets().subscribe(
            data => {
                this.supermarkets = data;
            },
            error => {
                this.errorMessage = 'Failed to load supermarkets';
                console.error(error);
            }
        );
    }

    updateSelection(option: string) {
        this.selectedOption = option;
        this.isDropdownVisible = false; // Optional: Close dropdown after selection.
    }

    toggleDropdown() {
        this.isDropdownVisible = !this.isDropdownVisible;
    }

    register(form: NgForm) {
        if (form.valid) {
            const registrationData: RegistrationData = {
                email: form.value.email,
                password: form.value.password,
                firstName: form.value.firstName,
                lastName: form.value.lastName,
                supermarketTitle: form.value.supermarketTitle
            };

            this.authService.register(registrationData).subscribe(
                response => {
                    console.log('Registration successful', response);
                    this.router.navigate(['/sign-in']);
                },
                error => {
                    this.errorMessage = 'Registration failed';
                    console.error(error);
                }
            );
        }
    }
}
