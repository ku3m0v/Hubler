import {AfterViewInit, Component, ElementRef, OnInit} from '@angular/core';
import {AuthenticationService} from "../service/auth-service/authentication.service";
import {
  ExpiredInventory,
  ExpiredWarehouse,
  SupermarketSalesSummary,
  Top5ProductsBySupermarket,
  ViewsService
} from "../service/view-service/views.service";
import {FormControl} from "@angular/forms";

declare var ApexCharts: any;

interface Product {
  imageUrl: string;
  id: number;
  title: string;
  date?: string;
  soldQuantity?: number;
}

@Component({
  selector: 'app-chart',
  templateUrl: './chart.component.html',
  styleUrls: ['./chart.component.css']
})
export class ChartComponent implements OnInit {
  expiredInventories: ExpiredInventory[] = [];
  expiredWarehouses: ExpiredWarehouse[] = [];
  top5Products: Top5ProductsBySupermarket[] = [];
  salesSummaries: SupermarketSalesSummary[] = [];
  supermarketTitles: string[] = [];
  selectedSupermarketTitle: string = '';
  loading = true;
  showSpinner = true;
  showMsg = false;
  private options: any;

  constructor(private elRef: ElementRef, private authService: AuthenticationService, private viewsService: ViewsService) {
  }

  ngOnInit(): void {
    this.fetchSupermarketTitles();
  }

  fetchSupermarketTitles(): void {
    this.viewsService.getSupermarketTitles().subscribe({
      next: (data) => {
        this.supermarketTitles = data;
        if (this.supermarketTitles.length === 1) {
          this.selectedSupermarketTitle = this.supermarketTitles[0];
          this.onSelectSupermarketTitle(this.selectedSupermarketTitle);
        }
      },
      error: (error) => {
        console.error('Error loading supermarket titles', error);
        // this.showMessage = true;
        // this.messageContent = 'Error loading supermarket titles.';
      }
    });
  }

  onSelectSupermarketTitle(title: string): void {
    this.selectedSupermarketTitle = title;
    this.fetchData();
  }

  fetchData(): void {
    if (!this.selectedSupermarketTitle) {
      console.error('Supermarket title is required');
      return;
    }

    this.viewsService.getExpiredInventory(this.selectedSupermarketTitle).subscribe(
      (data: ExpiredInventory[]) => {
        this.expiredInventories = data;
        this.showSpinner = false;
      },
      (error) => console.error('Error fetching expired inventories:', error)
    );

    this.viewsService.getExpiredWarehouse(this.selectedSupermarketTitle).subscribe(
      (data: ExpiredWarehouse[]) => {
        this.expiredWarehouses = data;
        this.showSpinner = false;
      },
      (error) => console.error('Error fetching expired warehouses:', error)
    );

    this.viewsService.getTop5ProductsBySupermarket(this.selectedSupermarketTitle).subscribe(
      (data: Top5ProductsBySupermarket[]) => {
        this.top5Products = data;
        this.showSpinner = false;
      },
      (error) => console.error('Error fetching top 5 products:', error)
    );

    this.viewsService.getSupermarketSalesSummary(this.selectedSupermarketTitle).subscribe(
      (data: SupermarketSalesSummary[]) => {
        this.salesSummaries = data;
        this.showSpinner = false;
      },
      (error) => {
        console.error('Error fetching sales summaries:', error);
        this.loading = false;
      }
    );
  }

  protected readonly HTMLBodyElement = HTMLBodyElement;
}
