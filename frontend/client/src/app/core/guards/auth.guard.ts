import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { map, Observable } from 'rxjs';
import { AccountService } from 'src/app/account/account.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private accService: AccountService, private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    return this.accService.currentUser$.pipe(
      map(auth => {
        if(auth) return true;
        else {
          // queryParams will add ?returnUrl to our navigated url, so we'll have:
          // /acount/login?returnUrl=... and we can use this in LoginComponent after
          // successful login we can return user back to requested url
          this.router.navigate(['/account/login'], {queryParams: {returnUrl: state.url}})
          return false;
        }
      })
    )
  }
  
}
