import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { DeliveryMethod } from '../shared/models/deliveryMethod';

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {
  baseUrl: string = environment.apiUrl;

  constructor(private httpClient: HttpClient) { }

  getDeliveryMethods() {
    return this.httpClient.get<DeliveryMethod[]>(this.baseUrl + 'order/delivery-methods').pipe(
      map(dm => {
        return dm.sort((a, b) => b.price - a.price)
      })
    )
  }
}
