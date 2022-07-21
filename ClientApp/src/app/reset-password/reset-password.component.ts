import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
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
    this.resetModel.email = params[1];
    this.resetModel.token = params[0];
    console.log(this.resetModel.token);
  }

  ngOnInit(): void {
  }

  onSubmit()
  {
    if(this.resetModel.password === this.resetModel.confirmPassword) {
      this.userService.resetPassword(this.resetModel).subscribe(
        res => {
          Swal.fire({
            title: "Password has been changed successfully",
            text: "Click on button bellow to go to login page",
            icon: "success",
            confirmButtonText: 'Login'
          }).then((result) => {
            if (result.value) {
              this.router.navigateByUrl('login');
            }
          });
        },
        err => {
          console.log(err);
        }
      )
    } else {
      document.getElementById("confirmPassword_error").textContent = "the two password should be similar";
    }

  }

  emptyError(field: string)
  {
    document.getElementById(field).textContent = "";
  }

}
