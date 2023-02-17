import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AccountModule } from './account/account.module';
import { BasketModule } from './basket/basket.module';
import { CheckoutModule } from './checkout/checkout.module';
import { AuthGuard } from './core/guards/auth.guard';
import { NotFoundComponent } from './core/not-found/not-found.component';
import { ServerErrorComponent } from './core/server-error/server-error.component';
import { TestErrorComponent } from './core/test-error/test-error.component';
import { HomeComponent } from './home/home.component';
import { OrdersModule } from './orders/orders.module';
import { ShopModule } from './shop/shop.module';

const routes: Routes = [
  { path: '', component: HomeComponent, data: { breadcrumb: 'Home'} },
  { path: 'test-error', component: TestErrorComponent },
  { path: 'not-found', component: NotFoundComponent },
  { path: 'server-error', component: ServerErrorComponent },
  { path: 'shop', loadChildren: () => import('./shop/shop.module').then(m => ShopModule)},
  { path: 'basket', loadChildren: () => import('./basket/basket.module').then(m => BasketModule)},
  { 
    path: 'checkout',
    loadChildren: () => import('./checkout/checkout.module').then(m => CheckoutModule),
    canActivate: [AuthGuard]
  },
  { path: 'account', loadChildren: () => import('./account/account.module').then(m => AccountModule)},
  { path: 'orders', loadChildren: () => import('./orders/orders.module').then(m => OrdersModule)},
  { path: '**', redirectTo: '', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
