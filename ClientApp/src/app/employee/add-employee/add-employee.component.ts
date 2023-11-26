import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {ActivatedRoute, Router} from "@angular/router";
import {Employee, EmployeeService} from "../../service/employee-service/employee.service";
import {AuthenticationService} from "../../service/auth-service/authentication.service";

@Component({
  selector: 'app-add-employee',
  templateUrl: './add-employee.component.html',
  styleUrls: ['./add-employee.component.css']
})
export class AddEmployeeComponent implements OnInit{
  employeeForm: FormGroup;
  supermarketTitles: string[] = [];
  errorMessage: string = '';
  title: string = 'Add';
  employeeId: number | null = null;
  isDropdownVisible = false;
  selectedMarketTitle: string | null = null;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private employeeService: EmployeeService,
    private authService: AuthenticationService
  ) {
    this.employeeForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      supermarketTitle: ['', Validators.required],
      role: ['', Validators.required],
      adminId: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.employeeId = Number(this.route.snapshot.paramMap.get('id'));
    if (this.employeeId) {
      this.title = 'Edit';
      this.employeeService.getEmployeeDetails(this.employeeId).subscribe(
        (data: Employee) => {
          this.employeeForm.patchValue(data);
        },
        (error: any) => console.error(error)
      );
    }
    this.authService.getSupermarketTitles().subscribe(
      titles => {
        this.supermarketTitles = titles;
        console.log('Supermarket Titles:', this.supermarketTitles);
      },
      error => {
        this.errorMessage = 'Failed to load supermarket titles';
        console.error(error);
      }
    );
  }

  toggleDropdown() {
    this.isDropdownVisible = !this.isDropdownVisible;
  }

  updateSelection(title: string) {
    this.selectedMarketTitle = title;
    this.employeeForm.get('supermarketTitle')?.setValue(title);
    this.toggleDropdown();
  }


  saveEmployee(): void {
    if (this.employeeForm.invalid) {
      return;
    }

    const employeeData: Employee = this.employeeForm.value;
    if (this.title === 'Add') {
      this.employeeService.addEmployee(employeeData).subscribe(
        () => this.router.navigate(['/employees']),
        (error: any) => console.error(error)
      );
    } else {
      this.employeeService.updateEmployee(employeeData).subscribe(
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
