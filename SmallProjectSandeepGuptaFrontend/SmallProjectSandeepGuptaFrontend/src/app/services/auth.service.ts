import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, tap, catchError, throwError } from 'rxjs';
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  backendUrl = environment.backendUrl;

  constructor(private http: HttpClient) {}

  login(email: string, password: string): Observable<any> {
    const data = { email, passwordHash:password };
    return this.http.post<any>(`${this.backendUrl}/api/users/login`, data).pipe(
      tap(response => {
        // Store token and other user details
        console.log(response);
        localStorage.setItem('token', response.token);
        localStorage.setItem('UserID', response.userID);
        localStorage.setItem('Username', response.username); // Example: storing user ID
      }),
      catchError(this.handleError)
    );
  }

  signup(username: string, email: string, password: string): Observable<any> {
    const data = { username, email, passwordHash: password };
    return this.http.post<any>(`${this.backendUrl}/api/users/signup`, data).pipe(
      catchError(this.handleError)
    );
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }

  logout(): Observable<void> {
    return this.http.post<void>(`${this.backendUrl}/api/users/logout`, {}).pipe(
      tap(() => {
        localStorage.removeItem('token');
        localStorage.removeItem('UserID');
      }),
      catchError(this.handleError)
    );
  }

  // getUserProfile(): Observable<any> {
  //   const token = localStorage.getItem('token');
  //   const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
  //   return this.http.get<any>(`${this.backendUrl}/api/users/profile`, { headers }).pipe(
  //     catchError(this.handleError)
  //   );
  // }

  private handleError(error: any) {
    // Log or handle the error as needed
    console.error('An error occurred:', error);
    return throwError(() => new Error(error.message || 'Server Error'));
  }
}
