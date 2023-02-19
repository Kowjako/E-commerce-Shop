import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { DeliveryMethod } from '../shared/models/deliveryMethod';
import { Order, OrderToCreate } from '../shared/models/order';

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {
  baseUrl: string = environment.apiUrl;

  constructor(private httpClient: HttpClient) { }

  createOrder(order: OrderToCreate) {
    return this.httpClient.post<Order>(this.baseUrl + 'order', order)
  }

  getDeliveryMethods() {
    return this.httpClient.get<DeliveryMethod[]>(this.baseUrl + 'order/delivery-methods').pipe(
      map(dm => {
        return dm.sort((a, b) => b.price - a.price)
      })
    )
  }
}
