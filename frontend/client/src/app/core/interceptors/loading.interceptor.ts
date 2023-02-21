import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { delay, finalize, identity, Observable } from 'rxjs';
import { ServiceBusyService } from '../services/service-busy.service';
import { environment } from 'src/environments/environment';

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {

  constructor(private busyService: ServiceBusyService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    if(request.method === 'POST' && request.url.includes('orders')) {
      return next.handle(request);
    }
    
    this.busyService.busy();
    return next.handle(request).pipe(
      (environment.production ? identity : delay(1000)),
      finalize(() => this.busyService.idle())
    );
  }
}
