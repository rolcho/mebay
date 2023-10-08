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

  constructor(
    private jwtDecoder: JwtDecoderService,
    private router: Router,
    private user: UserService,
    private itemService: ItemService,
    private formBuilder: FormBuilder
  ) {
    this.amountForm = this.formBuilder.group({
      amount: [0, [Validators.required, Validators.min(0)]],
    });
  }

  ionViewWillEnter() {
    if (this.jwtDecoder.isExpired() || this.user.token === undefined) {
      this.router.navigate(['login']);
      return;
    }
    this.userName = this.user.name;
    this.credits = this.user.credits;
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
