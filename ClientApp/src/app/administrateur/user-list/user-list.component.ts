import { Component, ElementRef, OnInit, QueryList, ViewChildren } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { UserItemModel } from 'src/app/shared/models/user-item-model';
import { UserItems } from 'src/app/shared/models/user-items';
import { JwtService } from 'src/app/shared/services/jwt.service';
import { UserService } from 'src/app/shared/services/user-service.service';
import { TransactionPopupComponent } from 'src/app/transaction-popup/transaction-popup.component';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {

  listUser: Array<string>;
  userItems: UserItems;
  pageNumber: Array<number>;
  status = [
    '<strong>In Progress</strong>',
    '<strong class="text-success">Approuved</strong>',
    '<strong class="text-danger text-capitalize">Rejected</strong>'
  ]
  
  constructor(public dialog:MatDialog, private jwt:JwtService, private router: Router, private userService: UserService) {
    if(!this.jwt.isAdmin() || !this.jwt.switchBtn) {
      this.router.navigate(['global']);
    }
    this.listUser = new Array<string>();
    this.userItems = new UserItems();
    this.pageNumber = [];
  }

  ngOnInit(): void {
    this.getItems(0);
  }
  selectAll(e: Event)
  {
    let items = document.querySelectorAll('.items');
    for(let i = 0; i < items.length; i++)
    {
      (<HTMLInputElement> items[i]).checked = (<HTMLInputElement> e.target).checked;
      let id = (<HTMLInputElement> items[i]).value;
      this.toggleItem(id, (<HTMLInputElement> e.target).checked);
    }

  }
  addUser() : void {
    this.dialog.open(TransactionPopupComponent, {
      width: '60%',
      data: "right click"
    })
  }
  setUser(id: string, e:Event) {
    this.toggleItem(id, (<HTMLInputElement> e.target).checked);
  }
  toggleItem(id: string, isChecked: boolean)
  {
    let elt = document.querySelector('.dataTable-dropdown');
    if (isChecked) {
      this.listUser.push(id);
    } else {
      let index = this.listUser.indexOf(id);
      this.listUser.splice(index, 1);
    }
    if(this.listUser.length != 0)
    {
      elt.classList.remove("d-none");
    } else {
      elt.classList.add("d-none");
    }
    console.log(this.listUser);
  }
  @ViewChildren("checkboxes") checkboxes: QueryList<ElementRef>;

  uncheckAll() {
    this.checkboxes.forEach((element) => {
      element.nativeElement.checked = false;
    });
  }

  validateUser(cif: string)
  {
    this.listUser.push(cif);
    this.validateAll();
  }
  rejectUser(cif: string)
  {
    this.listUser.push(cif);
    this.rejectAll();
  }
  deleteUser(cif: string)
  {
    this.listUser.push(cif);
    this.deleteAll();
  }
  validateAll()
  {
    this.userService.validateUsers(this.listUser).subscribe(
      res => {
        this.userItems.users = this.userItems.users.filter(u => {
          return !this.listUser.includes(u.cif);
        });
        this.listUser.length = 0;
        Swal.fire({
          title: "User Validated successfully!!!",
          icon: "success",
        });
      },
      err => console.log(err)
    )
  }
  rejectAll()
  {
    this.userService.rejectUsers(this.listUser).subscribe(
      res => {
        this.userItems.users.forEach(u => {
          if(this.listUser.includes(u.cif)) {
            u.status = '2';
          }
        })
        this.listUser.length = 0;
        Swal.fire({
          title: "User Rejected successfully!!!",
          icon: "success",
        });
      },
      err => console.log(err)
    )
  }
  deleteAll()
  {
    this.userService.deleteUsers(this.listUser).subscribe(
      res => {
        this.userItems.users = this.userItems.users.filter(u => {
          return !this.listUser.includes(u.cif);
        });
        this.listUser.length = 0;
        Swal.fire({
          title: "User Deleted successfully!!!",
          icon: "success",
        });
      },
      err => console.log(err)
    )
  }

  getItems(num: number)
  {
    this.userService.getUsers(num).subscribe(
      res => {
        Object.assign(this.userItems, res);
        let result = Math.trunc(this.userItems.userNumber / 1);
        if(this.userItems.userNumber % 1 != 0) {
          result++;
        }
        this.pageNumber = Array.from(Array(result).keys());
      },
      err => {
        if([401,403].includes(err["status"])) {
            this.router.navigate(['global']);
        } else {
          Swal.fire({
            title: "There is a Problem!!!",
            text: err["error"],
            icon: "error"
          })
        }
      }
    );
    window.scrollTo(0, 0);
  }

  setActiveClass(i: number)
  {
    let current = document.querySelector(".pagination-items.active");
    current.classList.remove("active");
    document.querySelectorAll('.pagination-items')[i].classList.add("active");
  }

}
