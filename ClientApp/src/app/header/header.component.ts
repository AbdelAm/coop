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
    if(this.isAdmin())
    {
      (<HTMLInputElement> document.querySelector('.switch-btn')).checked = this.jwt.switchBtn;
    }
  }
  isAdmin()
  {
    return this.isAdmin;
  }
  toggleSideBar() {
    document.body.classList.toggle('sb-sidenav-toggled');
  }
  toggleMenu() {
    document.querySelector('.dropdown-menu').classList.toggle('show');
  }
  switchAccount(e: Event)
  {
    console.log((<HTMLInputElement> e.target).checked);
    this.jwt.switchBtn = (<HTMLInputElement> e.target).checked;
    (this.jwt.isAdmin() && this.jwt.switchBtn) ? this.router.navigate(['/admin']) : this.router.navigate(['/global'])
  }
  logout()
  {
    this.jwt.removeToken();
    this.router.navigateByUrl('login');
  }
}
