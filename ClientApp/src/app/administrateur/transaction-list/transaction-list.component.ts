import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { JwtService } from 'src/app/shared/services/jwt.service';
import { TransactionPopupComponent } from '../../transaction-popup/transaction-popup.component';

@Component({
  selector: 'app-transaction-list',
  templateUrl: './transaction-list.component.html',
  styleUrls: ['./transaction-list.component.css']
})
export class TransactionListComponent implements OnInit {
  listTransaction: Array<number>;
  constructor(public dialog:MatDialog, private jwt: JwtService, private router: Router) {
    (!this.jwt.isAdmin() || !this.jwt.switchBtn) ? this.router.navigateByUrl('transactions') : this.router.navigateByUrl('admin/transactions');
    this.listTransaction = new Array<number>();
  }

  addTransaction() : void {
    this.dialog.open(TransactionPopupComponent, {
      width: '60%',
      data: "right click"
    })
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
  setTransaction(id: number, e:Event) {
    this.toggleItem(id, (<HTMLInputElement> e.target).checked);
  }
  toggleItem(id: number, isChecked: boolean)
  {
    let elt = document.querySelector('.dataTable-dropdown');
    if (isChecked) {
      this.listTransaction.push(id);
    } else {
      let index = this.listTransaction.indexOf(id);
      this.listTransaction.splice(index, 1);
    }
    if(this.listTransaction.length != 0)
    {
      elt.classList.remove("d-none");
    } else {
      elt.classList.add("d-none");
    }
  }
}
