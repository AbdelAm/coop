import {Component, ElementRef, OnInit, QueryList, ViewChildren} from '@angular/core';
import {MatDialog} from '@angular/material/dialog';
import {Router} from '@angular/router';
import {UserItemModel} from 'src/app/shared/models/user-item-model';
import {ItemsModel} from 'src/app/shared/models/items-model';
import {JwtService} from 'src/app/shared/services/jwt.service';
import {UserService} from 'src/app/shared/services/user-service.service';
import {TransactionPopupComponent} from 'src/app/transaction-popup/transaction-popup.component';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {

  readonly pageSize = 5;
  currentPage: number;
  listUser: Array<string>;
  userItems: ItemsModel<UserItemModel>;
  pageNumber: Array<number>;
  status = [
    '<strong>In Progress</strong>',
    '<strong class="text-success">Approuved</strong>',
    '<strong class="text-danger text-capitalize">Rejected</strong>'
  ]
  
  constructor(public dialog:MatDialog, private jwt:JwtService, private router: Router, private userService: UserService) {
    if(!this.jwt.isConnected()) {
      this.router.navigateByUrl('/login');
    }
    if(!this.jwt.isAdmin() || !this.jwt.switchBtn) {
      this.router.navigateByUrl('/global');
    }
    this.listUser = new Array<string>();
    this.userItems = new ItemsModel<UserItemModel>();
    this.pageNumber = [];
    this.currentPage = 0;
  }

  ngOnInit(): void {
    this.getItems(0);
  }

  getItems(num: number) {
    this.userService.getUsers(num).subscribe(
      res => {
        Object.assign(this.userItems, res);
        let result = Math.trunc(this.userItems.itemsNumber / this.pageSize);
        if (this.userItems.itemsNumber % this.pageSize != 0) {
          result++;
        }
        this.pageNumber = Array.from(Array(result).keys());
        this.currentPage = num;
      },
      err => {
        if ([401, 403].includes(err['status'])) {
          this.router.navigate(['global']);
        } else {
          Swal.fire({
            title: 'There is a Problem!!!',
            text: err['error'],
            icon: 'error'
          });
        }
      }
    );
    window.scrollTo(0, 0);
  }

  selectAll(e: Event) {
    let items = document.querySelectorAll('.items');
    for (let i = 0; i < items.length; i++) {
      (<HTMLInputElement>items[i]).checked = (<HTMLInputElement>e.target).checked;
      let id = (<HTMLInputElement>items[i]).value;
      this.toggleItem(id, (<HTMLInputElement>e.target).checked);
    }

  }

  addUser(): void {
    this.dialog.open(TransactionPopupComponent, {
      width: '60%',
      data: 'right click'
    });
  }

  setUser(id: string, e: Event) {
    this.toggleItem(id, (<HTMLInputElement>e.target).checked);
  }

  toggleItem(id: string, isChecked: boolean) {
    let elt = document.querySelector('.dataTable-dropdown');
    if (isChecked) {
      this.listUser.push(id);
    } else {
      let index = this.listUser.indexOf(id);
      this.listUser.splice(index, 1);
    }
    if (this.listUser.length != 0) {
      elt.classList.remove('d-none');
    } else {
      elt.classList.add('d-none');
    }
    console.log(this.listUser);
  }

  @ViewChildren('checkboxes') checkboxes: QueryList<ElementRef>;

  uncheckAll() {
    this.checkboxes.forEach((element) => {
      element.nativeElement.checked = false;
    });
  }

  validateUser(cif: string) {
    this.listUser.push(cif);
    this.validateAll();
  }

  rejectUser(cif: string) {
    this.listUser.push(cif);
    this.rejectAll();
  }

  deleteUser(cif: string) {
    this.listUser.push(cif);
    this.deleteAll();
  }

  validateAll() {
    this.userService.validateUsers(this.listUser).subscribe(
      res => {
        this.userItems.items.map(u => {
          if (this.listUser.includes(u.cif)) {
            u.status = '1';
          }
        });
        this.listUser.length = 0;
        Swal.fire({
          title: 'User Validated successfully!!!',
          icon: 'success',
        });
      },
      err => console.log(err)
    );
  }

  rejectAll() {
    this.userService.rejectUsers(this.listUser).subscribe(
      res => {
        this.userItems.items.forEach(u => {
          if (this.listUser.includes(u.cif)) {
            u.status = '2';
          }
        });
        this.listUser.length = 0;
        Swal.fire({
          title: 'User Rejected successfully!!!',
          icon: 'success',
        });
      },
      err => console.log(err)
    );
  }

  deleteAll() {
    this.userService.deleteUsers(this.listUser).subscribe(
      res => {
        this.userItems.items = this.userItems.items.filter(u => {
          return !this.listUser.includes(u.cif);
        });
        this.listUser.length = 0;
        Swal.fire({
          title: 'User Deleted successfully!!!',
          icon: 'success',
        });
      },
      err => console.log(err)
    );
  }

  setActiveClass(i: number) {
    let current = document.querySelector('.pagination-items.active');
    current.classList.remove('active');
    document.querySelectorAll('.pagination-items')[i].classList.add('active');
  }

  searchItem(e: Event) {
    let value = (<HTMLInputElement>e.target).value;
    if (value != '') {
      this.userService.searchUser(value).subscribe(
        res => {
          this.userItems.items = [...res];
        },
        err => {
          Swal.fire({
            title: 'There is a Problem!!!',
            text: err['error'],
            icon: 'error',
          });
        }
      );
    } else {
      this.getItems(this.currentPage);
    }
  }

}
