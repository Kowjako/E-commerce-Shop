import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Order } from '../shared/models/order';

@Injectable({
  providedIn: 'root'
})
export class OrdersService {
  baseUrl = environment.apiUrl;

  constructor(private httpClient: HttpClient) { }

  getOrders() {
    return this.httpClient.get<Order[]>(this.baseUrl + 'order');
  }

  getOrderById(id: number) {
    return this.httpClient.get<Order>(this.baseUrl + 'order/' + id);
  }
}
