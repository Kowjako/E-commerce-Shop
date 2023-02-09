import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-test-error',
  templateUrl: './test-error.component.html',
  styleUrls: ['./test-error.component.scss']
})
export class TestErrorComponent {
  baseUrl = environment.apiUrl;
  validationErrors: string[] = [];

  constructor(private httpClient: HttpClient) {}

  get404Error() {
    this.httpClient.get(this.baseUrl + "products/42").subscribe({
      next: resp => console.log(resp),
      error: error => console.log(error)    
    })
  }

  get500Error() {
    this.httpClient.get(this.baseUrl + "buggy/server-error").subscribe({
      next: resp => console.log(resp),
      error: error => console.log(error) 
    })
  }

  get400Error() {
    this.httpClient.get(this.baseUrl + "buggy/badrequest").subscribe({
      next: resp => console.log(resp),
      error: error => console.log(error)      
    })
  }

  get400ValidationError() {
    this.httpClient.get(this.baseUrl + "products/million").subscribe({
      next: resp => console.log(resp),
      error: error => {
        for(const key in error.errors) {
          if(error.errors[key]) {
            this.validationErrors.push(error.errors[key])
          }
        }
      }   
    })
  }
}
