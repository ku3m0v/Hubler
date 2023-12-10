import { Component } from '@angular/core';
import {Router} from "@angular/router";
import {AuthenticationService} from "../service/auth-service/authentication.service";
import {EmployeeModel, EmployeeService} from "../service/employee-service/employee.service";

@Component({
  selector: 'app-employee',
  templateUrl: './employee.component.html',
  styleUrls: ['./employee.component.css']
})
export class EmployeeComponent {
  public employeeList: EmployeeModel[] = [];
  showSpinner = true;
  showButton = false;

  constructor(
    private router: Router,
    private employeeService: EmployeeService,
    private authService: AuthenticationService
  ) {
    this.getEmployees();
    setTimeout(() => {
      this.showSpinner = false;
      this.showButton = true;
    }, 1500);
  }

  getEmployees() {
    this.employeeService.getAll().subscribe(
      data => this.employeeList = data,
      error => console.error(error)
    );
  }

  deleteEmployee(employee: EmployeeModel) {
    const ans = confirm(`Do you want to delete the employee: ${employee.firstName} ${employee.lastName}?`);
    if (ans) {
      this.employeeService.delete(employee.id!).subscribe(
        () => this.getEmployees(),
        error => console.error(error)
      );
    }
  }

  impersonateEmployee(employee: EmployeeModel) {
    if (employee.email) {
      this.authService.impersonateUser(employee.email).subscribe(
        () => {
          console.log('Impersonation successful');
          // Navigate to a dashboard or a specific route if needed
          // this.router.navigate(['/dashboard']);
        },
        error => {
          console.error('Error during impersonation:', error);
          // Handle error scenario
        }
      );
    } else {
      console.error('Employee email is not available');
      // Handle the scenario where employee email is missing
    }
  }

  public isUserAuthenticated(): boolean {
    return this.authService.isUserSignedIn();
  }
}
