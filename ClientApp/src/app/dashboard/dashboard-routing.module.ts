import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { GlobalComponent } from './global/global.component';
import { ProfileComponent } from './profile/profile.component';
import { DashboardComponent } from './dashboard.component';
import { RequestListComponent } from './request-list/request-list.component';
import { TransactionListComponent } from './transaction-list/transaction-list.component';
import { UserListComponent } from './user-list/user-list.component';

const routes: Routes = [
  {path: '', component: DashboardComponent, children: [
    {path: 'global', component: GlobalComponent},
    {path: 'users', component: UserListComponent},
    {path: 'transaction', component: TransactionListComponent},
    {path: 'requests', component: RequestListComponent},
    {path: 'profile', component: ProfileComponent}
  ]}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DashboardRoutingModule { }
