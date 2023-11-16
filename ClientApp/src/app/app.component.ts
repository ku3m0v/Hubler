import {Component, OnInit} from '@angular/core';
import {initFlowbite} from 'flowbite';
import {AuthenticationService} from "./service/auth-service/authentication.service";

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
    title = 'Hubler';

    constructor(private authService: AuthenticationService) {
    }

    get isSignedIn(): boolean {
        return this.authService.isUserSignedIn();
    }

    ngOnInit(): void {
        initFlowbite();
    }
}
