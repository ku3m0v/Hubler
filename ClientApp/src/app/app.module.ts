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
    FooterComponent,
  ],
  bootstrap: [ AppComponent ]
})
export class AppModule { }
