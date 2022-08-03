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
  pageNumber = 1;
  pageSize = 10;
  totalElements = 100;
  maxSize = 5;

  constructor(public dialog: MatDialog, private jwt: JwtService, private router: Router, private transactionService: TransactionService) {
    (!this.jwt.isAdmin() || !this.jwt.switchBtn) ? this.router.navigateByUrl('transaction') : this.router.navigateByUrl('admin/transaction');
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
    this.getTransactionsByUser();
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
    const elt = document.querySelector('.dataTable-dropdown');
    if (isChecked) {
      this.listTransaction.push(id);
    } else {
      const index = this.listTransaction.indexOf(id);
      this.listTransaction.splice(index, 1);
    }
    if (this.listTransaction.length !== 0) {
      elt.classList.remove('d-none');
    } else {
      elt.classList.add('d-none');
    }
  }


  getTransactionsByUser() {
    this.transactionService.getTransactions(this.pageNumber, this.pageSize).subscribe(
      this.processResult()
    );
  }

  processResult() {
    return data => {
      this.transactions = data.response;
      this.pageNumber = data.pagination?.pageNumber;
      this.pageSize = data.pagination?.pageSize;
      this.totalElements = data.pagination?.totalRecords;
    };
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

  /*updateTransaction() {
    this.transactionService.updateTransaction();
  }
*/
  validateAllTransactions(listTransaction: Array<number>) {
    this.transactionService.validateAllTransaction(listTransaction).subscribe();

  }

  rejectAllTransactions(listTransaction: Array<number>) {
    this.transactionService.rejectAllTransaction(listTransaction).subscribe();
  }

  removeAllTransactions(listTransaction: Array<number>) {
    this.transactionService.removeAllTransaction(listTransaction).subscribe();
  }


  handleTransactionSearch(keyword: string) {

    if (keyword !== '') {
      this.transactionService.handleTransactionSearch(keyword).subscribe(
        this.processResult()
      );
    } else {
      this.getTransactionsByUser();
    }
  }

  statusCasting(status: number): string {
    switch (status) {
      case 0:
        return 'Progress';
      case 1:
        return 'Approved';
      case 2:
        return 'Rejected';

    }
  }
}
