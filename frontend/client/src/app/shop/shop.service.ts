import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable, of } from 'rxjs';
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
  products: Product[] = [];
  brands: Brand[] = [];
  types: Type[] = [];
  pagination?: Pagination<Product[]>;
  shopParams = new ShopParams();
  productCache = new Map<string, Pagination<Product[]>>();

  constructor(private httpClient: HttpClient) { }

  getProducts(useCache: boolean = true): Observable<Pagination<Product[]>> {
    if(!useCache) this.productCache = new Map();
    
    if(this.productCache.size > 0 && useCache) {
      if(this.productCache.has(Object.values(this.shopParams).join("-"))) {
        this.pagination = this.productCache.get(Object.values(this.shopParams).join("-"))
        if(this.pagination) return of(this.pagination)
      }
    }

    let params = new HttpParams();

    if (this.shopParams.brandId > 0) {
      params = params.append('brandId', this.shopParams.brandId);
    }

    if (this.shopParams.typeId > 0) {
      params = params.append('typeId', this.shopParams.typeId)
    }

    params = params.append('sort', this.shopParams.sort)
    params = params.append('pageIndex', this.shopParams.pageNumber)
    params = params.append('pageSize', this.shopParams.pageSize)

    if (this.shopParams.search) params = params.append('search', this.shopParams.search)

    return this.httpClient.get<Pagination<Product[]>>(this.baseUrl + "Products", {
      params
    }).pipe(
      map(resp => {
        this.productCache.set(Object.values(this.shopParams).join('-'), resp);
        this.pagination = resp;
        return resp;
      })
    );
  }

  setShopParams(params: ShopParams) {
    this.shopParams = params;
  }

  getShopParams() {
    return this.shopParams;
  }

  getProduct(id: number): Observable<Product> {
    const product = [...this.productCache.values()]
      .reduce((acc, paginatedResult) => {
        return {...acc, ...paginatedResult.data.find(x => x.id === id)}
      }, {} as Product)

    if (Object.keys(product).length !== 0) {
      return of(product);
    }

    return this.httpClient.get<Product>(this.baseUrl + "Products/" + id);
  }

  getBrands(): Observable<Brand[]> {
    if (this.brands.length > 0) {
      return of(this.brands);
    }

    return this.httpClient.get<Brand[]>(this.baseUrl + "Products/brands").pipe(
      map(data => {
        this.brands = data;
        return data;
      })
    );
  }

  getTypes(): Observable<Type[]> {
    if (this.types.length > 0) {
      return of(this.types)
    }

    return this.httpClient.get<Type[]>(this.baseUrl + "Products/types").pipe(
      map(data => {
        this.types = data;
        return data;
      })
    );
  }
}
