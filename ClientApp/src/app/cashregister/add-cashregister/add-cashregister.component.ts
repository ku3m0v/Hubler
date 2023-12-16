import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CashRegisterService, CashRegisterData, Employee } from '../../service/cashregister-service/cashregister.service';
import { AuthenticationService } from "../../service/auth-service/authentication.service";

@Component({
  selector: 'app-add-cash-register',
  templateUrl: './add-cashregister.component.html',
  styleUrls: ['./add-cashregister.component.css']
})
export class AddCashregisterComponent implements OnInit {
  cashRegisterForm: FormGroup;
  title: string = 'Add';
  statuses: string[] = [];
  supermarketTitles: string[] = [];
  employees: Employee[] = [];
  message: string = '';

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private cashRegisterService: CashRegisterService,
    private authService: AuthenticationService
  ) {
    this.cashRegisterForm = this.fb.group({
      id: 0,
      supermarketName: ['', Validators.required], // Include validation as necessary
      registerNumber: [null, Validators.required],
      statusName: ['', Validators.required],
      employeeId: 0
    });
  }

  ngOnInit(): void {
    const supermarketName = this.route.snapshot.paramMap.get('supermarketName');
    const registerNumber = this.route.snapshot.paramMap.get('registerNumber');

    if (supermarketName && registerNumber) {
      this.title = 'Edit';
      this.cashRegisterService.getBySupermarketNameAndRegisterNumber(supermarketName, +registerNumber)
        .subscribe(
          (data: CashRegisterData) => {
            this.cashRegisterForm.patchValue(data);
          },
          (error: any) => console.error(error)
        );
    }
    this.loadStatuses();
    this.loadEmployees();
    this.loadSupermarketTitles();
  }

  loadStatuses() {
    this.cashRegisterService.getStatuses().subscribe(data => {
      this.statuses = data;
    }, error => console.error(error));
  }

  loadEmployees() {
    this.cashRegisterService.getEmployees().subscribe(data => {
      this.employees = data;
    }, error => console.error(error));
  }

  loadSupermarketTitles() {
    this.cashRegisterService.getSupermarketTitles().subscribe(data => {
      this.supermarketTitles = data;
    }, error => console.error(error));
  }

  saveCashRegister(): void {
    if (this.cashRegisterForm.invalid) {
      return;
    }

    const cashRegisterData: CashRegisterData = this.cashRegisterForm.value;
    if (this.title === 'Add') {
      this.cashRegisterService.insert(cashRegisterData).subscribe(
        () => {
          this.message = 'Cash register been added successfully! Please, wait...';
          setTimeout(() => {
            this.router.navigate(['/cashregisters']);
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
      this.cashRegisterService.update(cashRegisterData).subscribe(
        () => {
          this.message = 'Cash egister been updated successfully! Please, wait...';
          setTimeout(() => {
            this.router.navigate(['/cashregisters']);
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
    this.router.navigate(['/cashregisters']);
  }

  public isUserAuthenticated(): boolean {
    return this.authService.isUserSignedIn();
  }
}
