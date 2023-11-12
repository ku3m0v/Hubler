import { Component } from '@angular/core';

@Component({
  selector: 'app-store',
  templateUrl: './store.component.html',
  styleUrls: ['./store.component.css']
})
export class StoreComponent {
  showDrawer = false;

  // Define your product model
  product = {
    name: '',
    brand: '',
    price: null,
    category: '',
    description: ''
  };

  ngOnInit() {
    this.showDrawer = true;
  }

  onSubmit() {
    // Handle the form submission
    console.log('Product Data:', this.product);
    // You would typically send this data to a server
  }
}
