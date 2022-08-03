import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { UserItemModel } from '../shared/models/user-item-model';
import { JwtService } from '../shared/services/jwt.service';
import { UserService } from '../shared/services/user-service.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  selectedButton: String;
  user: UserItemModel;
  constructor(private jwt:JwtService, private router: Router, private userService: UserService) {
    if(!this.jwt.isConnected()) {
      this.router.navigateByUrl('login');
    }
    this.user = new UserItemModel();
    this.selectedButton = "user";
  }

  ngOnInit(): void {
    this.userService.getUser(this.jwt.getConnectedUserId()).subscribe(
      res => {
        Object.assign(this.user, res);
        console.log(this.user);
      },
      err => {
        Swal.fire({
          title: "There is probleme !!!",
          text: err["error"],
          icon: "error",
        });
      }
    )
  }
  onSelected(s:string, e: Event) : void {
    this.selectedButton = s;
    let current = document.querySelector('a.active');
    current.classList.remove('active');
    (<HTMLElement>e.target).classList.add("active");
  }
}
