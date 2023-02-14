import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  loginForm: FormGroup = new FormGroup({
    email: new FormControl('', Validators.required),
    password: new FormControl('', Validators.required),
  })

  constructor(private accService: AccountService, private router: Router) {}

  onSubmit() {
    this.accService.login(this.loginForm.value).subscribe({
      next: user => {
        this.router.navigateByUrl("/shop")
      }
    })
  }
}
