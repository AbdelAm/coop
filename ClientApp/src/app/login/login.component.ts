import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router'; // Pour constructor(private router:Router)  : fonctionne
import {AuthentificationService} from '../shared/services/authentification.service';
import {NgForm} from '@angular/forms';
import {LoginModel} from '../shared/models/login-model';
import {TokenModel} from '../shared/models/token-model';
import Swal from 'sweetalert2';
import {JwtService} from '../shared/services/jwt.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  loginModel: LoginModel;

  constructor(
    private authService: AuthentificationService,
    private router: Router,
    private jwt: JwtService
  ) {
    this.loginModel = new LoginModel();
    if (this.jwt.isConnected()) {
      this.jwt.isAdmin() && this.jwt.switchBtn
        ? this.router.navigateByUrl('/dashboard/users')
        : this.router.navigateByUrl('/dashboard/global');
    }
  }

  ngOnInit(): void {
  }

  onSubmit() {
    this.authService.login(this.loginModel).subscribe(
      (res) => {
        let tokenModel = new TokenModel(res);
        if (this.jwt.saveToken(tokenModel)) {
          this.jwt.isAdmin() && this.jwt.switchBtn
            ? this.router.navigateByUrl('/dashboard/users')
            : this.router.navigateByUrl('/dashboard/global');
        }
      },
      (err) => {
        Swal.fire({
          title: 'There is probleme !!!',
          text: err['error'],
          icon: 'error',
        });
      }
    );
  }
}
