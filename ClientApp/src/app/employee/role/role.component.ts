import { Component, OnInit } from '@angular/core';
import {RoleData, RoleService} from "../../service/role-service/role.service";

@Component({
  selector: 'app-role',
  templateUrl: './role.component.html',
  styleUrls: ['./role.component.css']
})
export class RoleComponent implements OnInit {
  roles: RoleData[] = [];
  showSpinner = true;
  showButton = false;

  constructor(private roleService: RoleService) {
    this.loadRoles();
    setTimeout(() => {
      this.showSpinner = false;
      this.showButton = true;
    }, 1500);
  }

  ngOnInit(): void {
    this.loadRoles();
  }

  loadRoles(): void {
    this.roleService.getAllRoles().subscribe(
      data => this.roles = data,
      error => console.error('Error fetching roles', error)
    );
  }

  deleteRole(id: number): void {
    const confirmation = confirm('Are you sure you want to delete this role?');
    if (confirmation) {
      this.roleService.deleteRole(id).subscribe(
        () => {
          this.loadRoles();
        },
        error => console.error('Error deleting role', error)
      );
    }
  }
}
