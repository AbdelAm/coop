import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import { UserService } from 'src/app/shared/services/user-service.service';
import {JwtService} from '../../shared/services/jwt.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent implements OnInit {
  constructor(private jwt: JwtService, private router: Router, public userService: UserService) {
  }

  ngOnInit(): void {
  }

  isAdmin() {
    return this.jwt.isAdmin();
  }

  toggleSideBar() {
    document.body.classList.toggle('sb-sidenav-toggled');
    document.getElementById('main-container').classList.toggle('full-width');
  }

  toggleMenu() {
    document.querySelector('.dropdown-menu').classList.toggle('show');
  }

  setActiveClass(e: Event) {
    const current = document.querySelector('.active');
    if(current != null) current.classList.remove('active');
    document.querySelector('.dropdown-menu.dropdown-menu-end').classList.remove('show');
  }

  switchAccount(e: Event) {
    this.jwt.switchBtn = (<HTMLInputElement>e.target).checked;
    setTimeout(() => {
      this.jwt.switchBtn
        ? this.router.navigateByUrl('/dashboard/users')
        : this.router.navigateByUrl('/dashboard/global');
    });
  }
  showMsg() {
    document.querySelector('.messages-content').classList.toggle('show');
  }
  removeShow()
  {
    document.querySelector('.messages-content').classList.toggle('show');
  }
  logout() {
    this.jwt.removeToken();
    this.router.navigateByUrl('/login');
  }
}
