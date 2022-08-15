import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {JwtService} from '../../shared/services/jwt.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent implements OnInit {
  constructor(private jwt: JwtService, private router: Router) {
  }

  ngOnInit(): void {
  }

  isAdmin() {
    return this.jwt.isAdmin();
  }

  toggleSideBar() {
    document.body.classList.toggle('sb-sidenav-toggled');
  }

  toggleMenu() {
    document.querySelector('.dropdown-menu').classList.toggle('show');
  }

  setActiveClass(e: Event) {
    const current = document.querySelector('.active');
    current.classList.remove('active');
  }

  switchAccount(e: Event) {
    this.jwt.switchBtn = (<HTMLInputElement>e.target).checked;
    setTimeout(() => {
      this.jwt.switchBtn
        ? this.router.navigateByUrl('/dashboard/users')
        : this.router.navigateByUrl('/dashboard/global');
    });
  }

  logout() {
    this.jwt.removeToken();
    this.router.navigateByUrl('/login');
  }
}
