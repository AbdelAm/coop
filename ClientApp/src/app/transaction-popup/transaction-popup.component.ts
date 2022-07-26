import {Component, OnInit} from '@angular/core';
import {FormBuilder, Validators} from '@angular/forms';
import {STEPPER_GLOBAL_OPTIONS} from '@angular/cdk/stepper';

@Component({
  selector: 'app-transaction-popup',
  templateUrl: './transaction-popup.component.html',
  styleUrls: ['./transaction-popup.component.css'],
  providers: [
    {
      provide: STEPPER_GLOBAL_OPTIONS,
      useValue: {showError: true},
    },
  ],
})
export class TransactionPopupComponent implements OnInit {

  firstFormGroup = this._formBuilder.group({
    originalAccount: ['', Validators.required],
    amount: ['', Validators.required],

    destinationName: ['', Validators.required],
    concept: ['', Validators.required],
    transactionTimestamp: ['', Validators.required],
  });


  constructor(private _formBuilder: FormBuilder) {
  }

  ngOnInit(): void {
  }

  submitTransaction() {
    console.log(this.firstFormGroup.value);
  }
}
