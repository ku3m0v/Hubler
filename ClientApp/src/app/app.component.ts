import { Component } from '@angular/core';
import { OnInit } from '@angular/core';
import { initFlowbite } from 'flowbite';
import {AuthenticationService} from "./authentication.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Hubler';

  ngOnInit(): void {
    initFlowbite();
  }
  constructor(private authService: AuthenticationService) {}

  get isSignedIn(): boolean {
    return this.authService.isUserSignedIn();
  }
}
