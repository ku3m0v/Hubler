import { Component, OnInit } from '@angular/core';
import { WarehouseService, WarehouseModel } from '../service/warehouse-service/warehouse.service';

@Component({
  selector: 'app-warehouse',
  templateUrl: './warehouse.component.html',
  styleUrls: ['./warehouse.component.css']
})
export class WarehouseComponent implements OnInit {
  warehouseList: WarehouseModel[] = [];
  supermarketTitles: string[] = [];
  selectedSupermarketTitle: string = '';
  showSpinner = true;
  showMsg = false;

  constructor(private warehouseService: WarehouseService) {
    setTimeout(() => {
      this.showSpinner = false;
      this.showMsg = true;
    }, 1500);
  }

  ngOnInit(): void {
    this.loadWarehouse();
    this.loadSupermarketTitles();
  }

  loadWarehouse(): void {
    this.warehouseService.getAll(this.selectedSupermarketTitle).subscribe({
      next: (data) => {
        this.warehouseList = data;
      },
      error: (error) => {
        console.error('There was an error!', error);
      }
    });
  }

  onSelectSupermarketTitle(title: string): void {
    this.selectedSupermarketTitle = title;
    this.loadWarehouse(); // Reload warehouse based on selected title
  }

  loadSupermarketTitles(): void {
    this.warehouseService.getSupermarketTitles().subscribe({
      next: (data) => {
        this.supermarketTitles = data;
      },
      error: (error) => {
        console.error('Error loading supermarket titles', error);
      }
    });
  }

  deleteWarehouseEntry(warehouseEntry: WarehouseModel): void {
    const confirmation = confirm('Are you sure you want to delete this entry?');
    if (confirmation) {
      this.warehouseService.delete(warehouseEntry.id).subscribe(
        () => {
          this.loadWarehouse();
        },
        error => {
          console.error('Error deleting entry', error);
          this.showMsg = true;
        }
      );
    }
  }

  transferProduct(warehouseEntry: WarehouseModel): void {
    if (!this.selectedSupermarketTitle) {
      alert('Please select a supermarket title.');
      return;
    }

    this.warehouseService.transferProduct(warehouseEntry).subscribe({
      next: () => {
        console.log('Product transferred successfully');
        this.loadWarehouse();
      },
      error: (error) => {
        console.error('There was an error during the transfer!', error);
      }
    });
  }

  orderProducts(): void {
    if (!this.selectedSupermarketTitle) {
      alert('Please select a supermarket title.');
      return;
    }
    this.warehouseService.orderProducts(this.selectedSupermarketTitle).subscribe({
      next: () => {
        console.log('Products ordered successfully');
        this.loadWarehouse(); // Refresh the warehouse list
      },
      error: (error) => {
        console.error('There was an error!', error);
      }
    });
  }
}
