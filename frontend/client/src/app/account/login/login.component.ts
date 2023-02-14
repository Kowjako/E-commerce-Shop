import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  loginForm: FormGroup = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', Validators.required),
  })
  returnUrl: string;

  constructor(private accService: AccountService, private router: Router, private actRoute: ActivatedRoute) {
    this.returnUrl = actRoute.snapshot.queryParams["returnUrl"] || '/shop';
  }

  onSubmit() {
    this.accService.login(this.loginForm.value).subscribe({
      next: _ => {
        this.router.navigateByUrl(this.returnUrl)
      }
    })
  }
}
