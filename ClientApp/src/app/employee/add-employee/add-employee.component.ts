import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {ActivatedRoute, Router} from "@angular/router";
import {EmployeeModel, EmployeeService} from "../../service/employee-service/employee.service";
import {AuthenticationService} from "../../service/auth-service/authentication.service";

@Component({
  selector: 'app-add-employee',
  templateUrl: './add-employee.component.html',
  styleUrls: ['./add-employee.component.css']
})
export class AddEmployeeComponent implements OnInit {
  employeeForm: FormGroup;
  title: string = 'Add';
  managers: EmployeeModel[] = [];
  supermarketTitles: string[] = [];
  roles: string[] = [];
  message: string = '';

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private employeeService: EmployeeService,
    private authService: AuthenticationService
) {
    this.employeeForm = this.fb.group({
      id: 0,
      email: ['', [Validators.required, Validators.email]],
      password: [''],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      createdDate: [new Date()],
      supermarketName: [''],
      roleName: ['', Validators.required],
      adminId: 0
    });
  }

  ngOnInit(): void {
    const employeeEmail = this.route.snapshot.paramMap.get('email');
    if (employeeEmail) {
      this.title = 'Edit';
      this.employeeService.getDetails(employeeEmail).subscribe(
        (data: EmployeeModel) => {
          this.employeeForm.patchValue(data);
        },
        (error: any) => console.error(error)
      );
    }
    this.loadManagers();
    this.loadSupermarketTitles();
    this.loadRoles();
  }

  loadManagers() {
    this.employeeService.getManagers().subscribe(data => {
      this.managers = data;
    }, error => console.error(error));
  }

  loadSupermarketTitles() {
    this.employeeService.getSupermarketTitles().subscribe(data => {
      this.supermarketTitles = data;
    }, error => console.error(error));
  }

  loadRoles() {
    this.employeeService.getRoles().subscribe(data => {
      this.roles = data;
    }, error => console.error(error));
  }

  saveEmployee(): void {
    if (this.employeeForm.invalid) {
    return;
  }

  const employeeData: EmployeeModel = this.employeeForm.value;
  if (this.title === 'Add') {
    this.employeeService.insert(employeeData).subscribe(
      () => {
        this.message = 'Employee been added successfully! Please, wait...';
        setTimeout(() => {
          this.router.navigate(['/employees']);
        }, 1500);
      },
      (error: any) => {
        setTimeout(() => {
          this.message = 'Failed to add!';
        }, 1500);
        console.error(error)
      }
    );
  } else {
    this.employeeService.edit(employeeData).subscribe(
      () => {
        this.message = 'Employee been updated successfully! Please, wait...';
        setTimeout(() => {
          this.router.navigate(['/employees']);
        }, 1500);
      },
      (error: any) => {
        setTimeout(() => {
          this.message = 'Failed to update!';
        }, 1500);
        console.error(error)
      }
    );
  }
}

  cancel(): void {
    this.router.navigate(['/employees']);
  }

public isUserAuthenticated(): boolean {
    return this.authService.isUserSignedIn();
  }
}
