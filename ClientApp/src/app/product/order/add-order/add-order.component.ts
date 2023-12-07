import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {Router} from '@angular/router';
import {LkProduct} from "../../../service/product-service/product.service";
import {OrderService, ProductOrderModel} from "../../../service/order-service/order.service";

@Component({
  selector: 'app-add-order',
  templateUrl: './add-order.component.html',
  styleUrls: ['./add-order.component.css']
})
export class AddOrderComponent implements OnInit {
  orderForm: FormGroup;
  productTypes: string[] = ['perishable', 'nonperishable'];
  products: LkProduct[] = [];
  supermarketTitles: string[] = [];

  constructor(
    private fb: FormBuilder,
    private orderService: OrderService,
    private router: Router
  ) {
    this.orderForm = this.fb.group({
      supermarketName: ['', Validators.required],
      productName: ['', Validators.required],
      expireDate: [''],
      storageType: [''],
      shelfLife: [1],
      quantity: [1, [Validators.required, Validators.min(1)]],
      orderDate: ['', Validators.required],
      productType: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.loadProducts();
    this.loadSupermarketTitles();

    this.orderForm.get('productType')?.valueChanges.subscribe((type) => {
      this.updateFormValidations(type);
    });
  }

  updateFormValidations(productType: string): void {
    if (productType === 'perishable') {
      this.orderForm.get('expireDate')?.setValidators(Validators.required);
      this.orderForm.get('storageType')?.setValidators(Validators.required);
    } else {
      this.orderForm.get('expireDate')?.clearValidators();
      this.orderForm.get('storageType')?.clearValidators();
    }
    this.orderForm.get('expireDate')?.updateValueAndValidity();
    this.orderForm.get('storageType')?.updateValueAndValidity();
  }

  loadProducts() {
    this.orderService.getProducts().subscribe(data => {
      this.products = data;
    }, error => console.error(error));
  }

  loadSupermarketTitles() {
    this.orderService.getSupermarketTitles().subscribe(data => {
      this.supermarketTitles = data;
    }, error => console.error(error));
  }

  saveOrder(): void {
    if (this.orderForm.invalid) {
      return;
    }

    let orderData: ProductOrderModel = this.orderForm.value;

    if (orderData.productType === 'nonperishable') {
      const {expireDate, storageType, ...rest} = orderData;
      orderData = rest;
    }

    this.orderService.insertOrder(orderData, orderData.productType).subscribe(
      () => this.router.navigate(['/orders']),
      (error: any) => console.error(error)
    );
  }

  cancel(): void {
    this.router.navigate(['/orders']);
  }
}
