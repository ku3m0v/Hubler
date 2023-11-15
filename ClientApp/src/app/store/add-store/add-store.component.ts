import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import {ActivatedRoute, Router} from '@angular/router';
import { SupermarketService, SupermarketWithAddress } from '../../service/store-service/store.service';

@Component({
  templateUrl: './add-store.component.html'
})
export class AddStoreComponent implements OnInit {
    storeForm: FormGroup;
    title: string = 'Add';
    storeTitle: string | null = null;

    constructor(
        private fb: FormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private supermarketService: SupermarketService
    ) {
        this.storeForm = this.fb.group({
            title: ['', Validators.required],
            phone: ['', Validators.required],
            street: ['', Validators.required],
            house: ['', Validators.required],
            city: ['', Validators.required],
            postalCode: ['', Validators.required],
            country: ['', Validators.required]
        });
    }

    ngOnInit(): void {
        this.storeTitle = this.route.snapshot.paramMap.get('title');
        if (this.storeTitle) {
            this.title = 'Edit';
            this.supermarketService.getSupermarketByTitle(this.storeTitle).subscribe(
                (data: SupermarketWithAddress) => this.storeForm.patchValue(data),
                (error: any) => console.error(error)
            );
        }
    }

    saveStore(): void {
        if (this.storeForm.invalid) {
            return;
        }

        const storeData: SupermarketWithAddress = this.storeForm.value;
        if (this.title === 'Add') {
            this.supermarketService.insertSupermarket(storeData).subscribe(
                () => this.router.navigate(['/stores']),
                (error: any) => console.error(error)
            );
        } else {
            this.supermarketService.updateSupermarket(storeData).subscribe(
                () => this.router.navigate(['/stores']),
                (error: any) => console.error(error)
            );
        }
    }

    cancel(): void {
        this.router.navigate(['/stores']);
    }
}
