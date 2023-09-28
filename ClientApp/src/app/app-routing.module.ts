import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {RouterModule, Routes} from "@angular/router";
import {HomeComponent} from "./home/home.component";
import {CardComponent} from "./card/card.component";
import {SignInComponent} from "./auth/sign-in/sign-in.component";
import {SignUpComponent} from "./auth/sign-up/sign-up.component";



const routes: Routes = [
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
