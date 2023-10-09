import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { IonicModule } from '@ionic/angular';
import { IItemListingResponse } from '../../models/item-listing-response.dto';
import { UserService } from '../../services/user.service';
import { CommonModule } from '@angular/common';
import { ItemService } from '../../services/item.service';
import { ToastService } from '../../services/toast.service';

@Component({
  selector: 'app-item',
  templateUrl: './item.component.html',
  styleUrls: ['./item.component.scss'],
  standalone: true,
  imports: [IonicModule, CommonModule],
})
export class ItemComponent implements OnInit {
  @Input() item!: IItemListingResponse;
  @Output() buyEvent = new EventEmitter<number>();

  hasMoney = false;
  itemId = 0;
  credits = this.userService.credits;

  constructor(
    private userService: UserService,
    private itemService: ItemService,
    private toast: ToastService
  ) {}

  ngOnInit() {
    this.hasMoney = this.userService.credits >= this.item.instantPrice;
    this.itemId = this.item.id;
  }

  buyItem() {
    this.itemService.buyItem(this.itemId).subscribe({
      next: () => {
        this.toast.presentToast(
          `Congratulations to your new ${this.item.name}!`
        );
        this.userService.credits -= this.item.instantPrice;
        this.buyEvent.emit(this.item.instantPrice);
      },
      error: (response) => {
        console.log(response);
      },
    });
  }
}
