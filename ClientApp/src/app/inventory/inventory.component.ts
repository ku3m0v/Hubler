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
  messageContent: string = '';
  showMessage: boolean = false;

  constructor(private inventoryService: InventoryService) {
    setTimeout(() => {
      this.showSpinner = false;
      this.showMsg = true;
    }, 1500);
  }

  ngOnInit(): void {
    this.loadInventory();
    this.loadSupermarketTitles();
  }

  loadInventory(): void {
    this.inventoryService.getAllInventory(this.selectedSupermarketTitle).subscribe({
      next: (data) => {
        this.inventoryList = data;
      },
      error: (error) => {
        console.error('There was an error!', error);
      }
    });
  }

  // Method to update the selected supermarket title
  onSelectSupermarketTitle(title: string): void {
    this.selectedSupermarketTitle = title;
    this.loadInventory(); // Reload inventory based on selected title
  }

  loadSupermarketTitles(): void {
    this.inventoryService.getSupermarketTitles().subscribe({
      next: (data) => {
        this.supermarketTitles = data;
        if (this.supermarketTitles.length === 1) {
          this.selectedSupermarketTitle = this.supermarketTitles[0];
          this.onSelectSupermarketTitle(this.selectedSupermarketTitle);
        }
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
      this.showMessageWithTimeout('Please select a supermarket title.');
      return;
    }
    this.inventoryService.orderProducts(this.selectedSupermarketTitle).subscribe({
      next: () => {
        this.showMessageWithTimeout('Products ordered successfully');
        console.log('Products ordered successfully');
        // Additional logic after successful order
      },
      error: (error) => {
        this.showMessageWithTimeout('There was an error processing your order.');
        console.error('There was an error!', error);
      }
    });
  }

  showMessageWithTimeout(message: string) {
    this.messageContent = message;
    this.showMessage = true;
    setTimeout(() => {
      this.showMessage = false;
    }, 1500); // Hide the message after 1.5 seconds
  }

}
