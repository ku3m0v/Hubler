import { Injectable, EventEmitter } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ToastService {
  public toastVisibilityChanged = new EventEmitter<boolean>();

  constructor() {}

  showToast() {
    this.toastVisibilityChanged.emit(true);
  }

  hideToast() {
    this.toastVisibilityChanged.emit(false);
  }
}
