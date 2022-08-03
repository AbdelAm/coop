import { Component, ElementRef, OnInit, QueryList, ViewChildren } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { RequestPopupComponent } from '../../request-popup/request-popup.component';
import { JwtService } from 'src/app/shared/services/jwt.service';
import { RequestServiceService } from 'src/app/shared/services/request-service.service';
import { RequestModel } from '../../shared/models/request-model';
import { StatusModel } from '../../shared/models/status-model';
import Swal from 'sweetalert2';
//import { parse } from 'path';
import { ItemsModel } from '../../shared/models/items-model';


@Component({
  selector: 'app-request-list',
  templateUrl: './request-list.component.html',
  styleUrls: ['./request-list.component.css']
})
export class RequestListComponent implements OnInit {
  readonly pageSize = 1;
  listRequest: Array<number>
  requestItems: ItemsModel<RequestModel>;
  requests: RequestModel[];
  pageNumber: Array<number>;

  status = [
    '<strong>In Progress</strong>',
    '<strong class="text-success">Approuved</strong>',
    '<strong class="text-danger text-capitalize">Rejected</strong>'
  ]



  constructor(public dialog: MatDialog, private jwt: JwtService, private router: Router, private requestService: RequestServiceService) {
    (!this.jwt.isAdmin() || !this.jwt.switchBtn) ? this.router.navigateByUrl('requests') : this.router.navigateByUrl('admin/requests');
    this.listRequest = new Array<number>();
    this.requests = new Array<RequestModel>();
    this.requestItems = new ItemsModel<RequestModel>();
    this.pageNumber = [];

  }

  ngOnInit(): void {
    this.requestService.getRequests().subscribe(
      res => {
        this.requests.push(...res);
        console.log(this.requests);
      },
      err => console.log(err)
    );
    window.scrollTo(0, 0);
  }

  getItems(num: number) {
    this.requestService.getRequests(num).subscribe(
      res => {
        Object.assign(this.requestItems, res);
        let result = Math.trunc(this.requestItems.itemsNumber / this.pageSize);
        if (this.requestItems.itemsNumber % this.pageSize != 0) {
          result++;
        }
        this.pageNumber = Array.from(Array(result).keys());
      },
      err => {
        if ([401, 403].includes(err["status"])) {
          this.router.navigate(['global']);
        } else {
          Swal.fire({
            title: "There is a Problem!!!",
            text: err["error"],
            icon: "error"
          })
        }
      }
    );
    window.scrollTo(0, 0);
  }


  selectAll(e: Event)
  {
    let items = document.querySelectorAll('.items');
    for(let i = 0; i < items.length; i++)
    {
      (<HTMLInputElement> items[i]).checked = (<HTMLInputElement> e.target).checked;
      let id = parseInt((<HTMLInputElement> items[i]).value);
      this.toggleItem(id, (<HTMLInputElement> e.target).checked);
    }

  }
  setRequest(id: number, e:Event) {
    this.toggleItem(id, (<HTMLInputElement> e.target).checked);
  }
  toggleItem(id: number, isChecked: boolean)
  {
    let elt = document.querySelector(".dataTable-dropdown");
    if (isChecked) {
      this.listRequest.push(id);
    } else {
      let index = this.listRequest.indexOf(id);
      this.listRequest.splice(index, 1);
    }
    if(this.listRequest.length != 0)
    {
      elt.classList.remove("d-none");
    } else {
      elt.classList.add("d-none");
    }
  }
  @ViewChildren("checkboxes") checkboxes: QueryList<ElementRef>;

  addRequest(): void {
    this.dialog.open(RequestPopupComponent, {
      width: '60%',
      height: '60%',
      data: "right click"
    })
  }
  uncheckAll() {
    this.checkboxes.forEach((element) => {
      element.nativeElement.checked = false;
    });
  }
  validateRequest(id: number) {
    this.listRequest.push(id);
    this.validateAll();
  }
  rejectRequest(id: number) {
    this.listRequest.push(id);
    this.rejectAll();
  }
  deleteRequest(id: number) {
    this.listRequest.push(id);
    this.deleteAll();
  }
  validateAll() {
    console.log(this.listRequest);
    this.requestService.validateRequests(this.listRequest).subscribe(
      res => {
        this.requests.map(req => {
          if(this.listRequest.includes(req.id)) {
            req.status = StatusModel.Approuved;
          }
        });
        this.listRequest.length = 0;
        Swal.fire({
          title: "Request Validated successfully!!!",
          icon: "success",
        });
      },
      err => console.log(err)
    )
  }
  rejectAll() {
    console.log(this.listRequest);
    this.requestService.rejectRequests(this.listRequest).subscribe(
      res => {
        this.requests.map(req => {
          if (this.listRequest.includes(req.id)) {
            req.status = StatusModel.Rejected;
          }
        });
        this.listRequest.length = 0;
        Swal.fire({
          title: "Request Rejected successfully!!!",
          icon: "success",
        });
      },
      err => console.log(err)
    )
  }

  deleteAll() {
    this.requestService.deleteRequests(this.listRequest).subscribe(
      res => {
        this.requests = this.requests.filter(u => {
          return !this.listRequest.includes(u.id);
        });
        this.listRequest.length = 0;
        Swal.fire({
          title: "Request Deleted successfully!!!",
          icon: "success",
        });
      },
      err => console.log(err)
    )
  }
  
  searchItem(e: Event) {
    let value =(<HTMLInputElement>e.target).value;
    this.requestService.searchRequest(value).subscribe(
      res => console.log(res),
      err => console.log(err)
    )
  }
}
