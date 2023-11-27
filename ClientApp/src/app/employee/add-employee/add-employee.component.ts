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

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private employeeService: EmployeeService,
    private authService: AuthenticationService
) {
    this.employeeForm = this.fb.group({
      id: 0, // Include validation as necessary
      email: ['', [Validators.required, Validators.email]],
      password: [''], // Include validation as necessary
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      createdDate: [new Date()], // Default to current date, adjust as necessary
      supermarketName: [''], // Include validation as necessary
      roleName: ['', Validators.required],
      adminId: [0] // Include validation as necessary
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
      () => this.router.navigate(['/employees']),
      (error: any) => console.error(error)
    );
  } else {
    this.employeeService.edit(employeeData).subscribe(
      () => this.router.navigate(['/employees']),
      (error: any) => console.error(error)
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
