import {Component, OnInit} from '@angular/core';

@Component({
  selector: 'app-request-popup',
  templateUrl: './request-popup.component.html',
  styleUrls: ['./request-popup.component.css'],
})
export class RequestPopupComponent implements OnInit {
  constructor() {
  }

  ngOnInit(): void {
  }

  viewmessage(): void {
    const input = document.getElementById(
      'description'
    ) as HTMLInputElement | null;
    const value = input?.value;
    console.log(value);
  }
}
