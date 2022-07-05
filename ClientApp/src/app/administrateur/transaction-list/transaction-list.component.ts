import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-transaction-list',
  templateUrl: './transaction-list.component.html',
  styleUrls: ['./transaction-list.component.css']
})
export class TransactionListComponent implements OnInit {
  listTransaction: Array<number>
  constructor() {
    this.listTransaction = new Array<number>();
  }

  ngOnInit(): void {
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
