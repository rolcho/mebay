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
import UserProfileFormJson from '../../../assets/user_profile_form.json';
import { StorageService } from '../../services/storage.service';
import { ToastService } from '../../services/toast.service';

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
  selector: 'app-user-profile',
  templateUrl: './user-profile.page.html',
  styleUrls: ['./user-profile.page.scss'],
  standalone: true,
  imports: [IonicModule, CommonModule, FormsModule, ReactiveFormsModule],
  providers: [HttpClientModule],
})
export class UserProfilePage implements OnInit {
  user: IUserRegisterRequest = { name: '', email: '', password: '' };
  userId?: number;
  userProfileFormGroup: FormGroup;
  userProfileForm = UserProfileFormJson;

  constructor(
    private router: Router,
    private authService: AuthService,
    private fb: FormBuilder,
    private storage: StorageService,
    private toast: ToastService
  ) {
    this.userProfileFormGroup = this.fb.group({});
    this.createControls(this.userProfileForm);
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
      this.userProfileFormGroup.addControl(control.key, newFormControl);
    }
  }

  ngOnInit() {
    this.userId = parseInt(this.storage.get('userId'));
    console.log({ profileUserId: this.storage.get('userId') });
    const tokenKey = this.storage.get('tokenKey');
    if (tokenKey !== null) console.log({ profileToken: tokenKey });
    this.authService.profile(this.userId).subscribe({
      next: (response: IUserRegisterResponse) => {
        this.user.name = response.name;
        this.user.email = response.email;
        // console.log(this.user.name);
      },
      error: (response) => {
        // console.log(response);
      },
    });
  }

  goToLogin() {
    this.router.navigateByUrl('/login');
  }

  submitForm() {
    if (
      this.userProfileFormGroup.controls['password'].value !==
      this.userProfileFormGroup.controls['confirmPassword'].value
    )
      this.userProfileFormGroup.controls['confirmPassword'].setErrors(
        Validators.required
      );
    if (this.userProfileFormGroup.valid) {
      // this.authService
      //   .update(this.userProfileFormGroup.value, this.userId!)
      //   .subscribe({
      //     next: (response: IUserRegisterResponse) => {
      //       this.toast.presentToast('your profile have been updated');
      //     },
      //     error: (response: HttpErrorResponse) => {
      //       this.userProfileFormGroup.setErrors(Validators.required);
      //       console.log(response.status);
      //       if (response.status === 409) {
      //         this.userProfileFormGroup.controls['email'].setErrors(
      //           Validators.required
      //         );
      //         return;
      //       }
      //       if (response.status === 400)
      //         this.userProfileFormGroup.controls['password'].setErrors(
      //           Validators.required
      //         );
      //       this.userProfileFormGroup.controls['confirmPassword'].setErrors(
      //         Validators.required
      //       );
      //     },
      //   });
      console.log({ updatingProfile: true });
    }
  }
}
