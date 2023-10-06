import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { IonicModule } from '@ionic/angular';

@Component({
  selector: 'app-working',
  templateUrl: './working.page.html',
  styleUrls: ['./working.page.scss'],
  standalone: true,
  imports: [IonicModule, CommonModule, FormsModule]
})
export class WorkingPage implements OnInit {

  constructor() { }

  ngOnInit() {
  }

}
