import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';     // Pour constructor(private router:Router)  : fonctionne
import { UserService } from '../shared/services/user-service.service';
import { NgForm } from '@angular/forms';
import { LoginModel } from '../shared/models/login-model';
import { TokenModel } from '../shared/models/token-model';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginModel: LoginModel;
  constructor(private userService: UserService) {
    this.loginModel = new LoginModel();
  }

  ngOnInit(): void {
  }

  onSubmit()
  {
    this.userService.login(this.loginModel).subscribe(
      res => {
        console.log(res);
        let tokenModel = new TokenModel(res);
        tokenModel.save();
      }
    )
  }

}

