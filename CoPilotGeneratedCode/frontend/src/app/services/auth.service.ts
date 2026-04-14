import { Injectable, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';
import { ApiService } from './api.service';
import { ApiResponse } from '../models/api-response.model';
import { LoginRequest, LoginResponse, RegisterRequest, User, UserRole } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiService = inject(ApiService);
  private router = inject(Router);

  currentUser = signal<User | null>(null);
  isAuthenticated = signal<boolean>(false);

  constructor() {
    // Check if user is already logged in
    this.loadUserFromStorage();
  }

  login(credentials: LoginRequest): Observable<ApiResponse<LoginResponse>> {
    return this.apiService.post<ApiResponse<LoginResponse>>('auth/login', credentials).pipe(
      tap(response => {
        if (response.success && response.data) {
          this.setCurrentUser(response.data);
        }
      })
    );
  }

  register(request: RegisterRequest): Observable<ApiResponse<number>> {
    return this.apiService.post<ApiResponse<number>>('auth/register', request);
  }

  logout(): void {
    localStorage.removeItem('userId');
    localStorage.removeItem('currentUser');
    this.currentUser.set(null);
    this.isAuthenticated.set(false);
    this.router.navigate(['/login']);
  }

  private setCurrentUser(loginResponse: LoginResponse): void {
    const user: User = {
      id: loginResponse.userId,
      username: loginResponse.username,
      email: loginResponse.email,
      firstName: loginResponse.firstName,
      lastName: loginResponse.lastName,
      role: loginResponse.role,
      isActive: true,
      createdAt: new Date()
    };

    localStorage.setItem('userId', user.id.toString());
    localStorage.setItem('currentUser', JSON.stringify(user));
    this.currentUser.set(user);
    this.isAuthenticated.set(true);
  }

  private loadUserFromStorage(): void {
    const userJson = localStorage.getItem('currentUser');
    if (userJson) {
      const user = JSON.parse(userJson) as User;
      this.currentUser.set(user);
      this.isAuthenticated.set(true);
    }
  }

  hasRole(role: UserRole): boolean {
    const user = this.currentUser();
    return user ? user.role === role : false;
  }

  isAdmin(): boolean {
    return this.hasRole(UserRole.Admin);
  }

  isManager(): boolean {
    return this.hasRole(UserRole.Manager);
  }

  isEmployee(): boolean {
    return this.hasRole(UserRole.Employee);
  }

  canApproveTimesheets(): boolean {
    const user = this.currentUser();
    return user ? (user.role === UserRole.Admin || user.role === UserRole.Manager) : false;
  }
}
