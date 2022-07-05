import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-request-list',
  templateUrl: './request-list.component.html',
  styleUrls: ['./request-list.component.css']
})
export class RequestListComponent implements OnInit {

  listRequest: Array<number>
  constructor() {
    this.listRequest = new Array<number>();
  }

  ngOnInit(): void {
  }
  selectAll()
  {

  }
  setTransaction(id: number, e: Event) {
    if ((<HTMLInputElement> e.target).checked) {
      this.listRequest.push(id);
    } else {
      let index = this.listRequest.indexOf(id);
      this.listRequest.splice(index, 1);
    }
  }
}
