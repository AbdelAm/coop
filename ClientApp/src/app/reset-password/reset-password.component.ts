import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ResetPasswordModel } from '../shared/models/reset-password-model';
import { UserService } from '../shared/services/user-service.service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordComponent implements OnInit {

  resetModel: ResetPasswordModel;
  constructor(private userService: UserService, private router: Router) {
    this.resetModel = new ResetPasswordModel();
    let params = this.router.url.split("param=")[1].split("-");
    console.log(params);
  }

  ngOnInit(): void {
  }

  onSubmit()
  {
    

  }

  emptyError(field: string)
  {
    document.getElementById(field).textContent = "";
  }

}
