import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {JwtService} from '../../shared/services/jwt.service';
import {BankAccountService} from '../../shared/services/bank-account.service';
import {TransactionModel} from '../../shared/models/transaction-model';
import {TransactionService} from '../../shared/services/transaction.service';

@Component({
  selector: 'app-global',
  templateUrl: './global.component.html',
  styleUrls: ['./global.component.css'],
})
export class GlobalComponent implements OnInit {
  userBalance = 0.0;
  userBankAccountId: number;
  transactions: TransactionModel[];

  constructor(
    private jwt: JwtService,
    private router: Router,
    private bankAccountService: BankAccountService,
    private transactionService: TransactionService
  ) {
    if (!this.jwt.isConnected()) {
      this.router.navigateByUrl('/login');
    }
    if (this.jwt.isAdmin() && this.jwt.switchBtn) {
      this.router.navigateByUrl('/dashboard/users');
    }
  }

  ngOnInit(): void {
    this.getUserBalance();
  }

  getUserBalance() {
    this.bankAccountService
      .getBankAccount(this.jwt.getConnectedUserId())
      .subscribe((next) => {
        this.userBalance = next.balance;
        this.userBankAccountId = next.id;
        this.getLastFiveTransactions();
      });
  }

  getLastFiveTransactions() {
    this.transactionService
      .getTransactionsByUser(this.userBankAccountId, 1, 5)
      .subscribe(this.processResult());
  }

  processResult() {
    return (data) => {
      this.transactions = data.response;
    };
  }

  statusCasting(status: number): string {
    switch (status) {
      case 0:
        return 'Progreso';
      case 1:
        return 'Aprobada';
      case 2:
        return 'Rechazada';
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

  importAsCsv() {
    this.transactionService.importAsCsv(this.userBankAccountId);
  }

  importAsExcel() {
    this.transactionService.importAsExcel(this.userBankAccountId);
  }

  importAsPDF() {
    this.transactionService.importAsPdf(this.userBankAccountId);
  }

}
