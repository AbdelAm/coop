import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';



const routes: Routes = [
  {path: '', component: HomeComponent, pathMatch: 'full'},
  { path: 'admin', loadChildren: () => import('./administrateur/administrateur.module').then(m => m.AdministrateurModule) },
  {path: 'forgotpassword', component: ForgotPasswordComponent},


];

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    [RouterModule.forRoot(routes)],
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
