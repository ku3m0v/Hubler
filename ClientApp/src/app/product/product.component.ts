import { Component, OnInit } from '@angular/core';
import { LkProduct, ProductService } from "../service/product-service/product.service";

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductComponent implements OnInit {
  products: LkProduct[] = [];
  showSpinner = true;
  showMsg = false;

  constructor(private productService: ProductService) { }

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts() {
    this.productService.getAll().subscribe(
      data => {
        this.products = data;
        this.showSpinner = false;
        },
      error => {
        console.error(error);
        this.showSpinner = false;
        this.showMsg = true;
      }
    );
  }

  delete(id: number): void {
    const confirmation = confirm('Are you sure you want to delete this product?');
    if (confirmation) {
      this.productService.delete(id).subscribe(
        () => {
          this.loadProducts();
        },
        error => {
          console.error('Error deleting product', error);
          this.showMsg = true;
        }
      );
    }
  }
}
