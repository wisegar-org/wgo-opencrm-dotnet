import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AuthApiService } from '../services/auth-api.service';
import { RegisterRequest } from '../models/auth/register-request';

@Component({
  selector: 'app-register-page',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './register.page.html',
  styleUrls: ['./register.page.css'],
})
export class RegisterPage {
  readonly form: FormGroup;
  statusMessage = '';
  loading = false;

  constructor(private readonly formBuilder: FormBuilder, private readonly authApi: AuthApiService) {
    this.form = this.formBuilder.group({
      name: [''],
      lastname: [''],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      confirmPassword: ['', [Validators.required]],
    });
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const request: RegisterRequest = this.form.value;
    this.loading = true;
    this.statusMessage = 'Registrazione in corso...';

    this.authApi.register(request).subscribe({
      next: (response) => {
        if (response.success) {
          this.statusMessage = response.message || 'Registrazione completata. Controlla la tua email.';
        } else {
          const error = response.errors?.[0] ?? 'Registrazione non riuscita.';
          this.statusMessage = response.message || error;
        }
      },
      error: () => {
        this.statusMessage = 'Errore durante la registrazione.';
      },
      complete: () => {
        this.loading = false;
      },
    });
  }

  get emailControl() {
    return this.form.get('email');
  }

  get passwordControl() {
    return this.form.get('password');
  }

  get confirmPasswordControl() {
    return this.form.get('confirmPassword');
  }
}
