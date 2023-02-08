import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Brand } from '../shared/models/brand';
import { Pagination } from '../shared/models/pagination';
import { Product } from '../shared/models/product';
import { Type } from '../shared/models/type';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  baseUrl = "https://localhost:5001/api/";

  constructor(private httpClient: HttpClient) { }

  getProducts(brandId?: number, typeId?: number, sort?: string): Observable<Pagination<Product[]>> {
    let params = new HttpParams();

    if (brandId) {
      params = params.append('brandId', brandId);
    }

    if (typeId) {
      params = params.append('typeId', typeId)
    }

    if (sort) {
      params = params.append('sort', sort)
    }

    return this.httpClient.get<Pagination<Product[]>>(this.baseUrl + "Products", {
      params
    });
  }

  getBrands(): Observable<Brand[]> {
    return this.httpClient.get<Brand[]>(this.baseUrl + "Products/brands");
  }

  getTypes(): Observable<Type[]> {
    return this.httpClient.get<Type[]>(this.baseUrl + "Products/types");
  }
}
