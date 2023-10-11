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
import { Observable, tap } from 'rxjs';

@Component({
  selector: 'app-home',
  templateUrl: './home.page.html',
  styleUrls: ['./home.page.scss'],
  standalone: true,
  imports: [IonicModule, CommonModule, FormsModule, ReactiveFormsModule],
})
export class HomePage {
  topUp: boolean = false;
  creditForm: FormGroup;
  topUpUserCredits?: ITopUp;
  getUser$?: Observable<IUserResponse>;
  getSoldItems$?: Observable<IItemListingResponse[]>;
  getBoughtItems$?: Observable<IItemListingResponse[]>;

  constructor(
    private jwtDecoder: JwtDecoderService,
    private router: Router,
    private userService: UserService,
    private itemService: ItemService,
    private formBuilder: FormBuilder
  ) {
    this.creditForm = this.formBuilder.group({
      amount: [, [Validators.required, Validators.min(1)]],
    });
  }

  ionViewWillEnter() {
    if (this.jwtDecoder.isExpired() || this.userService.token === undefined) {
      this.router.navigate(['login']);
      return;
    }

    this.getUser$ = this.userService.getUser();
    this.getBoughtItems$ = this.itemService.getBoughtItems();
    this.getSoldItems$ = this.itemService.getSoldItems();
  }

  topUpCredit() {
    if (this.creditForm.valid) {
      this.topUpUserCredits = {
        credits: parseInt(this.creditForm.get('amount')!.value),
        id: this.userService.userId,
      };
      this.userService
        .topUp(this.topUpUserCredits!)
        .pipe(
          tap(() => {
            this.getUser$ = this.userService.getUser();
          })
        )
        .subscribe({
          next: () => {
            this.topUp = !this.topUp;
            this.creditForm.reset();
          },
        });
    }
  }
}
