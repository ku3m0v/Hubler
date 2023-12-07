import { Component, OnInit } from '@angular/core';
import { InventoryService, InventoryModel } from '../service/inventory-service/inventory.service';
import {ProductOrderModel} from "../service/order-service/order.service";

@Component({
  selector: 'app-inventory',
  templateUrl: './inventory.component.html',
  styleUrls: ['./inventory.component.css']
})
export class InventoryComponent implements OnInit {
  inventoryList: InventoryModel[] = [];
  supermarketTitles: string[] = [];
  selectedSupermarketTitle: string = '';
  showSpinner = true;
  showMsg = false;

  constructor(private inventoryService: InventoryService) {}

  ngOnInit(): void {
    this.loadInventory();
    this.loadSupermarketTitles();
  }

  loadInventory(): void {
    this.inventoryService.getAllInventory().subscribe({
      next: (data) => {
        this.inventoryList = data;
      },
      error: (error) => {
        console.error('There was an error!', error);
      }
    });
  }

  loadSupermarketTitles(): void {
    this.inventoryService.getSupermarketTitles().subscribe({
      next: (data) => {
        this.supermarketTitles = data;
      },
      error: (error) => {
        console.error('Error loading supermarket titles', error);
      }
    });
  }

  deleteInventory(inventory: InventoryModel): void {
    const confirmation = confirm('Are you sure you want to delete this order?');
    if (confirmation) {
      this.inventoryService.deleteInventory(inventory.id!).subscribe(
        () => {
          this.loadInventory();
        },
        error => {
          console.error('Error deleting order', error);
          this.showMsg = true;
        }
      );
    }
  }

  orderProducts(): void {
    if (!this.selectedSupermarketTitle) {
      alert('Please select a supermarket title.');
      return;
    }
    // Logic for ordering products
    this.inventoryService.orderProducts().subscribe({
      next: () => {
        console.log('Products ordered successfully');
        // Handle response for successful product ordering
      },
      error: (error) => {
        console.error('There was an error!', error);
      }
    });
  }
}
