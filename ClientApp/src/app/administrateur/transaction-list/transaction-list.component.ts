import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { TransactionPopupComponent } from '../../transaction-popup/transaction-popup.component';

@Component({
  selector: 'app-transaction-list',
  templateUrl: './transaction-list.component.html',
  styleUrls: ['./transaction-list.component.css']
})
export class TransactionListComponent implements OnInit {
  listTransaction: Array<number>;
  constructor(public dialog:MatDialog) {
    this.listTransaction = new Array<number>();
  }

  addTransaction() : void {
    this.dialog.open(TransactionPopupComponent, {
      width: '50%',
      height:'50%',
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
  setRequest(id: number, e:Event) {
    this.toggleItem(id, (<HTMLInputElement> e.target).checked);
  }
  toggleItem(id: number, isChecked: boolean)
  {
    if (isChecked) {
      this.listTransaction.push(id);
    } else {
      let index = this.listTransaction.indexOf(id);
      this.listTransaction.splice(index, 1);
    }
    console.log(this.listTransaction);
  }
}
