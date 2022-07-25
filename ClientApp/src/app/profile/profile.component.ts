import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { JwtService } from '../shared/services/jwt.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  selectedButton: String='user';
  constructor(private jwt:JwtService, private router: Router) {
    if(!this.jwt.isConnected()) {
      this.router.navigateByUrl('login');
    }
  }

  ngOnInit(): void {
  }
  onSelected(s) : void {
    this.selectedButton = s;
  }
}
