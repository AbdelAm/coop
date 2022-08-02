import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { JwtService } from '../shared/services/jwt.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  constructor(private jwt: JwtService, private router: Router) {
  }

  ngOnInit(): void {
    if(this.jwt.isAdmin()) {
      document.addEventListener("DOMContentLoaded", () => {
        (<HTMLInputElement> document.getElementById('switch-toggle')).checked = this.jwt.switchBtn;
      });
    }
  }
  isAdmin()
  {
    return this.jwt.isAdmin();
  }
  toggleSideBar() {
    document.body.classList.toggle('sb-sidenav-toggled');
  }
  toggleMenu() {
    document.querySelector('.dropdown-menu').classList.toggle('show');
  }
  switchAccount(e: Event)
  {
    this.jwt.switchBtn = (<HTMLInputElement> e.target).checked;
    setTimeout(() => {
      (this.jwt.switchBtn) ? this.router.navigateByUrl('/admin') : this.router.navigateByUrl('/global')
    }, 500)
  }
  logout()
  {
    this.jwt.removeToken();
    this.router.navigateByUrl('login');
  }
}
