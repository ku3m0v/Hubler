import { Component } from '@angular/core';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent {
  public sidebarLinks = [
    { route: '/home', label: 'Home', imgURL: '/assets/assets/home.svg' },
    { route: '/sing-in', label: 'User', imgURL: '/assets/assets/user.svg' },
    { route: '/card', label: 'Warehouse', imgURL: '/assets/assets/store.svg' },
    { route: '/about', label: 'About', imgURL: '/assets/assets/csv.svg' },
  ];

  public isActive(linkRoute: string, currentPath: string): boolean {
    return (currentPath.includes(linkRoute) && linkRoute.length > 1) || currentPath === linkRoute;
  }
}
