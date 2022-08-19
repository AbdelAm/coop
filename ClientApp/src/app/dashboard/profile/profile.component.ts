import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {UserInfoModel} from '../../shared/models/user-info-model';
import Swal from 'sweetalert2';
import {UserItemModel} from '../../shared/models/user-item-model';
import {JwtService} from '../../shared/services/jwt.service';
import {UserService} from '../../shared/services/user-service.service';
import {EmailUpdateModel} from 'src/app/shared/models/email-update-model';
import {PasswordUpdateModel} from 'src/app/shared/models/password-update-model';
import { UserBankItemModel } from 'src/app/shared/models/user-bank-item-model';
import { BankAccountModel } from 'src/app/shared/models/bankAccount-model';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],
})
export class ProfileComponent implements OnInit {
  selectedButton: String;
  user: UserBankItemModel;
  userInfo: UserInfoModel;
  emailUpdate: EmailUpdateModel;
  passwordUpdate: PasswordUpdateModel;
  bank : BankAccountModel;
  status = [
    '<strong>In Progress</strong>',
    '<strong class="text-success">Approuved</strong>',
    '<strong class="text-danger text-capitalize">Rejected</strong>',
  ];

  constructor(
    private jwt: JwtService,
    private router: Router,
    private userService: UserService
  ) {
    this.selectedButton = 'user';
    this.user = new UserBankItemModel();
    this.userInfo = new UserInfoModel();
    this.emailUpdate = new EmailUpdateModel();
    this.passwordUpdate = new PasswordUpdateModel();
    this.bank = new BankAccountModel();
  }

  ngOnInit(): void {
    this.userService.getUser(this.jwt.getConnectedUserId()).subscribe(
      (res) => {
        Object.assign(this.user, res);
        this.userInfo.setInfo(this.user);
        this.emailUpdate.cif = this.user.cif;
        this.passwordUpdate.cif = this.user.cif;
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

  submitAcivated() {
    return (
      this.userInfo.name !== this.user.name ||
      this.userInfo.socialNumber !== this.user.socialNumber ||
      this.userInfo.phone !== this.user.phone
    );
  }

  onSelected(s: string, e: Event): void {
    this.selectedButton = s;
    let current = document.querySelector('a.profile-active');
    current.classList.remove('profile-active');
    (<HTMLElement>e.target).classList.add('profile-active');
  }

  updateUserInfo() {
    this.userService.updateUserInfo(this.userInfo).subscribe(
      (res) => {
        console.log(res);
        Swal.fire({
          title: 'User information updated successfully!!!',
          icon: 'success',
        });
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

  updateUserEmail() {
    if (this.emailUpdate.currentEmail === this.user.email) {
      this.userService.updateEmail(this.emailUpdate).subscribe(
        (res) => {
          Swal.fire({
            title: 'Email Changed successfully!!!',
            text: res['message'],
            icon: 'success',
          }).then(() => {
            this.jwt.removeToken();
            this.router.navigateByUrl('/login');
          });
        },
        (err) => {
          Swal.fire({
            title: 'There is probleme !!!',
            text: err['error'],
            icon: 'error',
          });
        }
      );
    } else {
      document.getElementById('email_error').textContent =
        'The current email is not correct, please verify your email';
    }
  }

  updateUserPassword() {
    if (
      this.passwordUpdate.newPassword === this.passwordUpdate.confirmedPassword
    ) {
      this.userService.updatePassword(this.passwordUpdate).subscribe(
        (res) => {
          Swal.fire({
            title: 'Password Changed successfully!!!',
            text: res['message'],
            icon: 'success',
          }).then(() => {
            this.jwt.removeToken();
            this.router.navigateByUrl('/login');
          });
        },
        (err) => {
          Object.keys(err['error']).forEach((key) => {
            document.getElementById(key).textContent = err['error'][key];
          });
        }
      );
    } else {
      document.getElementById('confirmed_password_error').textContent =
        'the new password and the confirmed password should be the same';
    }
  }

  emptyError(field: string) {
    document.getElementById(field).textContent = '';
  }
}
