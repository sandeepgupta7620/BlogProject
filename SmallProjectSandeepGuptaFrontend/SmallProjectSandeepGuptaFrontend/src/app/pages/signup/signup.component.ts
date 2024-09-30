import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-signup',
  standalone: true,
  imports:[CommonModule,FormsModule],
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.scss']
})
export class SignupComponent {
  username: string = '';
  email: string = '';
  password: string = '';

  constructor(private authService: AuthService, private router: Router,private toastr : ToastrService) { }

  onSignup(): void {
    if (this.username && this.email && this.password) {
      this.authService.signup(this.username, this.email, this.password).subscribe({
        next: (response) => {
          // Handle successful signup
          console.log('Signup successful:', response);
          this.toastr.success('Signup successful', 'Success');
          this.router.navigate(['/login']);
        },
        error: (err) => {
          // Handle error during signup
          console.error('Signup error:', err);
          this.toastr.error('Signup failed', 'Error');
        }
      });
    }
  }

  gotoLogin(){
    this.router.navigate(['/login']);
  }
}
