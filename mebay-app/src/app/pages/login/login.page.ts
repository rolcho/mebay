import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { AlertController, IonicModule } from '@ionic/angular';
import { Router } from '@angular/router';
import { IUserLoginRequest } from 'src/app/models/user-login-request.dto';
import { AuthService } from 'src/app/services/auth.service';
import { IUserLoginResponse } from 'src/app/models/user-login-response.dto';
import { HttpClientModule, HttpErrorResponse } from '@angular/common/http';
import LoginFormJson from '../../../assets/login_form.json';

export interface Options {
  label?: string;
  required?: boolean;
  type?: string;
  children?: Array<FormControlObject>;
}
export interface FormControlObject {
  key: string;
  type: string;
  options: Options;
}

@Component({
  selector: 'app-login',
  templateUrl: './login.page.html',
  styleUrls: ['./login.page.scss'],
  standalone: true,
  imports: [IonicModule, CommonModule, FormsModule, ReactiveFormsModule],
  providers: [HttpClientModule],
})
export class LoginPage implements OnInit {
  user: IUserLoginRequest = { email: '', password: '' };

  loginFormGroup: FormGroup;
  loginForm = LoginFormJson;

  constructor(
    private router: Router,
    private authService: AuthService,
    private fb: FormBuilder,
    private alertCtrl: AlertController
  ) {
    this.loginFormGroup = this.fb.group({});
    this.createControls(this.loginForm);
  }

  createControls(controls: Array<FormControlObject>) {
    for (let control of controls) {
      const newFormControl = new FormControl();

      if (control.options.required) {
        newFormControl.setValidators(Validators.required);
      }
      if (control.options.type === 'email') {
        newFormControl.setValidators(Validators.email);
      }
      this.loginFormGroup.addControl(control.key, newFormControl);
    }
  }

  ngOnInit() {}

  goToRegistration() {
    this.router.navigateByUrl('/registration');
  }

  submitForm() {
    if (this.loginFormGroup.valid) {
      this.authService.login(this.loginFormGroup.value).subscribe({
        next: (response: IUserLoginResponse) => {
          console.log(response.token);
          this.router.navigateByUrl('/loader');
        },
        error: (response: HttpErrorResponse) => {
          this.loginFormGroup.setErrors(Validators.required);
          console.log(response.status);
          if (response.status === 404)
            this.loginFormGroup.controls['email'].setErrors(
              Validators.required
            );
          if (response.status === 401)
            this.loginFormGroup.controls['password'].setErrors(
              Validators.required
            );
        },
      });
    }
  }
}
