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
import { IonicModule } from '@ionic/angular';
import { Router } from '@angular/router';
import { IUserLoginRequest } from '../../models/user-login-request.dto';
import { UserService } from '../../services/user.service';
import { IUserLoginResponse } from '../../models/user-login-response.dto';
import {
  HttpClientModule,
  HttpErrorResponse,
  HttpStatusCode,
} from '@angular/common/http';
import LoginFormJson from '../../../assets/login_form.json';
import { JwtDecoderService } from '../../services/jwt.service';
import { ToastService } from '../../services/toast.service';
import { StorageService } from '../../services/storage.service';

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
    private userService: UserService,
    private fb: FormBuilder,
    private jwt: JwtDecoderService,
    private toast: ToastService,
    private storage: StorageService
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

  ngOnInit() {
    this.storage.clear();
  }

  goToRegistration() {
    this.router.navigateByUrl('/registration');
  }

  submitForm() {
    if (this.loginFormGroup.valid) {
      this.userService.login(this.loginFormGroup.value).subscribe({
        next: (response: IUserLoginResponse) => {
          this.jwt.decode(response.token);
          this.storage.set('credits', response.credits);
          this.storage.set('userId', response.id);
          this.storage.set('tokenKey', response.token);
          this.storage.set('name', response.name);
          this.toast.presentToast('You are logged in');
          this.loginFormGroup.reset();
          this.router.navigate(['home']);
        },
        error: (response: HttpErrorResponse) => {
          this.loginFormGroup.setErrors(Validators.required);
          console.log(response.status);
          if (response.status === HttpStatusCode.Conflict)
            this.loginFormGroup.controls['email'].setErrors(
              Validators.required
            );
          if (response.status === HttpStatusCode.Unauthorized)
            this.loginFormGroup.controls['password'].setErrors(
              Validators.required
            );
        },
      });
    }
  }
}
