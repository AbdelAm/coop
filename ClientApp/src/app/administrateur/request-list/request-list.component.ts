import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { RequestPopupComponent } from '../../request-popup/request-popup.component';

@Component({
  selector: 'app-request-list',
  templateUrl: './request-list.component.html',
  styleUrls: ['./request-list.component.css']
})
export class RequestListComponent implements OnInit {
  listRequest: Array<number>
  constructor(public dialog: MatDialog) {
    this.listRequest = new Array<number>();
  }

  ngOnInit(): void {
    window.scrollTo(0, 0);
  }
  selectAll()
  {

  }
  setRequest(id: number, e: Event) {
    if ((<HTMLInputElement> e.target).checked) {
      this.listRequest.push(id);
    } else {
      let index = this.listRequest.indexOf(id);
      this.listRequest.splice(index, 1);
    }
  }
  addRequest(): void {
    this.dialog.open(RequestPopupComponent, {
      width: '60%',
      height: '60%',
      data: "right click"
    })
  }
}
