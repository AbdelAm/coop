import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { JwtService } from '../shared/services/jwt.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  constructor(private jwt: JwtService, private router: Router) {
  }

  ngOnInit(): void {
    if(this.jwt.isConnected()) {
      (this.jwt.isAdmin() && this.jwt.switchBtn) ? this.router.navigateByUrl('/dashboard/users') : this.router.navigateByUrl('/dashboard/global');
    }
  }

}