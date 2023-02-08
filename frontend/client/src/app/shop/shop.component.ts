import { Component, OnInit } from '@angular/core';
import { Brand } from '../shared/models/brand';
import { Product } from '../shared/models/product';
import { Type } from '../shared/models/type';
import { ShopService } from './shop.service';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit {
  products: Product[] = [];
  brands: Brand[] = [];
  types: Type[] = [];
  brandIdSelected = 0;
  typeIdSelected = 0;
  sortSelected = 'name';
  sortOptions = [
    {name: 'Alphabetical', value: 'name'},
    {name: 'Price: Low to high', value: 'priceAsc'},
    {name: 'Price: High to low', value: 'priceDesc'}
  ]

  constructor(private shopService: ShopService) {}

  ngOnInit(): void {
    this.getBrands();
    this.getProducts();
    this.getTypes();
  }

  getProducts() : void{
    this.shopService.getProducts(this.brandIdSelected, this.typeIdSelected, this.sortSelected).subscribe({
      next: response => this.products = response.data,
      error: error => console.log(error)
    })
  }

  getBrands(): void {
    this.shopService.getBrands().subscribe({
      next: response => this.brands = [{id: 0, name: 'All'}, ...response],
      error: error => console.log(error)
    })
  }
  
  getTypes(): void {
    this.shopService.getTypes().subscribe({
      next: response => this.types = [{id: 0, name: 'All'}, ...response],
      error: error => console.log(error)
    })
  }

  onBrandSelected(brandId: number): void {
    this.brandIdSelected = brandId;
    this.getProducts();
  }

  onTypeSelected(typeId: number): void {
    this.typeIdSelected = typeId;
    this.getProducts();
  }
  
  onSortSelected(event: any) {
    this.sortSelected = event.target.value;
    this.getProducts();
  }
}
