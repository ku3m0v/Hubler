import {Component} from '@angular/core';
import {AuthenticationService} from "../service/auth-service/authentication.service";

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent {
  isModalVisible = false;
  public sidebarLinks : any[] = [];


  showModal() {
    const ans = confirm("Do you want to sign out?");
    if (ans) {
      this.signOut();
    }
  }

  hideModal() {
    this.isModalVisible = false;
  }

  constructor(private authService: AuthenticationService) {
    if (this.authService.isAdmin()) {
      this.sidebarLinks = [
        {route: '/chart', label: 'Home', imgURL: '/assets/assets/home.svg'},
        {route: '/user', label: 'User', imgURL: '/assets/assets/user.svg'},
        {route: '/stores', label: 'Stores', imgURL: '/assets/assets/store.svg'},
        {route: '/products', label: 'Catalog', imgURL: '/assets/assets/catalog.svg'},
        {route: '/employees', label: 'Employees', imgURL: '/assets/assets/members.svg'},
        {route: '/orders', label: 'Order', imgURL: '/assets/assets/order.svg'},
        {route: '/perishable', imgURL: '/assets/assets/members.svg', name: 'perishable'},
        {route: '/nonperishable', imgURL: '/assets/assets/members.svg', name: 'nonperishable'},
        {route: '/settings', label: 'Warehouses', imgURL: '/assets/assets/settings.svg'},
      ];
    } else if (this.authService.isManager()) {
      this.sidebarLinks = [
        {route: '/chart', label: 'Home', imgURL: '/assets/assets/home.svg'},
        {route: '/user', label: 'User', imgURL: '/assets/assets/user.svg'},
        {route: '/products', label: 'Catalog', imgURL: '/assets/assets/catalog.svg'},
        {route: '/stores', label: 'Stores', imgURL: '/assets/assets/store.svg'},
        {route: '/employees', label: 'Employees', imgURL: '/assets/assets/members.svg'},
        {route: '/orders', label: 'Order', imgURL: '/assets/assets/order.svg'},
        {route: '/perishable', imgURL: '/assets/assets/members.svg', name: 'perishable'},
        {route: '/nonperishable', imgURL: '/assets/assets/members.svg', name: 'nonperishable'},
        {route: '/settings', label: 'Warehouses', imgURL: '/assets/assets/settings.svg'},
      ];
    } else if (this.authService.isCashier()) {
      this.sidebarLinks = [
        {route: '/chart', label: 'Home', imgURL: '/assets/assets/home.svg'},
        {route: '/user', label: 'User', imgURL: '/assets/assets/user.svg'},
        {route: '/cashregisters', label: 'Cashregisters', imgURL: '/assets/assets/check.svg'},
      ];
    }
  }

  get isSignedIn(): boolean {
    return this.authService.isUserSignedIn();
  }

  signOut(): void {
    this.authService.signOut();
  }

  public isActive(linkRoute: string, currentPath: string): boolean {
    return (currentPath.includes(linkRoute) && linkRoute.length > 1) || currentPath === linkRoute;
  }
}
