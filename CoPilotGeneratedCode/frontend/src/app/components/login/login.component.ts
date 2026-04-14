import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { LoginRequest } from '../../models/user.model';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  loginForm: FormGroup;
  errorMessage = signal<string | null>(null);
  isLoading = signal(false);

  constructor() {
    this.loginForm = this.fb.group({
      username: ['', [Validators.required]],
      password: ['', [Validators.required]]
    });
  }

  onSubmit(): void {
    if (this.loginForm.valid) {
      this.isLoading.set(true);
      this.errorMessage.set(null);

      const loginRequest: LoginRequest = this.loginForm.value;

      this.authService.login(loginRequest).subscribe({
        next: (response) => {
          this.isLoading.set(false);
          if (response.success) {
            // Get return URL from route parameters or default to dashboard
            const returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/dashboard';
            this.router.navigate([returnUrl]);
          } else {
            this.errorMessage.set(response.message || 'Login failed');
          }
        },
        error: (error) => {
          this.isLoading.set(false);
          this.errorMessage.set('An error occurred during login. Please try again.');
          console.error('Login error:', error);
        }
      });
    }
  }

  get username() {
    return this.loginForm.get('username');
  }

  get password() {
    return this.loginForm.get('password');
  }
}
