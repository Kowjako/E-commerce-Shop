import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ShopComponent } from './shop.component';
import { ProductCardComponent } from './product-card/product-card.component';



@NgModule({
  declarations: [
    ShopComponent,
    ProductCardComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    ShopComponent
  ]
})
export class ShopModule { }
