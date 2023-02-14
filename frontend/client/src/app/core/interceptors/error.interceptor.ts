import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private router: Router, private toastr: ToastrService) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        console.log(error)
        if(error) {
          switch(error.status) {
            case 400: 
              if(error.error.errors) {
                throw error.error; //throw Validation Error to component so after subscribe we will have it inside error: err => console.log(err)
              }
              else {
                this.toastr.error('Requested resource was not found', error.status.toString());
              }
              break;
            case 401:
              this.toastr.error('Authorized, you are not', error.status.toString());
              break;
            case 404:
              this.router.navigateByUrl('/not-found');
              break;
            case 500:
              const navigationExtras: NavigationExtras = {
                state: {
                  error: error.error
                }
              }
              this.router.navigateByUrl('/server-error', navigationExtras);
              break;
            default:
              this.toastr.error('Something unexpected went wrong :(')
              break;
          }
        }
        return throwError(() => new Error(error.message));
      })
    );
  }
}
