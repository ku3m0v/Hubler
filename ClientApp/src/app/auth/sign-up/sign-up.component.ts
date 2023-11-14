import { Component } from '@angular/core';
import {AuthenticationService} from "../../service/auth-service/authentication.service";
import {animate, style, transition, trigger} from "@angular/animations";

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.css'],
  animations: [
    trigger('fadeInOut', [
      transition(':enter', [   // :enter is alias to 'void => *'
        style({ opacity: 0 }),
        animate(300, style({ opacity: 1 }))
      ]),
      transition(':leave', [   // :leave is alias to '* => void'
        animate(300, style({ opacity: 0 }))
      ])
    ])
  ]
})
export class SignUpComponent {
  showModal: boolean = false
  showToast: boolean = false;
  isDropdownVisible = false;

  radioButtons = [
    {
      id: 'default-radio-1',
      label: 'Berlin, Alexanderplatz 7, 10178'
    },
    {
      id: 'default-radio-2',
      label: 'Berlin, Steglitz Str. 243, 61500'
    },
    {
      id: 'default-radio-3',
      label: 'Berlin, Bernauer Str. 111, 45300'
    }
  ];

  selectedOption: string | null = null;

  constructor(private authService: AuthenticationService) {}

  toggleDropdown() {
    this.isDropdownVisible = !this.isDropdownVisible;
  }

  updateSelection(option: string) {
    this.selectedOption = option;
    this.isDropdownVisible = false; // Optional: Close dropdown after selection.
  }

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
