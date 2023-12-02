import {Component} from '@angular/core';
import {AuthenticationService} from "../service/auth-service/authentication.service";

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent {
  isModalVisible = false;

  showModal() {
    const ans = confirm("Do you want to sign out?");
    if (ans) {
      this.signOut();
    }
  }

  hideModal() {
    this.isModalVisible = false;
  }

  public sidebarLinks = [
    {route: '/chart', label: 'Home', imgURL: '/assets/assets/home.svg'},
    {route: '/user', label: 'User', imgURL: '/assets/assets/user.svg'},
    {route: '/stores', label: 'Stores', imgURL: '/assets/assets/store.svg'},
    {route: '/employees', label: 'Employees', imgURL: '/assets/assets/members.svg'},
    {route: '/roles', label: 'Roles', imgURL: '/assets/assets/role.svg'},
    {route: '/cashregisters', label: 'Cashregisters', imgURL: '/assets/assets/check.svg'},
    {route: '/statuses', label: 'Statuses', imgURL: '/assets/assets/flag.svg'},
    {route: '/logs', label: 'Logs', imgURL: '/assets/assets/logs.svg'},
  ];

  constructor(private authService: AuthenticationService) {
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
