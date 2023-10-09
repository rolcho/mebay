import { Component, OnInit, OnChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IonicModule } from '@ionic/angular';
import { UserService } from '../../../services/user.service';
import { Router } from '@angular/router';
import { JwtDecoderService } from '../../../services/jwt.service';
import { ItemService } from '../../../services/item.service';
import { IItemListingResponse } from '../../../models/item-listing-response.dto';
import { ItemComponent } from '../../../components/item/item.component';

@Component({
  selector: 'app-buy',
  templateUrl: './buy.page.html',
  styleUrls: ['./buy.page.scss'],
  standalone: true,
  imports: [IonicModule, CommonModule, ItemComponent],
})
export class BuyPage implements OnInit {
  userName?: string;
  credits: number = this.userService.credits;
  items?: IItemListingResponse[];

  constructor(
    private jwtDecoder: JwtDecoderService,
    private router: Router,
    private userService: UserService,
    private itemService: ItemService
  ) {}

  ngOnInit() {}

  ionViewWillEnter() {
    if (this.jwtDecoder.isExpired() || this.userService.token === undefined) {
      this.router.navigate(['login']);
      return;
    }
    this.credits = this.userService.credits;
    this.loadItems();
  }

  buyItem(price: number) {
    this.credits -= price;
    this.loadItems();
  }

  loadItems() {
    this.itemService.getAllBuying().subscribe({
      next: (items: IItemListingResponse[]) => {
        this.items = items;
      },
      error: (response) => {
        console.log(response);
      },
    });
  }
}
