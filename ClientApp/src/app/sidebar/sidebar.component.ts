import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { JwtService } from '../shared/services/jwt.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {

  constructor(private jwt: JwtService, private router: Router) {
  }

  ngOnInit(): void {
    let url = this.router.url.replace(/^\/+/, '');
    url = url.substring(url.indexOf('/')+1);
    /*document.getElementById(url).classList.add('active');*/
  }
  setActiveClass(e: Event)
  {
    let current = document.querySelector(".active");
    current.classList.remove("active");
    (<HTMLInputElement> e.target).classList.add("active");
  }
  linkShown()
  {
    return !this.jwt.isAdmin() || (this.jwt.isAdmin() && !this.jwt.switchBtn);
  }
  linkAdminShown()
  {
    return (this.jwt.isAdmin() && this.jwt.switchBtn);
  }
}
