import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { AccountService } from '../account/account.service';
import { BasketService } from '../basket/basket.service';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss']
})
export class CheckoutComponent implements OnInit {
  constructor(private fbs: FormBuilder, private accService: AccountService,
    private basketSvc: BasketService) {}

  ngOnInit(): void {
    this.getAddressFormValues();
    this.getDeliveryMethodValue();
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

  getDeliveryMethodValue() {
    const basket = this.basketSvc.getCurrentBasketValue();
    if(basket && basket.deliveryMethodId) {
      this.checkoutForm.get('deliveryForm')?.get('deliveryMethod')?.patchValue(basket.deliveryMethodId.toString())
    }
  }
}
