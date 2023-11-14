  import {Component, OnInit} from '@angular/core';
  import {SupermarketService, SupermarketWithAddress} from "../service/store-service/store.service";
  import {Router} from "@angular/router";
  import {AuthenticationService} from "../service/auth-service/authentication.service";

  @Component({
    selector: 'app-store',
    templateUrl: './store.component.html',
    styleUrls: ['./store.component.css']
  })
  export class StoreComponent implements OnInit {
    public storeList: SupermarketWithAddress[] = [];
    public userClaims: any; // To store user claims

    constructor(
      private router: Router,
      private supermarketService: SupermarketService,
      private authService: AuthenticationService
    ) {}

    ngOnInit(): void {
      this.getStores();
      this.retrieveUserClaims();
    }

    retrieveUserClaims(): void {
      this.userClaims = this.authService.getCurrentUserClaims();
      // Now you have access to userClaims throughout the component
      console.log(this.userClaims);
    }

    getStores() {
      this.supermarketService.getAllSupermarkets().subscribe(
        data => this.storeList = data,
        error => console.error(error)
      );
    }

    deleteStore(title: string) {
      const ans = confirm("Do you want to delete the store with title: " + title);
      if (ans) {
        this.supermarketService.deleteSupermarket(title).subscribe(
          () => this.getStores(),
          error => console.error(error)
        );
      }
    }
  }

  // Update this interface to match the structure of your store data
  interface StoreData {
    title: string;
    phone: string;
    street: string;
    house: string;
    city: string;
    postalCode: string;
    country: string;
  }
