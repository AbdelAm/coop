import {
  Component,
  ElementRef,
  OnInit,
  QueryList,
  ViewChildren,
} from '@angular/core';
import {Router} from '@angular/router';
import {JwtService} from 'src/app/shared/services/jwt.service';
import {RequestServiceService} from 'src/app/shared/services/request-service.service';
import {RequestModel} from '../../shared/models/request-model';
import {StatusModel} from '../../shared/models/status-model';
import Swal from 'sweetalert2';
import {ItemsModel} from '../../shared/models/items-model';
import {NgbModal, ModalDismissReasons} from '@ng-bootstrap/ng-bootstrap';
import {UserService} from '../../shared/services/user-service.service';

@Component({
  selector: 'app-request-list',
  templateUrl: './request-list.component.html',
  styleUrls: ['./request-list.component.css'],
})
export class RequestListComponent implements OnInit {
  ConnectedUserId: string;

  listRequest: Array<number>;
  requests: RequestModel[];
  pageNumber = 1;
  pgNumber = [];
  pageSize = 5;
  totalElements = 100;
  isConnected: boolean;
  hasAdminRole: boolean;
  switchBtn: boolean;
  request: RequestModel;
  requestItems: ItemsModel<RequestModel>;
  status = [
    '<strong>Progreso</strong>',
    '<strong class="text-success">Aprobada</strong>',
    '<strong class="text-danger text-capitalize">Rechazada</strong>'
  ];
  state: number;
  witness: boolean;
  currentPage: number;

  constructor(private jwt: JwtService, private router: Router, private requestService: RequestServiceService, private modalService: NgbModal) {

    this.listRequest = [];
    this.requests = [];
    this.isConnected = this.jwt.isConnected();
    this.hasAdminRole = this.jwt.isAdmin();
    this.switchBtn = this.jwt.switchBtn;
    this.requestItems = new ItemsModel<RequestModel>();
    this.ConnectedUserId = this.jwt.getConnectedUserId();
    this.request = new RequestModel();
    this.state = 2;
    this.witness = false;
    this.pgNumber = [];
  }

  ngOnInit(): void {
    this.loadRequestsByRole();
    window.scrollTo(0, 0);
  }

  getItems(num: number) {
    this.requestService.getRequests(num, this.pageSize).subscribe(
      (res) => {
        Object.assign(this.requestItems, res);
        this.requestService.progressNumber = this.requestItems.progressNumber;
        console.log(this.requestService.progressNumber);
        let result = Math.trunc(this.requestItems.itemsNumber / this.pageSize);
        if (this.requestItems.itemsNumber % this.pageSize != 0) {
          result++;
        }
        this.pgNumber = Array.from(Array(result).keys());
        this.currentPage = num;
      },
      (err) => {
        if ([401, 403].includes(err['status'])) {
          this.router.navigateByUrl('/dashboard/global');
        } else {
          Swal.fire({
            title: 'There is a Problem!!!',
            text: err['error'],
            icon: 'error',
          });
        }
      }
    );
    window.scrollTo(0, 0);
  }


  filterByApproved() {
    this.requestService.FilterRequest(StatusModel.Approuved, this.pageNumber, this.pageSize).subscribe(
      this.processResult()
    );
    this.state = 1;
    return this.witness = true;
  }

  filterByProgress() {
    this.requestService.FilterRequest(StatusModel.Progress, this.pageNumber, this.pageSize).subscribe(
      this.processResult()
    );
    this.state = 0;
    return this.witness = true;
  }


  getRequests() {
    this.requestService.getRequests(this.pageNumber, this.pageSize).subscribe(
      this.processResult()
    );
    this.witness = false;
  }

  getRequestsByUser() {
    this.requestService.getRequestsByUser(this.ConnectedUserId, this.pageNumber, this.pageSize).subscribe(
      this.processResult()
    );
  }

  processResult() {
    return data => {
      this.requests = data.response;
      this.pageNumber = data.pagination?.pageNumber;
      this.pageSize = data.pagination?.pageSize;
      this.totalElements = data.pagination?.totalRecords;
    };
  }


  loadRequestsByRole() {

    if (this.isConnected && this.hasAdminRole) {
      if (this.switchBtn) {
        if (this.witness == true && this.state == 0) {
          this.filterByProgress();
        } else if (this.witness == true && this.state == 1) {
          this.filterByApproved();
        } else if (this.witness == false && this.state != 0 && this.state != 1) {
          this.getRequests();
        } else {
          this.getRequests();
        }

      }
    } else {
      if (this.isConnected && !this.hasAdminRole) {
        this.getRequestsByUser();
      } else {
        this.router.navigateByUrl('/login');
      }
    }
  }


  setRequest() {

    const input = document.getElementById('msg') as HTMLTextAreaElement | null;
    const elem = document.getElementById('optdata') as HTMLSelectElement;

    const sel = elem.selectedIndex;
    const opt = elem.options[sel];

    this.request.type = opt.value.toString();
    this.request.message = input.value;
    this.request.userId = this.ConnectedUserId;

    this.requestService.setRequest(this.request).subscribe(
      (res) => {
        this.requests.push(this.request);
        this.requestService.progressNumber = this.requestService.progressNumber + 1;
        this.modalService.dismissAll();
        Swal.fire({
          title: '¡Solicitud añadida con éxito!',
          icon: 'success',
          showConfirmButton: false,
          timer: 1500
        });

        this.loadRequestsByRole();
      },
      (err) => console.log(err)
    );

  }

  toggleItem(id: number, isChecked: boolean) {
    let elt = document.querySelector('.dataTable-dropdown');
    if (isChecked) {
      this.listRequest.push(id);
    } else {
      let index = this.listRequest.indexOf(id);
      this.listRequest.splice(index, 1);
    }
    if (this.listRequest.length != 0) {
      elt.classList.remove('d-none');
    } else {
      elt.classList.add('d-none');
    }

  }

  selectAll(e: Event) {
    let items = document.querySelectorAll('.items');
    for (let i = 0; i < items.length; i++) {
      (<HTMLInputElement>items[i]).checked = (<HTMLInputElement>(
        e.target
      )).checked;
      let id = (<HTMLInputElement>items[i]).value;
      this.toggleItem(parseInt(id), (<HTMLInputElement>e.target).checked);
    }
  }

  @ViewChildren('checkboxes') checkboxes: QueryList<ElementRef>;

  uncheckAll() {
    this.checkboxes.forEach((element) => {
      element.nativeElement.checked = false;
    });
  }

  setrequestTogg(id: number, e: Event) {
    this.toggleItem(id, (<HTMLInputElement>e.target).checked);
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
    this.loadRequestsByRole();
  }

  validateAll() {

    this.requestService.validateRequests(this.listRequest).subscribe(
      (res) => {
        this.requests.map((req) => {
          if (this.listRequest.includes(req.id)) {
            req.status = StatusModel.Approuved;
          }
        });
        this.requestService.progressNumber = this.requestService.progressNumber - this.listRequest.length;
        this.listRequest.length = 0;
        Swal.fire({
          title: 'La Solicitud ha sido validada con éxito!',
          icon: 'success',
          showConfirmButton: false,
          timer: 1500
        });
      },
      (err) => console.log(err)
    );
  }

  rejectAll() {

    this.requestService.rejectRequests(this.listRequest).subscribe(
      (res) => {
        this.requests.map((req) => {
          if (this.listRequest.includes(req.id)) {
            req.status = StatusModel.Rejected;
          }
        });
        this.requestService.progressNumber = this.requestService.progressNumber - this.listRequest.length;
        this.listRequest.length = 0;
        Swal.fire({
          title: 'La solicitud ha sido rechazada!',
          icon: 'success',
          showConfirmButton: false,
          timer: 1500
        });
      },
      (err) => console.log(err)
    );
  }

  deleteAll() {
    this.requestService.deleteRequests(this.listRequest).subscribe(
      (res) => {
        this.requests = this.requests.filter((u) => {
          return !this.listRequest.includes(u.id);
        });
        this.requestService.progressNumber = this.requestService.progressNumber - this.listRequest.length;
        this.listRequest.length = 0;
        Swal.fire({
          title: 'La solicitud ha sido eliminada!',
          icon: 'success',
          showConfirmButton: false,
          timer: 1500
        });
        this.loadRequestsByRole();
      },
      (err) => console.log(err)
    );
  }


  // ------------------------------ POPUP METHODES ------------------------------
  options = [
    {name: 'Consultarnos dudas', value: 1},
    {name: 'Informarnos de cambios en tus datos', value: 2},
    {name: 'Solicitar alta o modificación de aportaciones periodicas', value: 3},
    {name: 'Solicitar la baja como socio', value: 4},
    {name: 'Solicitar mi historial de cuenta de años anteriores', value: 5},
  ];

  s
  getElement() {
    const input = document.getElementById('msg') as HTMLTextAreaElement | null;
    const e = document.getElementById('TypeRequest') as HTMLSelectElement;

    const sel = e.selectedIndex;
    const opt = e.options[sel];

    console.log(input.value);
    console.log(opt.value);
  }

  closeResult = '';

  open(content) {
    this.modalService
      .open(content, {ariaLabelledBy: 'modal-basic-title'})
      .result.then(
      (result) => {
        this.closeResult = `Closed with: ${result}`;
      },
      (reason) => {
        this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
      }
    );
  }

  private getDismissReason(reason: any): string {
    if (reason === ModalDismissReasons.ESC) {
      return 'by pressing ESC';
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      return 'by clicking on a backdrop';
    } else {
      return `with: ${reason}`;
    }
  }

  // ------------------------------ ------------ ------------------------------

}
