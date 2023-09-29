import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {RouterModule, Routes} from "@angular/router";
import {HomeComponent} from "./home/home.component";
import {CardComponent} from "./card/card.component";
import {SignInComponent} from "./auth/sign-in/sign-in.component";
import {SignUpComponent} from "./auth/sign-up/sign-up.component";
import {LandingComponent} from "./landing/landing.component";



const routes: Routes = [
  { path: '', redirectTo: '/landing', pathMatch: 'full' },
  { path: 'landing', component: LandingComponent },
  { path: 'home', component: HomeComponent },
  { path: 'card', component: CardComponent },
  { path: 'sing-in', component: SignInComponent },
  { path: 'sing-in', component: SignUpComponent },
];

@NgModule({
  imports: [ RouterModule.forRoot(routes) ],
  exports: [ RouterModule ]
})
export class AppRoutingModule {}
