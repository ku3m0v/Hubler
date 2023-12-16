import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
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
  title: string = 'Add';
  successMessage: string = '';

  constructor(
    private fb: FormBuilder,
    private saleService: SaleService,
    private router: Router,
  private route: ActivatedRoute
  ) {
    const today = new Date().toISOString().split('T')[0];

    this.saleForm = this.fb.group({
      saleId: 0,
      supermarketName: ['', Validators.required],
      productId: ['', Validators.required],
      quantitySold: [1, [Validators.required, Validators.min(1)]],
      saleDate: [{value: '', disabled: true}, Validators.required]
    });
  }

  ngOnInit(): void {
    this.loadSupermarketTitles();
    this.route.queryParams.subscribe(params => {
      if (params['supermarket']) {
        this.saleForm.get('supermarketName')?.setValue(params['supermarket']);
        this.loadProducts(params['supermarket']);
      }
    });
    this.loadSupermarketTitles();
    const today = new Date().toISOString().split('T')[0];
    this.saleForm.get('saleDate')?.setValue(today);
  }

  loadSupermarketTitles(): void {
    this.saleService.getSupermarketTitles().subscribe(data => {
      this.supermarketTitles = data;
      // Check if supermarketName is already set before setting it again
      if (this.supermarketTitles.length === 1 && !this.saleForm.get('supermarketName')?.value) {
        this.saleForm.get('supermarketName')?.setValue(this.supermarketTitles[0]);
        this.loadProducts(this.supermarketTitles[0]);
      }
    }, error => console.error(error));
  }


  loadProducts(supermarketTitle: string): void {
    if (supermarketTitle) {
      this.saleService.getProducts(supermarketTitle).subscribe(data => {
        this.products = data;
      }, error => console.error(error));
    } else {
      this.products = [];
    }
  }


  saveSale(): void {
    if (this.saleForm.invalid) {
      return;
    }

    const saleData: SaleModel = this.saleForm.value;

    this.saleService.insertSale(saleData).subscribe(
      () => {
        this.successMessage = 'Sale added successfully! Please, wait...';
        setTimeout(() => {
          this.router.navigate(['/sales']);
        }, 1500);
      },
      (error: any) => {
        console.error(error);
        this.successMessage = 'Adding failed!';
      }
    );
  }


  cancel(): void {
    this.router.navigate(['/sales']);
  }
}
