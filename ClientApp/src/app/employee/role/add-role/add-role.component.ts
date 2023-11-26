import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import {RoleData, RoleService} from "../../../service/role-service/role.service";
import {AuthenticationService} from "../../../service/auth-service/authentication.service";

@Component({
  templateUrl: './add-role.component.html'
})
export class AddRoleComponent implements OnInit {
  roleForm: FormGroup;
  title: string = 'Add';
  roleName: string | null = null;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private roleService: RoleService,
    private authService: AuthenticationService
  ) {
    this.roleForm = this.fb.group({
      roleName: ['', Validators.required],
    });
  }

  ngOnInit(): void {
    this.roleName = this.route.snapshot.paramMap.get('roleName');
    if (this.roleName) {
      this.title = 'Edit';
      this.roleService.getRoleDetails(this.roleName).subscribe(
        (role: RoleData) => {
          this.roleForm.patchValue(role);
        },
        (error: any) => console.error(error)
      );
    }
  }

  saveRole(): void {
    if (this.roleForm.invalid) {
      return;
    }

    const roleData: RoleData = this.roleForm.value;
    if (this.title === 'Add') {
      this.roleService.createRole(roleData).subscribe(
        () => this.router.navigate(['/roles']),
        (error: any) => console.error(error)
      );
    } else {
      this.roleService.updateRole(roleData).subscribe(
        () => this.router.navigate(['/roles']),
        (error: any) => console.error(error)
      );
    }
  }

  cancel(): void {
    this.router.navigate(['/roles']);
  }

  public isUserAuthenticated(): boolean {
    return this.authService.isUserSignedIn();
  }
}
