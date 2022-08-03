import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit{
  
  url: string;
  constructor(private router: Router){
  }

  ngOnInit(): void
  {
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.url = event.url;
      }
    })
  }

  isAuthRoute() {
    return ['/login', '/signup', 'email-confirmation', 'forgot-password', 'reset-password'].includes(this.url);
  }
}
