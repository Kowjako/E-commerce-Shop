import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Pagination } from './models/pagination';
import { Product } from './models/product';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'client';
  products: Product[] = [];

  constructor(private httpClient: HttpClient) {}

  ngOnInit(): void {
    this.httpClient.get<Pagination<Product[]>>("https://localhost:5001/api/products?pageSize=50")
      .subscribe({
        next: response => this.products = response.data,
        error: err => console.log(err),
        complete: () => {
          console.log("Request completed");
          console.log("Extra statements")
        }
      })
  }
}
