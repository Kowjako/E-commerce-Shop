import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Pagination } from '../shared/models/pagination';
import { Product } from '../shared/models/product';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  baseUrl = "https://localhost:5001/api/";

  constructor(private httpClient: HttpClient) { }

  getProducts(): Observable<Pagination<Product[]>> {
    return this.httpClient.get<Pagination<Product[]>>(this.baseUrl + "Products?pageSize=50");
  }
}
