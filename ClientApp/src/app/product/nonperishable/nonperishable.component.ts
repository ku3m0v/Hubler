import {Component, OnInit} from '@angular/core';
import {NonPerishableProductModel, NonperishableService} from "../../service/product-service/nonperishable.service";

@Component({
  selector: 'app-nonperishable',
  templateUrl: './nonperishable.component.html',
  styleUrls: ['./nonperishable.component.css']
})
export class NonperishableComponent implements OnInit {
  nonperishableProducts: NonPerishableProductModel[] = [];
  showSpinner = true;
  showMsg = false;

  constructor(private perishableProductService: NonperishableService) {
    this.loadProducts();
  }

  ngOnInit(): void {
    this.loadProducts();
    setTimeout(() => {
      this.showSpinner = false;
      this.showMsg = true;
    }, 1500);
  }

  loadProducts() {
    this.perishableProductService.getAll().subscribe(
      data => this.nonperishableProducts = data,
      error => console.error(error)
    );
  }

  delete(id: number): void {
    const confirmation = confirm('Are you sure you want to delete this product?');
    if (confirmation) {
      this.perishableProductService.delete(id).subscribe(
        () => {
          this.loadProducts();
        },
        error => console.error('Error deleting product', error)
      );
    }
  }
}
