import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Order } from 'src/app/shared/models/order';

@Component({
  selector: 'app-checkout-success',
  templateUrl: './checkout-success.component.html',
  styleUrls: ['./checkout-success.component.scss']
})
export class CheckoutSuccessComponent {
  orderId?: number;

  constructor(private router: Router) {
    this.orderId = (router.getCurrentNavigation()?.extras?.state as Order)?.id;
  }
}
