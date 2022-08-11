import {Component, OnInit} from '@angular/core';
import {MatDialog} from '@angular/material/dialog';
import {JwtService} from '../../shared/services/jwt.service';
import {Router} from '@angular/router';
import {TransactionModel} from '../../shared/models/transaction-model';
import {TransactionService} from '../../shared/services/transaction.service';
import Swal from 'sweetalert2';
import {TransactionPopupComponent} from '../transaction-popup/transaction-popup.component';
import {BankAccountService} from 'src/app/shared/services/bank-account.service';
import {UserService} from '../../shared/services/user-service.service';

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
  isConnected = false;
  hasAdminRole = false;
  userBankAccountId: number;
  switchBtn: boolean;
  userName: string;

  constructor(public dialog: MatDialog, private jwt: JwtService, private router: Router, private transactionService: TransactionService, private bankService: BankAccountService, private userService: UserService) {
    this.transactions = [];
    this.listTransaction = [];
    this.isConnected = this.jwt.isConnected();
    this.hasAdminRole = this.jwt.isAdmin();
    this.switchBtn = this.jwt.switchBtn;
  }

  addTransaction(): void {
    this.dialog.open(TransactionPopupComponent, {
      width: '60%',
      data: 'right click'
    });
  }

  ngOnInit(): void {
    window.scrollTo(0, 0);
    this.bankService.getBankAccount(this.jwt.getConnectedUserId()).subscribe(
      res => {
        this.userBankAccountId = res.id;
        this.loadTransactionsByRole();
      },
      err => console.log(err)
    );
    this.userService.getUser(this.jwt.getConnectedUserId()).subscribe(
      res => this.userName = res.name
    );
  }

  loadTransactionsByRole() {
    console.log(this.switchBtn, this.hasAdminRole);
    if (this.isConnected && this.hasAdminRole && this.switchBtn) {
      this.getTransactions();
    }
    if (this.isConnected) {
      this.getTransactionsByUser();
    } else {
      this.router.navigateByUrl('login');
    }
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


  getTransactions() {
    this.transactionService.getTransactions(this.pageNumber, this.pageSize).subscribe(
      this.processResult()
    );

  }

  getTransactionsByUser() {
    this.transactionService.getTransactionsByUser(this.userBankAccountId, this.pageNumber, this.pageSize).subscribe(this.processResult()
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
    this.transactionService.validateTransaction(transactionId).subscribe(
      next => {
        Swal.fire({
          icon: 'success',
          title: 'Transaction has successfully validated ',
          showConfirmButton: false,
          timer: 1000
        });
        this.transactions = [];
        this.loadTransactionsByRole();
      },
      error => {
        Swal.fire({
          icon: 'error',
          title: 'Transaction cannot be validated',
          showConfirmButton: false,
          timer: 1000
        });
      }
    );
  }

  rejectTransaction(transactionId: number) {
    this.transactionService.rejectTransaction(transactionId).subscribe(
      next => {
        Swal.fire({
          icon: 'success',
          title: 'Transaction has successfully rejected ',
          showConfirmButton: false,
          timer: 1000
        });
        this.transactions = [];
        this.loadTransactionsByRole();
      },
      error => {
        Swal.fire({
          icon: 'error',
          title: 'Transaction cannot be rejected',
          showConfirmButton: false,
          timer: 1000
        });
      }
    );
  }

  removeTransaction(transactionId: number) {
    this.transactionService.removeTransaction(transactionId).subscribe(
      next => {
        Swal.fire({
          icon: 'success',
          title: 'Transaction has successfully removed ',
          showConfirmButton: false,
          timer: 1000
        });
        this.transactions = [];
        this.loadTransactionsByRole();
      },
      error => {
        Swal.fire({
          icon: 'error',
          title: 'Transaction cannot be deleted',
          showConfirmButton: false,
          timer: 1000
        });
      }
    );

  }


  validateAllTransactions(listTransaction: Array<number>) {
    this.transactionService.validateAllTransaction(listTransaction).subscribe(
      next => {
        this.transactions = [];
        this.loadTransactionsByRole();
      }
    );

  }

  rejectAllTransactions(listTransaction: Array<number>) {
    this.transactionService.rejectAllTransaction(listTransaction).subscribe(
      next => {
        this.transactions = [];
        this.loadTransactionsByRole();
      }
    )
    ;
  }

  removeAllTransactions(listTransaction: Array<number>) {
    this.transactionService.removeAllTransaction(listTransaction).subscribe(
      next => {
        this.transactions = [];
        this.loadTransactionsByRole();
      }
    );
  }


  handleTransactionSearch(keyword: string) {

    if (keyword !== '') {
      this.transactionService.handleTransactionSearch(keyword, this.pageNumber, this.pageSize).subscribe(
        this.processResult()
      );
    } else {
      this.getTransactions();
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

  statusColor(status: number) {
    switch (status) {
      case 1:
        return 'green';
      case 2:
        return 'red';

    }
  }
}
