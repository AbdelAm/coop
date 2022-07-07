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
  selectAll()
  {

  }
  setTransaction(id: number, e: Event) {
    if ((<HTMLInputElement> e.target).checked) {
      this.listTransaction.push(id);
    } else {
      let index = this.listTransaction.indexOf(id);
      this.listTransaction.splice(index, 1);
    }
    console.log(this.listTransaction);
  }
}
