import { Component } from '@angular/core';
import {AuthenticationService} from "../../service/auth-service/authentication.service";

@Component({
  selector: 'app-p-manager',
  templateUrl: './p-manager.component.html',
  styleUrls: ['./p-manager.component.css']
})
export class PManagerComponent {

  constructor(private authService: AuthenticationService) { }

  isAdmin(): boolean {
    return this.authService.isAdmin();
  }
}
