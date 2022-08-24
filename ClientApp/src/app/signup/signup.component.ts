import {Component, OnInit} from '@angular/core';
import {RegisterModel} from '../shared/models/register-model';
import {AuthentificationService} from '../shared/services/authentification.service';
import Swal from 'sweetalert2';
import {Router} from '@angular/router';
import { UserService } from '../shared/services/user-service.service';
import {webSocket} from 'rxjs/webSocket'

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css'],
})
export class SignupComponent implements OnInit {
  registerModel: RegisterModel;
  role: string;
  //readonly socket = webSocket('ws://localhost:44349');

  items = {
    '/signup': 'Usuarias',
    '/administrateur/register': 'Administración',
  };

  constructor(
    private authService: AuthentificationService,
    private router: Router,
    private userService: UserService
  ) {
    this.registerModel = new RegisterModel();
  }

  ngOnInit(): void {
    this.role = this.items[this.router.url];
  }

  onSubmit() {
    if (this.registerModel.password == this.registerModel.confirmPassword) {
      if (this.router.url === '/signup') {
        this.authService.register(this.registerModel).subscribe(
          (res) => {
            /*this.socket.subscribe();
            this.socket.next('done');
            this.socket.complete();
            this.socket.error({ code: 4000, reason: 'I think our app just broke!' });*/
            this.userService.progressNumber = this.userService.progressNumber + 1;
            Swal.fire({
              title: 'User created successfully!!!',
              text: res['message'],
              icon: 'success',
            });
          },
          (err) => {
            Object.keys(err['error']).forEach((key) => {
              document.getElementById(key).textContent = err['error'][key];
            });
          }
        );
      } else {
        this.authService.registerAdmin(this.registerModel).subscribe(
          (res) => {
            Swal.fire({
              title: 'Admin created successfully!!!',
              text: res['message'],
              icon: 'success',
            });
          },
          (err) => {
            Object.keys(err['error']).forEach((key) => {
              document.getElementById(key).textContent = err['error'][key];
            });
          }
        );
      }
    } else {
      document.getElementById('confirmPassword_error').textContent =
        'the two password should be similar';
    }
  }

  emptyError(field: string) {
    document.getElementById(field).textContent = '';
    if(field === "password_error") {
      document.getElementById('password_pattern').style.display = "block";
    }
  }
  emptyPattern()
  {
    document.getElementById('password_pattern').style.display = "none";
  }
}
