import {Component, OnInit} from '@angular/core';
import {RegisterModel} from '../shared/models/register-model';
import {AuthentificationService} from '../shared/services/authentification.service';
import Swal from 'sweetalert2';
import {Router} from '@angular/router';
import { UserService } from '../shared/services/user-service.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css'],
})

export class SignupComponent implements OnInit {
  registerModel: RegisterModel;
  role: string;

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
    if(!this.registerModel.name.match(/^[a-zA-Z ]{4,}$/gm))
    {
      document.getElementById('name_error').textContent = 'Por favor ingrese un nombre valido';
    } else if(!this.registerModel.phone.match(/^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$/im))
    {
      document.getElementById('phone_error').textContent = 'Por favor ingrese un número de teléfono válido'; 
    } else if(!this.registerModel.email.match(/[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,}$/gm))
    {
      document.getElementById('email_error').textContent = 'Por favor introduzca una dirección de correo electrónico válida';
    } else if(!this.registerModel.password.match(/(?=.*\W)(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,}/gm))
    {
      document.getElementById('password_error').textContent ='Por favor introduce una contraseña válida';
    } else if (this.registerModel.password != this.registerModel.confirmPassword) {
      document.getElementById('confirmPassword_error').textContent =
        'La nueva contraseña y la contraseña confirmada deben ser las mismas';
    } else {
      if (this.router.url === '/signup') {
        this.authService.register(this.registerModel).subscribe(
          (res) => {
            this.userService.progressNumber = this.userService.progressNumber + 1;
            Swal.fire({
              title: '¡Usuario creado con éxito!',
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
              title: '¡Administrador creado con éxito!',
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
