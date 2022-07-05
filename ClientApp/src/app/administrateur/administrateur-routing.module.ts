import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdministrateurComponent } from './administrateur.component';
import { TransactionListComponent } from './transaction-list/transaction-list.component';
import { UserListComponent } from './user-list/user-list.component';

const routes: Routes = [
  {path: '', component: AdministrateurComponent, children: [
    {path: '', redirectTo: 'users', pathMatch: 'full'},
    {path: 'users', component: UserListComponent},
    {path: 'transactions', component: TransactionListComponent} 
  ]}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdministrateurRoutingModule { }