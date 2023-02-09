import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Brand } from '../shared/models/brand';
import { Pagination } from '../shared/models/pagination';
import { Product } from '../shared/models/product';
import { ShopParams } from '../shared/models/shopParams';
import { Type } from '../shared/models/type';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  baseUrl = "https://localhost:5001/api/";

  constructor(private httpClient: HttpClient) { }

  getProducts(shopParams: ShopParams): Observable<Pagination<Product[]>> {
    let params = new HttpParams();

    if (shopParams.brandId > 0) {
      params = params.append('brandId', shopParams.brandId);
    }

    if (shopParams.typeId > 0) {
      params = params.append('typeId', shopParams.typeId)
    }

    params = params.append('sort', shopParams.sort)
    params = params.append('pageIndex', shopParams.pageNumber)
    params = params.append('pageSize', shopParams.pageSize)
    
    if(shopParams.search) params = params.append('search', shopParams.search)
    
    return this.httpClient.get<Pagination<Product[]>>(this.baseUrl + "Products", {
      params
    });
  }

  getProduct(id: number): Observable<Product> {
    return this.httpClient.get<Product>(this.baseUrl + "Products/" + id);
  }

  getBrands(): Observable<Brand[]> {
    return this.httpClient.get<Brand[]>(this.baseUrl + "Products/brands");
  }

  getTypes(): Observable<Type[]> {
    return this.httpClient.get<Type[]>(this.baseUrl + "Products/types");
  }
}
