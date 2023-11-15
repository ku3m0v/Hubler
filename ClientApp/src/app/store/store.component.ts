import {Component} from '@angular/core';
import {SupermarketService, SupermarketWithAddress} from "../service/store-service/store.service";
import {Router} from "@angular/router";

@Component({
    selector: 'app-store',
    templateUrl: './store.component.html',
    styleUrls: ['./store.component.css']
})
export class StoreComponent {
    public storeList: SupermarketWithAddress[] = [];

    constructor(private router: Router, private supermarketService: SupermarketService) {
        this.getStores();
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
    id: number;
    title: string;
    phone: string;
    street: string;
    house: string;
    city: string;
    postalCode: string;
    country: string;
}
