import {Component, OnInit} from '@angular/core';
import {MatDialog} from '@angular/material/dialog';
import {TransactionPopupComponent} from '../../transaction-popup/transaction-popup.component';
import {JwtService} from '../../shared/services/jwt.service';
import {Router} from '@angular/router';
import {TransactionModel} from '../../shared/models/transaction-model';
import {TransactionService} from '../../shared/services/transaction.service';

@Component({
  selector: 'app-transaction-list',
  templateUrl: './transaction-list.component.html',
  styleUrls: ['./transaction-list.component.css']
})
export class TransactionListComponent implements OnInit {
  listTransaction: number[];
  transactions: TransactionModel[];

  constructor(public dialog: MatDialog, private jwt: JwtService, private router: Router, private transactionService: TransactionService) {
    (!this.jwt.isAdmin() || !this.jwt.switchBtn) ? this.router.navigateByUrl('transactions') : this.router.navigateByUrl('admin/transactions');
    this.transactions = [];
    this.listTransaction = [];
  }

  addTransaction(): void {
    this.dialog.open(TransactionPopupComponent, {
      width: '60%',
      data: 'right click'
    });
  }

  ngOnInit(): void {
    window.scrollTo(0, 0);
    this.getTransactions();
  }

  selectAll(e: Event) {
    const items = document.querySelectorAll('.items');
    for (let i = 0; i < items.length; i++) {
      (<HTMLInputElement>items[i]).checked = (<HTMLInputElement>e.target).checked;
      const id = parseInt((<HTMLInputElement>items[i]).value);
      this.toggleItem(id, (<HTMLInputElement>e.target).checked);
    }

  }

  setTransaction(id: number, e: Event) {
    this.toggleItem(id, (<HTMLInputElement>e.target).checked);
  }

  toggleItem(id: number, isChecked: boolean) {
    let elt = document.querySelector('.dataTable-dropdown');
    if (isChecked) {
      this.listTransaction.push(id);
    } else {
      let index = this.listTransaction.indexOf(id);
      this.listTransaction.splice(index, 1);
    }
    if (this.listTransaction.length != 0) {
      elt.classList.remove('d-none');
    } else {
      elt.classList.add('d-none');
    }
  }


  getTransactions() {
    this.transactionService.getTransactions().subscribe(
      data => this.transactions = data
    );
  }


  validateTransaction(transactionId: number) {
    this.transactionService.validateTransaction(transactionId).subscribe();
  }

  rejectTransaction(transactionId: number) {
    this.transactionService.rejectTransaction(transactionId).subscribe();

  }

  removeTransaction(transactionId: number) {
    this.transactionService.removeTransaction(transactionId).subscribe();

  }

  updateTransaction() {
    this.transactionService.updateTransaction();
  }

  validateAllTransactions(listTransaction: Array<number>) {
    this.transactionService.validateAllTransaction(listTransaction).subscribe();

  }

  rejectAllTransactions(listTransaction: Array<number>) {
    this.transactionService.rejectAllTransaction(listTransaction).subscribe();
  }

  removeAllTransactions(listTransaction: Array<number>) {
    this.transactionService.removeAllTransaction(listTransaction).subscribe();
  }
}
