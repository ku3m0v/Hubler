import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ProductService, LkProduct } from "../../../service/product-service/product.service";
import { OrderService, ProductOrderModel } from "../../../service/order-service/order.service";
import { Router } from "@angular/router";

@Component({
  selector: 'app-add-order',
  templateUrl: './add-order.component.html',
  styleUrls: ['./add-order.component.css']
})
export class AddOrderComponent implements OnInit {
  orderForm: FormGroup;
  products: LkProduct[] = [];
  selectedProductType: string | null = null;

  constructor(
    private fb: FormBuilder,
    private productService: ProductService,
    private productOrderService: OrderService,
    private router: Router
  ) {
    this.orderForm = this.fb.group({
      productName: ['', Validators.required],
      quantity: [0, [Validators.required, Validators.min(1)]],
      orderDate: [new Date(), Validators.required],
      expireDate: [''],
      storageType: [''],
      shelfLife: [0]
    });
  }

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.productService.getAll().subscribe(
      data => this.products = data,
      error => console.error(error)
    );
  }

  onProductChange(productName: string): void {
    this.selectedProductType = this.determineProductType(productName);
    this.updateFormValidators();
  }

  determineProductType(productName: string): string {
    // Placeholder logic to determine product type
    if (productName.toLowerCase().includes("perishable")) {
      return 'perishable';
    }
    return 'nonperishable';
  }

  updateFormValidators(): void {
    if (this.selectedProductType === 'perishable') {
      this.orderForm.get('expireDate')?.setValidators([Validators.required]);
      this.orderForm.get('storageType')?.setValidators([Validators.required]);
      this.orderForm.get('shelfLife')?.clearValidators();
    } else {
      this.orderForm.get('shelfLife')?.setValidators([Validators.required]);
      this.orderForm.get('expireDate')?.clearValidators();
      this.orderForm.get('storageType')?.clearValidators();
    }

    this.orderForm.get('expireDate')?.updateValueAndValidity();
    this.orderForm.get('storageType')?.updateValueAndValidity();
    this.orderForm.get('shelfLife')?.updateValueAndValidity();
  }

  saveOrder(): void {
    if (this.orderForm.invalid) {
      console.log('Form is invalid');
      return;
    }

    const orderData: ProductOrderModel = {
      ...this.orderForm.value,
      productType: this.selectedProductType
    };
    this.productOrderService.insertOrder(orderData, this.selectedProductType)
      .subscribe({
        next: () => this.router.navigate(['/orders']),
        error: (err) => console.error('Error saving order:', err)
      });
  }

  cancel(): void {
    this.router.navigate(['/orders']);
  }
}
