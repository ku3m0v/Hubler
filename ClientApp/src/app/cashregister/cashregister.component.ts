import { Component, OnInit } from '@angular/core';
import {CashRegisterData, CashRegisterService} from "../service/cashregister-service/cashregister.service";

@Component({
  selector: 'app-cashregister',
  templateUrl: './cashregister.component.html'
})
export class CashRegisterComponent implements OnInit {
  cashRegisters: CashRegisterData[] = [];

  constructor(private cashRegisterService: CashRegisterService) {}

  ngOnInit(): void {
    this.loadCashRegisters();
  }

  loadCashRegisters(): void {
    this.cashRegisterService.getAllCashRegisters().subscribe(
      data => this.cashRegisters = data,
      error => console.error(error)
    );
  }

  deleteCashRegister(id: number): void {
    const confirmation = confirm('Are you sure you want to delete this cash register?');
    if (confirmation) {
      this.cashRegisterService.deleteCashRegister(id).subscribe(
        () => this.loadCashRegisters(),
        error => console.error(error)
      );
    }
  }
}
