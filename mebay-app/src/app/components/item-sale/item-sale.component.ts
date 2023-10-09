import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { IonicModule } from '@ionic/angular';
import { IItemListingResponse } from '../../models/item-listing-response.dto';
import { CommonModule } from '@angular/common';
import { ItemService } from '../../services/item.service';
import { ToastService } from '../../services/toast.service';

@Component({
  selector: 'app-item-sale',
  templateUrl: './item-sale.component.html',
  styleUrls: ['./item-sale.component.scss'],
  standalone: true,
  imports: [IonicModule, CommonModule],
})
export class ItemSaleComponent implements OnInit {
  @Input() item!: IItemListingResponse;
  @Output() deleteEvent = new EventEmitter<number>();
  picture = '';

  itemId = 0;
  constructor(private itemService: ItemService, private toast: ToastService) {}

  ngOnInit() {
    this.picture = this.item.picture;
  }

  deleteItem(itemIn: number) {
    this.itemId = this.item.id;
    this.itemService.deleteItem(itemIn).subscribe({
      next: () => {
        this.toast.presentToast(`${this.item.name} deleted.`);
        this.deleteEvent.emit(itemIn);
      },
      error: (response) => {
        console.log(response);
      },
    });
  }
}
