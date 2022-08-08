import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { JwtService } from '../../shared/services/jwt.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  constructor(private jwt: JwtService, private router: Router) {
  }

  ngOnInit(): void {
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
      (this.jwt.switchBtn) ? this.router.navigateByUrl('/dashboard/users') : this.router.navigateByUrl('/dashboard/global')
    }, 250)
  }
  logout()
  {
    this.jwt.removeToken();
    this.router.navigateByUrl('/login');
  }
}
