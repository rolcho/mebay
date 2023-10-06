import { Component } from '@angular/core';
import { IonicModule } from '@ionic/angular';
import { TabsComponent } from './components/tabs/tabs.component';
import { NgIf } from '@angular/common';
import { UserService } from './services/user.service';

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['app.component.scss'],
  standalone: true,
  imports: [IonicModule, TabsComponent, NgIf],
  providers: [UserService],
})
export class AppComponent {
  constructor(public userService: UserService) {}
}
