import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';



const routes: Routes = [
  {path: '', component: HomeComponent, pathMatch: 'full'},
  { path: 'admin', loadChildren: () => import('./administrateur/administrateur.module').then(m => m.AdministrateurModule) }
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
