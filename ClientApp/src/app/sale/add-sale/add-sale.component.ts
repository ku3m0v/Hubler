import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { SaleService, SaleModel, Product } from '../../service/sale-service/sale.service';

@Component({
  selector: 'app-add-sale',
  templateUrl: './add-sale.component.html',
  styleUrls: ['./add-sale.component.css']
})
export class AddSaleComponent implements OnInit {
  saleForm: FormGroup;
  products: Product[] = [];
  supermarketTitles: string[] = [];

  constructor(
    private fb: FormBuilder,
    private saleService: SaleService,
    private router: Router
  ) {
    this.saleForm = this.fb.group({
      supermarketName: ['', Validators.required],
      productId: ['', Validators.required],
      quantitySold: [1, [Validators.required, Validators.min(1)]],
      saleDate: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.loadSupermarketTitles();
  }

  loadSupermarketTitles(): void {
    this.saleService.getSupermarketTitles().subscribe(data => {
      this.supermarketTitles = data;
    }, error => console.error(error));
  }

  onSelectSupermarketTitle(title: string): void {
    this.saleForm.get('supermarketName')?.setValue(title);
    this.loadProducts(title);
  }

  loadProducts(supermarketTitle: string): void {
    this.saleService.getProducts(supermarketTitle).subscribe(data => {
      this.products = data;
    }, error => console.error(error));
  }

  saveSale(): void {
    if (this.saleForm.invalid) {
      return;
    }

    const saleData: SaleModel = this.saleForm.value;

    this.saleService.insertSale(saleData).subscribe(
      () => this.router.navigate(['/sales']),
      (error: any) => console.error(error)
    );
  }

  cancel(): void {
    this.router.navigate(['/sales']);
  }
}
