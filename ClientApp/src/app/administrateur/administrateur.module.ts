import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdministrateurRoutingModule } from './administrateur-routing.module';
import { AdministrateurComponent } from './administrateur.component';
import { UserListComponent } from './user-list/user-list.component';
import { TransactionListComponent } from './transaction-list/transaction-list.component';
import { RequestListComponent } from './request-list/request-list.component';
import { HeaderComponent } from '../header/header.component';
import { SidebarComponent } from '../sidebar/sidebar.component';


@NgModule({
  declarations: [
    AdministrateurComponent,
    HeaderComponent,
    SidebarComponent,
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
