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
import { JwtDecoderService } from '../../../services/jwt.service';
import { Router } from '@angular/router';
import { UserService } from '../../../services/user.service';
import { HttpErrorResponse } from '@angular/common/http';
import { ITopUp } from '../../../models/user-topup.dto';
import { ItemService } from '../../../services/item.service';
import { IItemListingResponse } from '../../../models/item-listing-response.dto';
import { IUserResponse } from '../../../models/user-response.dto';

@Component({
  selector: 'app-home',
  templateUrl: './home.page.html',
  styleUrls: ['./home.page.scss'],
  standalone: true,
  imports: [IonicModule, CommonModule, FormsModule, ReactiveFormsModule],
})
export class HomePage {
  userName?: string;
  credits?: number;
  topUp: boolean = false;
  amountForm: FormGroup;
  amount?: number;
  soldItems?: IItemListingResponse[];
  boughtItems?: IItemListingResponse[];
  user?: IUserResponse;

  constructor(
    private jwtDecoder: JwtDecoderService,
    private router: Router,
    private userService: UserService,
    private itemService: ItemService,
    private formBuilder: FormBuilder
  ) {
    this.amountForm = this.formBuilder.group({
      amount: [, [Validators.required, Validators.min(1)]],
    });
  }

  ionViewWillEnter() {
    if (this.jwtDecoder.isExpired() || this.userService.token === undefined) {
      this.router.navigate(['login']);
      return;
    }

    this.userService.getUser().subscribe({
      next: (user: IUserResponse) => {
        this.userName = user.name;
        this.credits = user.credits;
      },
      error: (response) => {
        console.log(response);
      },
    });

    this.itemService.getBoughtItems().subscribe({
      next: (items: IItemListingResponse[]) => {
        this.boughtItems = items;
      },
      error: (response) => {
        console.log(response);
      },
    });
    this.itemService.getSoldItems().subscribe({
      next: (items: IItemListingResponse[]) => {
        this.soldItems = items;
      },
      error: (response) => {
        console.log(response);
      },
    });
  }

  topUpCredit() {
    if (this.amountForm.valid) {
      this.amount = this.amountForm.get('amount')!.value;
      this.userService
        .topUp({ credits: this.amount!, id: parseInt(this.userService.userId) })
        .subscribe({
          next: (response: ITopUp) => {
            this.topUp = !this.topUp;
            this.credits = response.credits;
            this.userService.credits = response.credits;
          },
          error: (err: HttpErrorResponse) => {
            console.log(err.status);
          },
        });
    }
  }
}
