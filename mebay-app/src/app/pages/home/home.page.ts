import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormsModule,
  Validators,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
} from '@angular/forms';
import { IonicModule } from '@ionic/angular';
import { JwtDecoderService } from '../../services/jwt.service';
import { Router } from '@angular/router';
import { UserService } from '../../services/user.service';
import { HttpErrorResponse } from '@angular/common/http';
import { ITopUp } from 'src/app/models/user-topup.dto';

@Component({
  selector: 'app-home',
  templateUrl: './home.page.html',
  styleUrls: ['./home.page.scss'],
  standalone: true,
  imports: [IonicModule, CommonModule, FormsModule, ReactiveFormsModule],
})
export class HomePage implements OnInit {
  userName?: string;
  credits?: number;
  topUp: boolean = false;
  amountForm: FormGroup;
  amount?: number;
  constructor(
    private jwtDecoder: JwtDecoderService,
    private router: Router,
    private user: UserService,
    private formBuilder: FormBuilder
  ) {
    this.amountForm = this.formBuilder.group({
      amount: [0, [Validators.required, Validators.min(0)]],
    });
  }

  ngOnInit() {
    if (this.jwtDecoder.isExpired()) {
      this.router.navigate(['login']);
      return;
    }
    this.userName = this.user.name;
    this.credits = this.user.credits;
  }

  topUpCredit() {
    if (this.amountForm.valid) {
      this.amount = this.amountForm.get('amount')!.value;
      this.user
        .topUp({ credits: this.amount!, id: parseInt(this.user.userId) })
        .subscribe({
          next: (response: ITopUp) => {
            this.topUp = !this.topUp;
            this.credits = response.credits;
            this.user.credits = response.credits;
          },
          error: (err: HttpErrorResponse) => {
            console.log(err.status);
          },
        });
    }
  }
}