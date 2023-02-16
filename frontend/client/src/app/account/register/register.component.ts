import { Component } from '@angular/core';
import { AbstractControl, AsyncValidatorFn, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { debounceTime, finalize, map, switchMap, take } from 'rxjs';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  errors: string[] | null = null;

  constructor(private fb: FormBuilder, private accService: AccountService,
    private router: Router) { }

  passExpression = "^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\s).{4,8}$"

  registerForm = this.fb.group({
    displayName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email], [this.validateEmailNotTaken()]],
    password: ['', [Validators.required, Validators.pattern(this.passExpression)]]
  })

  onSubmit() {
    this.accService.register(this.registerForm.value).subscribe({
      next: () => this.router.navigateByUrl("/shop"),
      error: modelStateError => this.errors = modelStateError.errors
    })
  }

  validateEmailNotTaken(): AsyncValidatorFn {
    return (control: AbstractControl) => {
      return control.valueChanges.pipe(
        debounceTime(1000),
        take(1),
        switchMap(() => {
          return this.accService.checkEmailExists(control.value).pipe(
            map(result => result ? { emailExists: true } : null),
            finalize(() => control.markAsTouched())
          )
        })
      )
    }
  }
}
