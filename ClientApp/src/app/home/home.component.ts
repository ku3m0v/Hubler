import {Component} from '@angular/core';
import {AuthenticationService} from "../service/auth-service/authentication.service";

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.css']
})
export class HomeComponent {
    showLoading: boolean = true;
    showGallery: boolean = false;

    constructor(private authService: AuthenticationService) {
    }

    get isSignedIn(): boolean {
        return this.authService.isUserSignedIn();
    }

    ngOnInit() {
        setTimeout(() => {
            this.showLoading = false;
            this.showGallery = true;
        }, 1000);
    }

    signOut(): void {
        this.authService.signOut();
    }
}
