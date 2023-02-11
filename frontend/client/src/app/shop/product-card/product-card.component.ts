import { Component, Input } from '@angular/core';
import { BasketService } from 'src/app/basket/basket.service';
import { Product } from 'src/app/shared/models/product';

@Component({
  selector: 'app-product-card',
  templateUrl: './product-card.component.html',
  styleUrls: ['./product-card.component.scss']
})
export class ProductCardComponent {
  @Input() product?: Product;

  constructor(private basketService: BasketService) {}
  
  addItemToBasket() {
    this.product && this.basketService.addItemToBasket(this.product);
  }
}
