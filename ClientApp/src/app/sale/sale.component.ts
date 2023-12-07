import { Component, OnInit } from '@angular/core';
import { SaleService, SaleModel, Product } from '../service/sale-service/sale.service';

@Component({
  selector: 'app-sale',
  templateUrl: './sale.component.html',
  styleUrls: ['./sale.component.css']
})
export class SaleComponent implements OnInit {
  sales: SaleModel[] = [];
  products: Product[] = [];
  supermarketTitles: string[] = [];
  selectedSupermarketTitle: string = '';
  showSpinner: boolean = true;
  showMessage: boolean = false;
  messageContent: string = '';

  constructor(private saleService: SaleService) {
    setTimeout(() => {
      this.showSpinner = false;
      this.showMessage = true;
    }, 1500);
  }

  ngOnInit(): void {
    this.loadSupermarketTitles();
  }

  loadSales(): void {
    if (this.selectedSupermarketTitle) {
      this.saleService.getAllSales(this.selectedSupermarketTitle).subscribe({
        next: (data) => {
          this.sales = data;
        },
        error: (error) => {
          console.error('Error loading sales', error);
          this.showMessage = true;
          this.messageContent = 'Error loading sales data.';
        }
      });
    }
  }

  onSelectSupermarketTitle(title: string): void {
    this.selectedSupermarketTitle = title;
    this.loadSales();
    this.loadProducts();
  }

  loadProducts(): void {
    this.saleService.getProducts(this.selectedSupermarketTitle).subscribe({
      next: (data) => {
        this.products = data;
      },
      error: (error) => {
        console.error('Error loading products', error);
        this.showMessage = true;
        this.messageContent = 'Error loading product data.';
      }
    });
  }

  loadSupermarketTitles(): void {
    this.saleService.getSupermarketTitles().subscribe({
      next: (data) => {
        this.supermarketTitles = data;
      },
      error: (error) => {
        console.error('Error loading supermarket titles', error);
        this.showMessage = true;
        this.messageContent = 'Error loading supermarket titles.';
      }
    });
  }

  deleteSale(sale: SaleModel): void {
    const confirmation = confirm('Are you sure you want to delete this sale?');
    if (confirmation) {
      this.saleService.deleteSale(sale.saleId).subscribe({
        next: () => {
          this.loadSales();
          this.showMessage = true;
          this.messageContent = 'Sale deleted successfully.';
        },
        error: (error) => {
          console.error('Error deleting sale', error);
          this.showMessage = true;
          this.messageContent = 'Error deleting sale.';
        }
      });
    }
  }

  // Additional methods such as addSale, updateSale, etc., can be added here
}
