import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss']
})
export class CheckoutComponent {
  constructor(private fbs: FormBuilder) {}

  checkoutForm = this.fbs.group({
    addressForm: this.fbs.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      street: ['', Validators.required],
      city: ['', Validators.required],
      state: ['', Validators.required],
      zipcode: ['', Validators.required],
    }),
    deliveryForm: this.fbs.group({
      deliveryMethod: ['', Validators.required]
    }),
    paymentForm: this.fbs.group({
      nameOnCard: ['', Validators.required]
    })
  })
}
