import {Component, OnInit} from '@angular/core';
import {PerishableProduct, PerishableService} from "../../service/product-service/perishable.service";
import {EmployeeModel} from "../../service/employee-service/employee.service";

@Component({
  selector: 'app-perishable',
  templateUrl: './perishable.component.html',
  styleUrls: ['./perishable.component.css']
})
export class PerishableComponent {
  perishableProducts: PerishableProduct[] = [];
  showSpinner = true;
  showMsg = false;

  constructor(private perishableProductService: PerishableService) {
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
      data => this.perishableProducts = data,
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
