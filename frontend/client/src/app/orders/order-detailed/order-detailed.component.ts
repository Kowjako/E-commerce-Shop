import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Order } from 'src/app/shared/models/order';
import { BreadcrumbService } from 'xng-breadcrumb';
import { OrdersService } from '../orders.service';

@Component({
  selector: 'app-order-detailed',
  templateUrl: './order-detailed.component.html',
  styleUrls: ['./order-detailed.component.scss']
})
export class OrderDetailedComponent implements OnInit {
  order?: Order;

  constructor(private ordersSvc: OrdersService, private actRoute: ActivatedRoute,
    private bcService: BreadcrumbService) {
      this.bcService.set('@OrderDetailed', ' ');
  }

  ngOnInit(): void {
    this.loadOrder();
  }

  private loadOrder() {
    const orderId = this.actRoute.snapshot.paramMap.get("id");
    if (orderId) {
      this.ordersSvc.getOrderById(+orderId).subscribe({
        next: order => {
          this.order = order
          this.bcService.set("@OrderDetailed", `Order# ${order.id} - ${order.status}`);
        }
      })
    }
  }
}
