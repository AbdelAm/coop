import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormControl, Validators} from '@angular/forms';
import {STEPPER_GLOBAL_OPTIONS} from '@angular/cdk/stepper';

import Swal from 'sweetalert2';
import {CoopValidators} from '../../shared/validators/coopValidators';
import {JwtService} from '../../shared/services/jwt.service';
import {TransactionService} from '../../shared/services/transaction.service';
import {TransactionModel} from '../../shared/models/transaction-model';
import {BankAccountService} from 'src/app/shared/services/bank-account.service';
import {TransactionPostModel} from '../../shared/models/Transaction-post-model';

@Component({
  selector: 'app-transaction-popup',
  templateUrl: './transaction-popup.component.html',
  styleUrls: ['./transaction-popup.component.css'],
  providers: [
    {
      provide: STEPPER_GLOBAL_OPTIONS,
      useValue: {displayDefaultIndicatorType: false},
    },
  ],
})
export class TransactionPopupComponent implements OnInit {
  userBankAccountNumber: string;
  readonly hasAdminRole: boolean;
  readonly isConnected: boolean;
  readonly switchBtn: boolean;

  transactionFormGroup = this._formBuilder.group({
    origin: this._formBuilder.group({
      originalAccount: new FormControl(''),
      receiverAccount: new FormControl('', [
        Validators.required,
        CoopValidators.notOnlyWhiteSpace,
      ]),
      externalAccount: new FormControl(''),
    }),
    destination: this._formBuilder.group({
      destinationAccount: new FormControl('', [
        Validators.required,
        CoopValidators.notOnlyWhiteSpace,
      ]),
      externalDestination: new FormControl(''),
    }),
    receiverInfo: this._formBuilder.group({
      amount: new FormControl('', [
        Validators.required,
        Validators.pattern('^-?[0-9]\\d*(\\.\\d{1,2})?$'),
        Validators.min(0),
      ]),
      concept: new FormControl('', [
        Validators.required,
        CoopValidators.notOnlyWhiteSpace,
      ]),
    }),
  });

  constructor(
    private _formBuilder: FormBuilder,
    private jwt: JwtService,
    private transactionService: TransactionService,
    private bankService: BankAccountService
  ) {
    this.isConnected = jwt.isConnected();
    this.hasAdminRole = jwt.isAdmin();
    this.switchBtn = this.jwt.switchBtn;
  }

  ngOnInit(): void {
    this.bankService.getBankAccount(this.jwt.getConnectedUserId()).subscribe(
      (res) => {
        this.userBankAccountNumber = res.accountNumber;
      },
      (err) => console.log(err)
    );
  }

  onSubmit() {
    if (this.transactionFormGroup.invalid) {
      this.transactionFormGroup.markAllAsTouched();
      return;
    }

    const transaction = new TransactionPostModel();
    transaction.senderBankAccountNumber = this.transactionFormGroup.get(
      'origin.originalAccount'
    ).value
      ? this.transactionFormGroup.get('origin.originalAccount').value
      : this.transactionFormGroup.get('origin.receiverAccount').value;
    transaction.receiverBankAccountNumber = this.transactionFormGroup.get(
      'destination.destinationAccount'
    ).value;
    transaction.amount = this.transactionFormGroup.get(
      'receiverInfo.amount'
    ).value;
    transaction.motif = this.transactionFormGroup.get(
      'receiverInfo.concept'
    ).value;
    this.transactionService.postTransaction(transaction).subscribe(
      (next) => {
        Swal.fire({
          icon: 'success',
          title: 'Transacción agregada con éxito',
          showConfirmButton: false,
          timer: 1000,
        });
        this.transactionService.refresh();
        document.getElementById('closeDialog').click();
      },
      (error) => {
        Swal.fire({
          icon: 'error',
          title: 'Algo anda mal con tus entradas',
          showConfirmButton: false,
          timer: 1000,
        });
      }
    );
  }

  get concept() {
    return this.transactionFormGroup.get('receiverInfo.concept');
  }

  get amount() {
    return this.transactionFormGroup.get('receiverInfo.amount');
  }

  get receiverAccount() {
    return this.transactionFormGroup.get('origin.receiverAccount');
  }

  get externalAccount() {
    return this.transactionFormGroup.get('origin.externalAccount');
  }

  get destination() {
    return this.transactionFormGroup.get('destination.destinationAccount');
  }

  get externalDestination() {
    return this.transactionFormGroup.get('destination.externalDestination');
  }

  disableOriginInput(event) {
    if (event.target.checked) {
      this.transactionFormGroup.get('origin.externalAccount').disable();
      this.transactionFormGroup.get('origin.receiverAccount').disable();
      this.transactionFormGroup.get('origin.externalAccount').reset();
      this.transactionFormGroup.get('origin.receiverAccount').reset();
    }
  }

  enableExternalInput(event) {
    if (event.target.value) {
      this.transactionFormGroup.get('origin.externalAccount').enable();
    }
  }

  enableReceiverInput(event) {
    if (event.target.value) {
      this.transactionFormGroup.get('origin.receiverAccount').enable();
    }
  }

  disableExternalInput(event) {
    if (event.target.value) {
      this.transactionFormGroup.get('destination.destinationAccount').enable();
      this.transactionFormGroup
        .get('destination.externalDestination')
        .disable();
      this.transactionFormGroup.get('destination.externalDestination').reset();
    }
  }

  disableDestinationInput(event) {
    if (event.target.value) {
      this.transactionFormGroup.get('destination.externalDestination').enable();
      this.transactionFormGroup.get('destination.destinationAccount').disable();
      this.transactionFormGroup.get('destination.destinationAccount').reset();
    }
  }
}
