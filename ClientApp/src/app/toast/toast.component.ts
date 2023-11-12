import {Component, EventEmitter, Output} from '@angular/core';

@Component({
  selector: 'app-toast',
  templateUrl: './toast.component.html',
  styleUrls: ['./toast.component.css']
})
export class ToastComponent {
  @Output() confirm = new EventEmitter();
  @Output() cancel = new EventEmitter();

  confirmLogout() {
    this.confirm.emit();
  }

  cancelLogout() {
    this.cancel.emit();
  }
}
