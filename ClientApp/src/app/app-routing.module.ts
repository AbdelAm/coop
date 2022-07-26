import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { LoginComponent } from './login/login.component';
import { SignupComponent } from './signup/signup.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { ProfileComponent } from './profile/profile.component';
import { GlobalComponent } from './global/global.component';
import { EmailConfirmationComponent } from './email-confirmation/email-confirmation.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';
import { UserListComponent } from './administrateur/user-list/user-list.component';
import { TransactionListComponent } from './administrateur/transaction-list/transaction-list.component';
import { RequestListComponent } from './administrateur/request-list/request-list.component';




const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'signup', component: SignupComponent },
  { path: 'email-confirmation', component: EmailConfirmationComponent},
  { path: 'forgot-password', component: ForgotPasswordComponent },
  { path: 'reset-password', component: ResetPasswordComponent },
  { path: 'users', component: UserListComponent},
  { path: 'transactions', component: TransactionListComponent},
  { path: 'requests', component: RequestListComponent},
  { path: 'global', component: GlobalComponent },
  { path: 'profile', component: ProfileComponent },
  { path: 'admin', loadChildren: () => import('./administrateur/administrateur.module').then(m => m.AdministrateurModule) },
  { path: '**', component: PageNotFoundComponent },
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
