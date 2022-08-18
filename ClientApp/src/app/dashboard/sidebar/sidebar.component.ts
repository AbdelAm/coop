import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {JwtService} from '../../shared/services/jwt.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css'],
})
export class SidebarComponent implements OnInit {
  constructor(private jwt: JwtService, private router: Router) {
  }

  ngOnInit(): void {
  }

  setActiveClass(e: Event) {
    let current = document.querySelector('.active');
    if(current != null) current.classList.remove('active');
    (<HTMLInputElement>e.target).classList.add('active');
    document.querySelector('.mat-typography').classList.remove('sb-sidenav-toggled');
  }

  linkShown() {
    return !this.jwt.isAdmin() || (this.jwt.isAdmin() && !this.jwt.switchBtn);
  }

  linkAdminShown() {
    return this.jwt.isAdmin() && this.jwt.switchBtn;
  }
}
