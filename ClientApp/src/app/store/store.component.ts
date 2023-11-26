import {Component} from '@angular/core';
import {SupermarketService, SupermarketWithAddress} from "../service/store-service/store.service";
import {Router} from "@angular/router";
import {AuthenticationService} from "../service/auth-service/authentication.service";

@Component({
  selector: 'app-store', templateUrl: './store.component.html', styleUrls: ['./store.component.css']
})
export class StoreComponent {
  public storeList: SupermarketWithAddress[] = [];
  showSpinner = true;
  showButton = false;

  constructor(private router: Router, private supermarketService: SupermarketService, private authService: AuthenticationService,) {
    this.getStores();
    setTimeout(() => {
      this.showSpinner = false;
      this.showButton = true;
    }, 1500);
  }

  getStores() {
    this.supermarketService.getAllSupermarkets().subscribe(data => this.storeList = data, error => console.error(error));
  }

  deleteStore(title: string) {
    const ans = confirm("Do you want to delete the store with title: " + title);
    if (ans) {
      this.supermarketService.deleteSupermarket(title).subscribe(() => this.getStores(), error => console.error(error));
    }
  }

  public isUserAuthenticated(): boolean {
    return this.authService.isUserSignedIn();
  }
}
