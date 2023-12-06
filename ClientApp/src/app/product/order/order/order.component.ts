import {Component, OnInit} from '@angular/core';
import {OrderService, ProductOrderModel} from "../../../service/order-service/order.service";

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.css']
})
export class OrderComponent implements OnInit {
  productOrders: ProductOrderModel[] = [];
  showSpinner = true;
  showMsg = false;

  constructor(private productOrderService: OrderService) {}

  ngOnInit(): void {
    this.loadProductOrders();
  }

  loadProductOrders() {
    this.productOrderService.getAllOrders().subscribe(
      data => {
        this.productOrders = data;
        this.showSpinner = false;
      },
      error => {
        console.error(error);
        this.showSpinner = false;
      }
    );
  }

  delete(order: ProductOrderModel): void {
    const confirmation = confirm('Are you sure you want to delete this order?');
    if (confirmation) {
      this.productOrderService.deleteOrder(order.id!).subscribe(
        () => {
          this.loadProductOrders();
        },
        error => {
          console.error('Error deleting order', error);
          this.showMsg = true;
        }
      );
    }
  }
}
