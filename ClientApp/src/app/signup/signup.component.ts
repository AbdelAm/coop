import { Component, OnInit } from '@angular/core';
import { RegisterModel } from '../shared/models/register-model';
import { UserService } from '../shared/services/user-service.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent implements OnInit {

  registerModel: RegisterModel;

  constructor(private userService: UserService) {
    this.registerModel = new RegisterModel();
  }

  ngOnInit(): void {
  }

  onSubmit()
  {
    this.userService.register(this.registerModel).subscribe(
      res => {
        console.log(res);
        Swal.fire({
          title: "User created successfully!!!",
          text: res["message"],
          icon: "success",
        });
      }
    )
  }

}
