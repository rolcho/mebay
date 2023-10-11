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
import {
  HttpClientModule,
  HttpErrorResponse,
  HttpStatusCode,
} from '@angular/common/http';
import { UserService } from '../../../services/user.service';
import { IUserRegisterRequest } from '../../../models/user-register-request.dto.ts';
import { IUserRegisterResponse } from '../../../models/user-register-response.dto.ts';
import UserProfileFormJson from '../../../../assets/user_profile_form.json';
import { ToastService } from '../../../services/toast.service';
import { IUserResponse } from '../../../models/user-response.dto';

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
  selector: 'app-profile',
  templateUrl: './profile.page.html',
  styleUrls: ['./profile.page.scss'],
  standalone: true,
  imports: [IonicModule, CommonModule, FormsModule, ReactiveFormsModule],
  providers: [HttpClientModule],
})
export class UserProfilePage {
  user: IUserRegisterRequest = { name: '', email: '', password: '' };
  userId?: number;
  userProfileFormGroup: FormGroup;
  userProfileForm = UserProfileFormJson;
  isAdmin = false;
  userList: IUserResponse[] = [];

  constructor(
    private router: Router,
    private userService: UserService,
    private fb: FormBuilder,
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

  ionViewWillEnter() {
    this.userId = this.userService.userId;
    this.userService.profile(this.userId).subscribe({
      next: (response: IUserRegisterResponse) => {
        this.userProfileFormGroup.get('name')?.setValue(response.name);
        this.userProfileFormGroup.get('email')?.setValue(response.email);
        this.user.name = response.name;
        this.user.email = response.email;
      },
      error: (response) => {
        console.log(response);
      },
    });
    this.isAdmin = this.userService.isAdmin;
  }

  ionViewWillLeave() {
    this.userList = [];
  }

  goToLogin() {
    this.router.navigateByUrl('/');
  }

  logout() {
    this.userService.logout();
    this.goToLogin();
  }

  listUsers() {
    this.userService.listUsers().subscribe({
      next: (response: IUserResponse[]) => {
        this.userList = response;
        console.log(response);
      },
      error: (response) => {
        console.log(response);
      },
    });
  }

  deleteProfile() {
    if (this.userProfileFormGroup.valid) {
      this.user.password =
        this.userProfileFormGroup.controls['existingPassword'].value;
      this.userService.login(this.user).subscribe({
        next: () => {
          this.userService.delete(this.userService.userId).subscribe({
            next: () => {
              this.userService.logout();
              this.router.navigate(['/']);
            },
            error: () => {
              this.toast.presentToast('You must enter your password!');
            },
          });
        },
        error: () => {
          this.userProfileFormGroup.controls[
            'existingPassword'
          ].markAsTouched();
        },
      });
    } else {
      this.toast.presentToast('You must enter your password!');
    }
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
      this.user.password =
        this.userProfileFormGroup.controls['existingPassword'].value;
      if (this.user.password === '') {
        this.userProfileFormGroup.controls['existingPassword'].markAsTouched();
      } else
        this.userService.login(this.user).subscribe({
          next: () => {
            if (this.userProfileFormGroup.controls['password'].value)
              this.user.password =
                this.userProfileFormGroup.controls['password'].value;
            this.user.name = this.userProfileFormGroup.controls['name'].value;
            this.user.email = this.userProfileFormGroup.controls['email'].value;
            this.userService
              .update(this.user, this.userService.userId)
              .subscribe({
                next: (response: IUserRegisterResponse) => {
                  this.toast.presentToast('Your profile has been updated.');
                  this.userProfileFormGroup.controls[
                    'existingPassword'
                  ].reset();
                  this.userProfileFormGroup.controls['password'].reset();
                  this.userProfileFormGroup.controls['confirmPassword'].reset();
                },
                error: (response: HttpErrorResponse) => {
                  this.userProfileFormGroup.setErrors(Validators.required);
                  if (response.status === HttpStatusCode.Conflict) {
                    this.userProfileFormGroup.controls['email'].setErrors(
                      Validators.required
                    );
                    return;
                  }
                  if (response.status === HttpStatusCode.Unauthorized)
                    this.userProfileFormGroup.controls[
                      'existingPassword'
                    ].setErrors(Validators.required);
                  this.userProfileFormGroup.controls[
                    'confirmPassword'
                  ].setErrors(Validators.required);
                },
              });
          },
          error: (response) => {
            if (response.status === HttpStatusCode.Unauthorized) {
              this.userProfileFormGroup.controls['existingPassword'].setErrors(
                Validators.required
              );
              return;
            }
          },
        });
    }
  }
}
