import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormControl, Validators} from '@angular/forms';
import {STEPPER_GLOBAL_OPTIONS} from '@angular/cdk/stepper';
import {JwtService} from '../shared/services/jwt.service';
import {CoopValidators} from '../shared/validators/coopValidators';

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
  readonly connectedUserId: string;

  transactionFormGroup = this._formBuilder.group({
    origin: this._formBuilder.group({
      originalAccount: new FormControl(''),
      externalAccount: new FormControl(''),
    }),
    destination: this._formBuilder.group({
      originalAccountDestination: new FormControl(''),
      externalDestination: new FormControl(''),
      destinationAccount: new FormControl(''),
    }),
    receiverInfo: this._formBuilder.group({
      amount: new FormControl('', [
        Validators.required,
        Validators.pattern('^-?[0-9]\\d*(\\.\\d{1,2})?$'),
        Validators.min(0),
      ]),
      destinationName: new FormControl('', [
        Validators.required,
        CoopValidators.notOnlyWhiteSpace,
      ]),
      concept: new FormControl('', [
        Validators.required,
        CoopValidators.notOnlyWhiteSpace,
      ]),
    }),
  });

  constructor(private _formBuilder: FormBuilder, private jwt: JwtService) {
    this.connectedUserId = jwt.getConnectedUserId();
  }

  ngOnInit(): void {
  }

  get destinationName() {
    return this.transactionFormGroup.get('receiverInfo.destinationName');
  }

  get concept() {
    return this.transactionFormGroup.get('receiverInfo.concept');
  }

  get amount() {
    return this.transactionFormGroup.get('receiverInfo.amount');
  }

  get transactionTimestamp() {
    return this.transactionFormGroup.get('receiverInfo.transactionTimestamp');
  }

  onSubmit() {
    console.log(this.transactionFormGroup.value);
  }

  disableOriginInput(event) {
    if (event.target.checked) {
      this.transactionFormGroup.get('origin.externalAccount').disable();
      this.transactionFormGroup.get('origin.externalAccount').reset();
    }
  }

  enableOriginInput(event) {
    if (event.target.value) {
      this.transactionFormGroup.get('origin.externalAccount').enable();
    }
  }

  disableAllDestinationInputs(event) {
    if (event.target.value) {
      this.transactionFormGroup.get(
        'destination.destinationAccount'
      ).disable();
      this.transactionFormGroup.get(
        'destination.externalDestination'
      ).disable();
      this.transactionFormGroup.get(
        'destination.destinationAccount'
      ).reset();
      this.transactionFormGroup.get(
        'destination.externalDestination'
      ).reset();
    }
  }

  disableExternalInput(event) {
    if (event.target.value) {
      this.transactionFormGroup.get(
        'destination.destinationAccount'
      ).enable();
      this.transactionFormGroup.get(
        'destination.externalDestination'
      ).disable();
      this.transactionFormGroup.get(
        'destination.externalDestination'
      ).reset();
    }
  }

  disableDestinationInput(event) {
    if (event.target.value) {
      this.transactionFormGroup.get(
        'destination.externalDestination'
      ).enable();
      this.transactionFormGroup.get(
        'destination.destinationAccount'
      ).disable();
      this.transactionFormGroup.get(
        'destination.destinationAccount'
      ).reset();
    }
  }
}
