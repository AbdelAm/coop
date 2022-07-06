import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-administrateur',
  templateUrl: './administrateur.component.html',
  styleUrls: ['./administrateur.component.css']
})
export class AdministrateurComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }
  setActiveClass(e: Event)
  {
    let current = document.querySelector(".active");
    current.classList.remove("active");
    (<HTMLInputElement> e.target).classList.add("active");
  }
  toggleSideBar()
  {
    document.body.classList.toggle('sb-sidenav-toggled');
  }

}
