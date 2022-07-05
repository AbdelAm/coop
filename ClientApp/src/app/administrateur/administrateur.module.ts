import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdministrateurRoutingModule } from './administrateur-routing.module';
import { AdministrateurComponent } from './administrateur.component';
import { UserListComponent } from './user-list/user-list.component';


@NgModule({
  declarations: [
    AdministrateurComponent,
    UserListComponent
  ],
  imports: [
    CommonModule,
    AdministrateurRoutingModule
  ],
  bootstrap: [AdministrateurComponent]
})
export class AdministrateurModule { }
