import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  selectedButton: String='user';
  constructor() { }

  ngOnInit(): void {
  }
  onSelected(s) : void {
    this.selectedButton = s;
  }
}
