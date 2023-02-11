import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BasketModule } from './basket/basket.module';
import { NotFoundComponent } from './core/not-found/not-found.component';
import { ServerErrorComponent } from './core/server-error/server-error.component';
import { TestErrorComponent } from './core/test-error/test-error.component';
import { HomeComponent } from './home/home.component';
import { ShopModule } from './shop/shop.module';

const routes: Routes = [
  { path: '', component: HomeComponent, data: { breadcrumb: 'Home'} },
  { path: 'test-error', component: TestErrorComponent },
  { path: 'not-found', component: NotFoundComponent },
  { path: 'server-error', component: ServerErrorComponent },
  { path: 'shop', loadChildren: () => import('./shop/shop.module').then(m => ShopModule)},
  { path: 'basket', loadChildren: () => import('./basket/basket.module').then(m => BasketModule)},
  { path: '**', redirectTo: '', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
