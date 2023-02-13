import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../shared/models/user';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private currentUserSource  = new BehaviorSubject<User | null>(null);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private httpClient: HttpClient, private router: Router) { }

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
