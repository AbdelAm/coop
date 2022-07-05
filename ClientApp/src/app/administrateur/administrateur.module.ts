import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdministrateurRoutingModule } from './administrateur-routing.module';
import { AdministrateurComponent } from './administrateur.component';
import { UserListComponent } from './user-list/user-list.component';
import { TransactionListComponent } from './transaction-list/transaction-list.component';
import { RequestListComponent } from './request-list/request-list.component';


@NgModule({
  declarations: [
    AdministrateurComponent,
    UserListComponent,
    TransactionListComponent,
    RequestListComponent
  ],
  imports: [
    CommonModule,
    AdministrateurRoutingModule
  ],
  bootstrap: [AdministrateurComponent]
})
export class AdministrateurModule { }
