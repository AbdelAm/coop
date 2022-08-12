import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {AuthentificationService} from '../shared/services/authentification.service';

@Component({
  selector: 'app-email-confirmation',
  templateUrl: './email-confirmation.component.html',
  styleUrls: ['./email-confirmation.component.css'],
})
export class EmailConfirmationComponent implements OnInit {
  constructor(
    private router: Router,
    private authService: AuthentificationService
  ) {
    this.authService.confirmEmail(this.router.url).subscribe((res) => {
      document.querySelector('.title').textContent = 'Email Confirmed !!!';
      document.querySelector('.message').textContent = res['message'];
    });
  }

  ngOnInit(): void {
  }
}
