import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { GlobalComponent } from '../global/global.component';
import { AdministrateurComponent } from './administrateur.component';
import { RequestListComponent } from './request-list/request-list.component';
import { TransactionListComponent } from './transaction-list/transaction-list.component';
import { UserListComponent } from './user-list/user-list.component';

const routes: Routes = [
  {path: 'admin', component: AdministrateurComponent, 
  children: [
    {path: 'users', component: UserListComponent},
    {path: 'transactions', component: TransactionListComponent},
    {path: 'requests', component: RequestListComponent},
    {path: 'global', component: GlobalComponent},

  ]}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdministrateurRoutingModule { }
