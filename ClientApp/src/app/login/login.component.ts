import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';     // Pour constructor(private router:Router)  : fonctionne
import { UserService } from '../shared/services/user-service.service';
import { NgForm } from '@angular/forms';
import { LoginModel } from '../shared/models/login-model';
import { TokenModel } from '../shared/models/token-model';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginModel: LoginModel;
  constructor(private userService: UserService, private router: Router) {
    this.loginModel = new LoginModel();
  }

  ngOnInit(): void {
  }

  onSubmit()
  {
    this.userService.login(this.loginModel).subscribe(
      res => {
        let tokenModel = new TokenModel(res);
        tokenModel.save();
        Swal.fire({
          title: "User login successfully",
          text: "Click on button bellow to go your interface",
          icon: "success",
          confirmButtonText: 'My Interface'
        }).then((result) => {
          if (result.value) {
            this.router.navigateByUrl('admin');
          }
        });
      },
      err => {
        Swal.fire({
          title: "There is probleme !!!",
          text: err["error"]["message"],
          icon: "error",
        });
      }
    )
  }

}

