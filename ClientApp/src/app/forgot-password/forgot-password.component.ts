import {Component, OnInit} from '@angular/core';
import Swal from 'sweetalert2';
import {ForgetPasswordModel} from '../shared/models/forget-password-model';
import {AuthentificationService} from '../shared/services/authentification.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css'],
})
export class ForgotPasswordComponent implements OnInit {
  forgetModel: ForgetPasswordModel;

  constructor(private authService: AuthentificationService) {
    this.forgetModel = new ForgetPasswordModel();
  }

  ngOnInit(): void {
  }

  onSubmit() {
    this.authService.forgetPassword(this.forgetModel).subscribe(
      (res) => {
        Swal.fire({
          title: 'Se ha enviado un correo electrónico para confirmar tu registro',
          text: res['message'],
          icon: 'success',
        });
      },
      (err) => {
        document.getElementById('email_error').textContent =
          err['error']['email_error'];
      }
    );
  }

  emptyError(field: string) {
    document.getElementById(field).textContent = '';
  }
}
