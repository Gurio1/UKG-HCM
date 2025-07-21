import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.scss'],
})
export class SignupComponent {
  signupForm: FormGroup;
  isLoading = false;
  hidePassword = true;
  hideConfirmPassword = true;
  submitted = false;

  departments = ['IT', 'HR', 'Finance', 'Marketing', 'Operations', 'Sales'];

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.signupForm = this.fb.group(
      {
        firstName: ['', [Validators.required, Validators.maxLength(50)]],
        lastName: ['', [Validators.required, Validators.maxLength(50)]],
        email: ['', [Validators.required, Validators.email]],
        jobTitle: ['', [Validators.required, Validators.maxLength(100)]],
        salary: [
          '',
          [Validators.required, Validators.min(0), Validators.max(1000000)],
        ],
        department: ['', [Validators.required]],
        password: [
          '',
          [
            Validators.required,
            Validators.minLength(8),
            this.passwordValidator,
          ],
        ],
        confirmedPassword: ['', [Validators.required]],
      },
      { validators: this.passwordMatchValidator }
    );
  }

  onSubmit(): void {
    this.submitted = true;
    if (this.signupForm.invalid) {
      return;
    }

    this.isLoading = true;
    this.authService.signup(this.signupForm.value).subscribe({
      next: () => {
        this.router.navigate(['/dashboard']);
      },
      error: (err: HttpErrorResponse) => {
        this.isLoading = false;
        if (err.status === 400 && err.error?.errors) {
          Object.entries(err.error.errors).forEach(([field, messages]) => {
            const control = this.signupForm.get(field);
            if (control) {
              control.setErrors({ serverError: (messages as string[])[0] });
            }
          });
        } else {
          console.error('Signup failed', err);
        }
      },
    });
  }

  navigateToLogin(): void {
    this.router.navigate(['/login']);
  }

  passwordValidator(control: any) {
    const value: string = control.value;
    if (!value) return null;

    const hasNumber = /\d/.test(value);
    const hasSpecial = /[!@#$%^&*()_+\[\]{}:;<>,.?/~\\-]/.test(value);

    return hasNumber && hasSpecial ? null : { invalidPassword: true };
  }

  passwordMatchValidator(form: FormGroup) {
    const password = form.get('password');
    const confirm = form.get('confirmedPassword');

    return password && confirm && password.value !== confirm.value
      ? { passwordMismatch: true }
      : null;
  }

  get firstName() {
    return this.signupForm.get('firstName')!;
  }
  get lastName() {
    return this.signupForm.get('lastName')!;
  }
  get email() {
    return this.signupForm.get('email')!;
  }
  get jobTitle() {
    return this.signupForm.get('jobTitle')!;
  }
  get salary() {
    return this.signupForm.get('salary')!;
  }
  get department() {
    return this.signupForm.get('department')!;
  }
  get password() {
    return this.signupForm.get('password')!;
  }
  get confirmedPassword() {
    return this.signupForm.get('confirmedPassword')!;
  }
}
