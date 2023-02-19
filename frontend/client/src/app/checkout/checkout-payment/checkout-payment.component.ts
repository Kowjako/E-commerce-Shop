import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { NavigationExtras, Router } from '@angular/router';
import { loadStripe, Stripe } from '@stripe/stripe-js';
import { StripeCardCvcElement, StripeCardExpiryElement, StripeCardNumberElement } from '@stripe/stripe-js/types/stripe-js';
import { ToastrService } from 'ngx-toastr';
import { BasketService } from 'src/app/basket/basket.service';
import { Basket } from 'src/app/shared/models/basket';
import { Address } from 'src/app/shared/models/user';
import { CheckoutService } from '../checkout.service';

@Component({
  selector: 'app-checkout-payment',
  templateUrl: './checkout-payment.component.html',
  styleUrls: ['./checkout-payment.component.scss']
})
export class CheckoutPaymentComponent implements OnInit {
  @Input() checkoutForm?: FormGroup;
  @ViewChild('cardNumber') cardNumberElement?: ElementRef;
  @ViewChild('cardExpiry') cardExpiryElement?: ElementRef;
  @ViewChild('cardCvc') cardCvcElement?: ElementRef;
  stripe: Stripe | null = null;
  cardNumber?: StripeCardNumberElement;
  cardExpiry?: StripeCardExpiryElement;
  cardCvc?: StripeCardCvcElement;
  cardErrors: any;

  constructor(private basketService: BasketService, private router: Router,
    private checkoutSvc: CheckoutService, private toastr: ToastrService) { }

  ngOnInit(): void {
    loadStripe("pk_test_51McsH7JDC3vMQlymy39eITmZLOph23uTCqnLWo1nlNlGuYL816zJz8gGSrYffV8gWuMNk0fSCg8bpJx5BXoqmHEY00skICM7rb").then(stripe => {
      this.stripe = stripe;
      const elements = stripe?.elements();
      if(elements) {
        this.cardNumber = elements.create('cardNumber');
        this.cardNumber.mount(this.cardNumberElement?.nativeElement);
        this.cardNumber.on("change", event => {
          if(event.error) this.cardErrors = event.error.message;
          else {
            this.cardErrors = null;
          }
        })

        this.cardExpiry = elements.create('cardExpiry');
        this.cardExpiry.mount(this.cardExpiryElement?.nativeElement);
        this.cardExpiry.on("change", event => {
          if(event.error) this.cardErrors = event.error.message;
          else {
            this.cardErrors = null;
          }
        })

        this.cardCvc = elements.create('cardCvc');
        this.cardCvc.mount(this.cardCvcElement?.nativeElement);
        this.cardCvc.on("change", event => {
          if(event.error) this.cardErrors = event.error.message;
          else {
            this.cardErrors = null;
          }
        })
      }
    })
  }

  submitOrder() {
    const basket = this.basketService.getCurrentBasketValue();
    if (!basket) return;

    const orderToCreate = this.getOrderToCreate(basket);
    if(!orderToCreate) return;
    this.checkoutSvc.createOrder(orderToCreate).subscribe({
      next: createdOrder => {
        this.toastr.success("Order created successfully");
        this.stripe?.confirmCardPayment(basket.clientSecret!, {
          payment_method: {
            card: this.cardNumber!,
            billing_details: {
              name: this.checkoutForm?.get('paymentForm')?.get('nameOnCard')?.value
            }
          }
        }).then(result => {
          if(result.paymentIntent) {
            this.basketService.deleteLocalBasket();
            const navigationExtras: NavigationExtras = {state: createdOrder}
            this.router.navigate(['checkout/success'], navigationExtras);
          } else {
            this.toastr.error(result.error.message)
          }
        });
      }
    })
  }

  private getOrderToCreate(basket: Basket) {
    const deliveryMethodId = this.checkoutForm?.get('deliveryForm')?.get('deliveryMethod')?.value;
    const shipToAddress = this.checkoutForm?.get('addressForm')?.value as Address;

    if(!deliveryMethodId || !shipToAddress) return;
    return {
      basketId: basket.id,
      deliveryId: deliveryMethodId,
      shipToAddress: shipToAddress
    }
  }
}
