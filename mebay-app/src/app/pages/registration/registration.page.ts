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
import { HttpClientModule, HttpErrorResponse } from '@angular/common/http';
import { AuthService } from '../../services/auth.service';
import { IUserRegisterRequest } from '../../models/user-register-request.dto.ts';
import { IUserRegisterResponse } from '../../models/user-register-response.dto.ts';
import RegistrationFormJson from '../../../assets/registration_form.json';

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
  selector: 'app-registration',
  templateUrl: './registration.page.html',
  styleUrls: ['./registration.page.scss'],
  standalone: true,
  imports: [IonicModule, CommonModule, FormsModule, ReactiveFormsModule],
  providers: [HttpClientModule],
})
export class RegistrationPage implements OnInit {
  user: IUserRegisterRequest = { name: '', email: '', password: '' };

  registrationFormGroup: FormGroup;
  registrationForm = RegistrationFormJson;

  constructor(
    private router: Router,
    private authService: AuthService,
    private fb: FormBuilder
  ) {
    this.registrationFormGroup = this.fb.group({});
    this.createControls(this.registrationForm);
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
      this.registrationFormGroup.addControl(control.key, newFormControl);
    }
  }

  ngOnInit() {}

  goToLogin() {
    this.router.navigateByUrl('/login');
  }

  submitForm() {
    if (
      this.registrationFormGroup.controls['password'].value !==
      this.registrationFormGroup.controls['confirmPassword'].value
    )
      this.registrationFormGroup.controls['confirmPassword'].setErrors(
        Validators.required
      );
    if (this.registrationFormGroup.valid) {
      this.authService.register(this.registrationFormGroup.value).subscribe({
        next: (response: IUserRegisterResponse) => {
          this.router.navigateByUrl('/login');
        },
        error: (response: HttpErrorResponse) => {
          this.registrationFormGroup.setErrors(Validators.required);
          console.log('formgroup ' + this.registrationFormGroup.value);
          console.log('registration error ' + response.status);
          if (response.status === 409) {
            this.registrationFormGroup.controls['email'].setErrors(
              Validators.required
            );
            return;
          }
          // if (response.status === 400)
          //   this.registrationFormGroup.controls['password'].setErrors(
          //     Validators.required
          //   );
          // this.registrationFormGroup.controls['confirmPassword'].setErrors(
          //   Validators.required
          // );
        },
      });
    }
  }
}
