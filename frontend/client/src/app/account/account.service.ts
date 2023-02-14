import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, map, of, ReplaySubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../shared/models/user';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  // each component who subscribe to it, will be updated after
  // we will set value to ReplaySubject
  private currentUserSource  = new ReplaySubject<User | null>(1);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private httpClient: HttpClient, private router: Router) { }

  loadCurrentUser(token: string | null) {
    if(token === null) {
      this.currentUserSource.next(null);
      return of(null)
    }

    let header = new HttpHeaders();
    header = header.set('Authorization', `Bearer ${token}`);
    return this.httpClient.get<User>(this.baseUrl + "Account", {headers: header}).pipe(
      map(user => {
        if(user) {
          localStorage.setItem('token', user.jwtToken)
          // set behavioursubject value = update all observables for components
          // which are subscribed to this BehaviorSubjct
          this.currentUserSource.next(user)
          return user;
        } else {
          return null;
        }
      })
    )
  }

  login(values: any) {
    return this.httpClient.post<User>(this.baseUrl + "Account/login", values).pipe(
      map(user => {
        localStorage.setItem('token', user.jwtToken)
        this.currentUserSource.next(user)
      })
    )
  }

  register(values: any) {
    return this.httpClient.post<User>(this.baseUrl + "Account/register", values).pipe(
      map(user => {
        localStorage.setItem('token', user.jwtToken)
        this.currentUserSource.next(user)
      })
    )
  }

  logout() {
    localStorage.removeItem('token');
    this.currentUserSource.next(null);
    this.router.navigateByUrl('/');
  }

  checkEmailExists(email: string) {
    return this.httpClient.get<boolean>(this.baseUrl + "Account/email-exists?email=" + email);
  }
}
