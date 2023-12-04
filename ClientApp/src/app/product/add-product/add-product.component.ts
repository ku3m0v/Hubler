import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { LkProduct, ProductService } from "../../service/product-service/product.service";
import { AuthenticationService } from "../../service/auth-service/authentication.service";

@Component({
  templateUrl: './add-product.component.html'
})
export class AddProductComponent implements OnInit {
  productForm: FormGroup;
  title: string = 'Add';
  productTitle: string | null = null;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private productService: ProductService,
    private authService: AuthenticationService
  ) {
    this.productForm = this.fb.group({
      lk_Product_Id: 0,
      title: ['', Validators.required],
      currentPrice: [0, Validators.required]
    });
  }

  ngOnInit(): void {
    this.productTitle = this.route.snapshot.paramMap.get('title');
    if (this.productTitle) {
      this.title = 'Edit';
      this.productService.getDetails(this.productTitle).subscribe(
        (product: LkProduct) => {
          this.productForm.patchValue(product);
        },
        (error: any) => console.error(error)
      );
    }
  }

  saveProduct(): void {
    if (this.productForm.invalid) {
      return;
    }

    const productData: LkProduct = this.productForm.value;
    if (this.title === 'Add') {
      this.productService.insert(productData).subscribe(
        () => this.router.navigate(['/products']),
        (error: any) => console.error(error)
      );
    } else {
      this.productService.update(productData).subscribe(
        () => this.router.navigate(['/products']),
        (error: any) => console.error(error)
      );
    }
  }

  cancel(): void {
    this.router.navigate(['/products']);
  }

  public isUserAuthenticated(): boolean {
    return this.authService.isUserSignedIn();
  }
}
