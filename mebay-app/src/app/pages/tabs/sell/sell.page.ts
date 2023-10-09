import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IonicModule } from '@ionic/angular';
import { UserService } from '../../../services/user.service';
import { Router } from '@angular/router';
import { JwtDecoderService } from '../../../services/jwt.service';
import { ItemService } from '../../../services/item.service';
import { IItemListingResponse } from '../../../models/item-listing-response.dto';
import { ItemSaleComponent } from '../../../components/item-sale/item-sale.component';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { IItemAddRequest } from '../../../models/item-add-reqest.dto';
import AddItemFormJson from '../../../../assets/add_item_form.json';
import { HttpErrorResponse } from '@angular/common/http';

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
  selector: 'app-sell',
  templateUrl: './sell.page.html',
  styleUrls: ['./sell.page.scss'],
  standalone: true,
  imports: [
    IonicModule,
    CommonModule,
    ItemSaleComponent,
    FormsModule,
    ReactiveFormsModule,
  ],
})
export class SellPage implements OnInit {
  items?: IItemListingResponse[];
  upload = false;
  newItem: IItemAddRequest = {
    name: '',
    description: '',
    picture: '',
    instantPrice: 0,
  };

  addItemFormGroup: FormGroup;
  addItemForm = AddItemFormJson;

  constructor(
    private jwtDecoder: JwtDecoderService,
    private router: Router,
    private userService: UserService,
    private itemService: ItemService,
    private fb: FormBuilder
  ) {
    this.addItemFormGroup = this.fb.group({});
    this.createControls(this.addItemForm);
  }

  ngOnInit() {}

  ionViewWillEnter() {
    if (this.jwtDecoder.isExpired() || this.userService.token === undefined) {
      this.router.navigate(['login']);
      return;
    }
    this.loadItems();
    this.upload = false;
  }

  deleteItem(item: number) {
    this.loadItems();
  }

  loadItems() {
    this.itemService.getAllSelling().subscribe({
      next: (items: IItemListingResponse[]) => {
        this.items = items;
      },
      error: (response) => {
        console.log(response);
      },
    });
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
      this.addItemFormGroup.addControl(control.key, newFormControl);
    }
  }

  submitForm() {
    if (this.addItemFormGroup.valid) {
      this.itemService.addItem(this.addItemFormGroup.value).subscribe({
        next: () => {
          this.addItemFormGroup.reset();
          this.loadItems();
          this.upload = false;
        },
        error: (response: HttpErrorResponse) => {
          this.addItemFormGroup.setErrors(Validators.required);
        },
      });
    }
  }
}
