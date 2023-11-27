import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { StatusModel, StatusService } from "../../../service/status-service/status.service";
import { AuthenticationService } from "../../../service/auth-service/authentication.service";

@Component({
  templateUrl: './add-status.component.html'
})
export class AddStatusComponent implements OnInit {
  statusForm: FormGroup;
  title: string = 'Add';
  statusName: string | null = null;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private statusService: StatusService,
    private authService: AuthenticationService
  ) {
    this.statusForm = this.fb.group({
      id: 0,
      statusName: ['', Validators.required],
    });
  }

  ngOnInit(): void {
    this.statusName = this.route.snapshot.paramMap.get('statusName');
    if (this.statusName) {
      this.title = 'Edit';
      this.statusService.getDetails(this.statusName).subscribe(
        (status: StatusModel) => {
          this.statusForm.patchValue(status);
        },
        (error: any) => console.error(error)
      );
    }
  }

  saveStatus(): void {
    if (this.statusForm.invalid) {
      return;
    }

    const statusData: StatusModel = this.statusForm.value;
    if (this.title === 'Add') {
      this.statusService.create(statusData).subscribe(
        () => this.router.navigate(['/statuses']),
        (error: any) => console.error(error)
      );
    } else {
      this.statusService.update(statusData).subscribe(
        () => this.router.navigate(['/statuses']),
        (error: any) => console.error(error)
      );
    }
  }

  cancel(): void {
    this.router.navigate(['/statuses']);
  }

  public isUserAuthenticated(): boolean {
    return this.authService.isUserSignedIn();
  }
}
