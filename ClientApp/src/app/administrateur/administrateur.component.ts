import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { JwtService } from '../shared/services/jwt.service';

@Component({
  selector: 'app-administrateur',
  templateUrl: './administrateur.component.html',
  styleUrls: ['./administrateur.component.css']
})
export class AdministrateurComponent implements OnInit {

  constructor(private jwt: JwtService, private router: Router) {
    if(this.jwt.isConnected()) {
      if(!this.jwt.isAdmin() || !this.jwt.switchBtn) this.router.navigateByUrl('users');
    } else {
      this.router.navigateByUrl('login');
    }
  }

  ngOnInit(): void {
  }

}
