import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {ActivatedRoute, Router} from '@angular/router';
import {SupermarketService, SupermarketWithAddress} from '../../service/store-service/store.service';

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
            title: ['', Validators.required, Validators.pattern(/^[a-zA-Z]+$/), Validators.maxLength(25)],
            phone: ['', [Validators.required, Validators.pattern(/^[0-9]{3}-[0-9]{3}-[0-9]{4}$/)]],
            street: ['', Validators.required, Validators.pattern(/^[a-zA-Z]+$/), Validators.maxLength(25)],
            house: ['', Validators.required, Validators.maxLength(5), Validators.pattern(/^[0-9]+$/)],
            city: ['', Validators.required, Validators.pattern(/^[a-zA-Z]+$/), Validators.maxLength(25)],
            postalCode: ['', Validators.required, Validators.pattern(/^[0-9]{2}-[0-9]{3}$/)],
            country: ['', Validators.required, Validators.pattern(/^[a-zA-Z]+$/), Validators.maxLength(25)]
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
