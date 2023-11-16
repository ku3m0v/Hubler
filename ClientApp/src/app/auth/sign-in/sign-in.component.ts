import {Component} from '@angular/core';
import configurl from "../../../assets/config/config.json";
import {Router} from "@angular/router";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {JwtHelperService} from "@auth0/angular-jwt";
import {NgForm} from "@angular/forms";

@Component({
    selector: 'app-sign-in',
    templateUrl: './sign-in.component.html',
    styleUrls: ['./sign-in.component.css']
})
export class SignInComponent {
    invalidLogin?: boolean;
    url = configurl.apiServer.url + 'api/authentication/';

    constructor(private router: Router, private http: HttpClient, private jwtHelper: JwtHelperService) {
    }

    public login = (form: NgForm) => {
        const credentials = JSON.stringify(form.value);
        this.http.post(this.url + "login", credentials, {
            headers: new HttpHeaders({
                "Content-Type": "application/json"
            })
        }).subscribe(response => {
            const token = (<any>response).token;
            localStorage.setItem("jwt", token);
            this.invalidLogin = false;
            this.router.navigate(["/chart"]);
        }, err => {
            this.invalidLogin = true;
            setTimeout(() => {
                this.invalidLogin = false;
            }, 3000);
        });
    }

    isUserAuthenticated() {
        const token = localStorage.getItem("jwt");
        if (token && !this.jwtHelper.isTokenExpired(token)) {
            return true;
        } else {
            return false;
        }
    }
}
