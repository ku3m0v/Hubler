import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {HttpClientModule} from '@angular/common/http';

import {AppComponent} from './app.component';
import {AppRoutingModule} from './app-routing.module';
import {SidebarComponent} from "./sidebar/sidebar.component";
import {HomeComponent} from "./home/home.component";
import {SignInComponent} from "./auth/sign-in/sign-in.component";
import {NavComponent} from "./nav/nav.component";
import {LandingComponent} from "./landing/landing.component";
import {SignUpComponent} from "./auth/sign-up/sign-up.component";
import {UserComponent} from "./user/user.component";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {ChartComponent} from "./chart/chart.component";
import {NgApexchartsModule} from "ng-apexcharts";
import {AuthGuard} from "./guards/auth-guard.service";
import {RouterModule, Routes} from "@angular/router";
import {LoginComponent} from "./login/login.component";
import { JwtModule } from "@auth0/angular-jwt";

const routes: Routes = [
  { path: 'chart', component: ChartComponent, canActivate: [AuthGuard] },
  { path: 'login', component: LoginComponent },
];
//function is use to get jwt token from local storage
export function tokenGetter() {
  return localStorage.getItem("jwt");
}

@NgModule({
  imports: [
    BrowserModule.withServerTransition({appId: 'ng-cli-universal'}),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    NgApexchartsModule,
    RouterModule.forRoot(routes),
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: ["localhost:7234"],
        disallowedRoutes: []
      }
    }),
  ],
  providers: [AuthGuard],
  declarations: [
    AppComponent,
    SidebarComponent,
    UserComponent,
    HomeComponent,
    SignInComponent,
    SignUpComponent,
    NavComponent,
    LandingComponent,
    ChartComponent,
    LoginComponent
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
