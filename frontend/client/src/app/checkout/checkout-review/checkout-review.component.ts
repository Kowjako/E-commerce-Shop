import { CdkStepper } from '@angular/cdk/stepper';
import { Component, Input } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { BasketService } from 'src/app/basket/basket.service';

@Component({
  selector: 'app-checkout-review',
  templateUrl: './checkout-review.component.html',
  styleUrls: ['./checkout-review.component.scss']
})
export class CheckoutReviewComponent {
  /* We need it since we dont have cdkStepperNext but we have (click) */
  @Input() appStepper?: CdkStepper

  constructor(private basketSvc: BasketService, private toastr: ToastrService) {}

  createPaymentIntent() {
    this.basketSvc.createPaymentIntent().subscribe({
      next: () => {
        this.appStepper?.next();
      },
      error: err => this.toastr.error(err.message)
    })
  }
}
