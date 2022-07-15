import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }
  toggleSideBar() {
    document.body.classList.toggle('sb-sidenav-toggled');
  }
  toggleMenu() {
    document.querySelector('.dropdown-menu').classList.toggle('show');
  }
}
