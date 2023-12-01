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
  employees: Employee[] = [];

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private cashRegisterService: CashRegisterService,
    private authService: AuthenticationService
  ) {
    this.cashRegisterForm = this.fb.group({
      id: 0,
      supermarketName: ['', Validators.required],
      registerNumber: [null, Validators.required],
      statusName: ['', Validators.required],
      employeeId: [null]
    });
  }

  ngOnInit(): void {
    const registerNumber = this.route.snapshot.paramMap.get('registerNumber');
    if (registerNumber) {
      this.title = 'Edit';
      this.cashRegisterService.getById(+registerNumber).subscribe( // FIXME - this should be getDetails(registerNumber)
        (data: CashRegisterData) => {
          this.cashRegisterForm.patchValue(data);
        },
        (error: any) => console.error(error)
      );
    }
    this.loadStatuses();
    this.loadEmployees();
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

  saveCashRegister(): void {
    if (this.cashRegisterForm.invalid) {
      return;
    }

    const cashRegisterData: CashRegisterData = this.cashRegisterForm.value;
    if (this.title === 'Add') {
      this.cashRegisterService.insert(cashRegisterData).subscribe(
        () => this.router.navigate(['/cashregisters']),
        (error: any) => console.error(error)
      );
    } else {
      this.cashRegisterService.update(cashRegisterData).subscribe(
        (response) => {
          console.log(response);
          this.router.navigate(['/cashregisters']);
        },
        (error: any) => console.error(error)
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
