import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { IonicModule } from '@ionic/angular';
import { JwtDecoderService } from '../../services/jwt.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-start',
  templateUrl: './start.page.html',
  styleUrls: ['./start.page.scss'],
  standalone: true,
  imports: [IonicModule, CommonModule, FormsModule],
})
export class StartPage implements OnInit {
  constructor(private jwtDecoder: JwtDecoderService, private router: Router) {}

  ngOnInit() {
    if (this.jwtDecoder.isExpired()) {
      this.router.navigate(['login']);
      return;
    }
    this.router.navigate(['user-profile']);
  }
}
