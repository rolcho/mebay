import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { IonicModule } from '@ionic/angular';
import { Router } from '@angular/router';
import { HttpClientModule, HttpErrorResponse } from '@angular/common/http';
import { AuthService } from 'src/app/services/auth.service';
import { IUserRegisterRequest } from 'src/app/models/user-register-request.dto.ts';
import { IUserRegisterResponse } from 'src/app/models/user-register-response.dto.ts';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.page.html',
  styleUrls: ['./registration.page.scss'],
  standalone: true,
  imports: [IonicModule, CommonModule, FormsModule],
  providers: [HttpClientModule],
})
export class RegistrationPage implements OnInit {
  user: IUserRegisterRequest = { name: '', email: '', password: '' };

  constructor(private router: Router, private authService: AuthService) {}

  ngOnInit() {}

  goToLogin() {
    this.router.navigateByUrl('/login');
  }

  register() {
    this.authService.register(this.user).subscribe({
      next: (response: IUserRegisterResponse) => {
        this.goToLogin();
      },
      error: (response: HttpErrorResponse) => {
        console.log(response.message);
      },
    });
  }
}
