import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { AccountService } from '../account/account.service';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss']
})
export class CheckoutComponent implements OnInit {
  constructor(private fbs: FormBuilder, private accService: AccountService) {}

  ngOnInit(): void {
    this.getAddressFormValues();
  }

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

  getAddressFormValues() {
    this.accService.getUserAddress().subscribe({
      next: addr => {
        addr && this.checkoutForm.get('addressForm')?.patchValue(addr);
      }
    })
  }
}
