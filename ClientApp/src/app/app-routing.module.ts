import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
<<<<<<< Updated upstream
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
=======
>>>>>>> Stashed changes



const routes: Routes = [
<<<<<<< Updated upstream
  {path: '', component: HomeComponent, pathMatch: 'full'},
  { path: 'admin', loadChildren: () => import('./administrateur/administrateur.module').then(m => m.AdministrateurModule) },
  {path: 'forgotpassword', component: ForgotPasswordComponent},

=======
>>>>>>> Stashed changes

  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full',
  },
  { path: 'login', component: LoginComponent },
  
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
