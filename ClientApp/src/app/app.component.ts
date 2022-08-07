import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { JwtService } from './shared/services/jwt.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {
  
  constructor(private jwt: JwtService, private router: Router){
  }

  ngOnInit(): void {
    if(!this.jwt.isConnected()) {
      this.router.navigateByUrl('/login')
    } else {
      (this.jwt.isAdmin() && this.jwt.switchBtn) ? this.router.navigateByUrl('/dashboard/users') : this.router.navigateByUrl('/dashboard/global');
    }
  }
}
