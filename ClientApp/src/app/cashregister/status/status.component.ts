import {Component, OnInit} from '@angular/core';
import {StatusModel, StatusService} from "../../service/status-service/status.service";

@Component({
  selector: 'app-status',
  templateUrl: './status.component.html',
  styleUrls: ['./status.component.css']
})
export class StatusComponent implements OnInit {
  statuses: StatusModel[] = [];
  showSpinner = true;
  showButton = false;

  constructor(private StatusService: StatusService) {
    this.load();
    setTimeout(() => {
      this.showSpinner = false;
      this.showButton = true;
    }, 1500);
  }

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.StatusService.GetAll().subscribe(
      data => this.statuses = data,
      error => console.error('Error fetching statuses', error)
    );
  }

  delete(id: number): void {
    const confirmation = confirm('Are you sure you want to delete this staus?');
    if (confirmation) {
      this.StatusService.delete(id).subscribe(
        () => {
          this.load();
        },
        error => console.error('Error deleting status', error)
      );
    }
  }
}
