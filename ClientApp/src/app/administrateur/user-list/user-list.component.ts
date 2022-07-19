import { Component, ElementRef, OnInit, QueryList, ViewChildren } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { TransactionPopupComponent } from 'src/app/transaction-popup/transaction-popup.component';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {

  listUser: Array<number>
  constructor(public dialog:MatDialog) {
    this.listUser = new Array<number>();
  }

  ngOnInit(): void {
    window.scrollTo(0, 0);
  }
  selectAll(e: Event)
  {
    let items = document.querySelectorAll('.items');
    for(let i = 0; i < items.length; i++)
    {
      (<HTMLInputElement> items[i]).checked = (<HTMLInputElement> e.target).checked;
      let id = parseInt((<HTMLInputElement> items[i]).value);
      this.toggleItem(id, (<HTMLInputElement> e.target).checked);
    }

  }
  addUser() : void {
    this.dialog.open(TransactionPopupComponent, {
      width: '60%',
      data: "right click"
    })
  }
  setUser(id: number, e:Event) {
    this.toggleItem(id, (<HTMLInputElement> e.target).checked);
  }
  toggleItem(id: number, isChecked: boolean)
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
}
