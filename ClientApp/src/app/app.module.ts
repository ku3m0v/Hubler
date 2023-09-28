import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import {FooterComponent} from "./footer/footer.component";
import {SidebarComponent} from "./sidebar/sidebar.component";
import {HomeComponent} from "./home/home.component";
import {CardComponent} from "./card/card.component";
import {SignInComponent} from "./auth/sign-in/sign-in.component";
import {SignUpComponent} from "./auth/sign-up/sign-up.component";
import {NavComponent} from "./nav/nav.component";


@NgModule({
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    AppRoutingModule
  ],
  declarations: [
    AppComponent,
    SidebarComponent,
    HomeComponent,
    CardComponent,
    SignInComponent,
    SignUpComponent,
    FooterComponent,
    NavComponent,
  ],
  bootstrap: [ AppComponent ]
})
export class AppModule { }
