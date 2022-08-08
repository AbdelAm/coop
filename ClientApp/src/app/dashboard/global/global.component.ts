import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { JwtService } from '../../shared/services/jwt.service';

@Component({
  selector: 'app-global',
  templateUrl: './global.component.html',
  styleUrls: ['./global.component.css']
})
export class GlobalComponent implements OnInit {

  constructor(private jwt:JwtService, private router: Router) {
    if(!this.jwt.isConnected()) {
      this.router.navigateByUrl('/login');
    }
    if(this.jwt.isAdmin() && this.jwt.switchBtn) {
      this.router.navigateByUrl('/dashboard/users');
    }
  }

  ngOnInit(): void {
  }

}
