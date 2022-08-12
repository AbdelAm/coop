import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {ForgotPasswordComponent} from './forgot-password/forgot-password.component';
import {LoginComponent} from './login/login.component';
import {SignupComponent} from './signup/signup.component';
import {PageNotFoundComponent} from './page-not-found/page-not-found.component';
import {EmailConfirmationComponent} from './email-confirmation/email-confirmation.component';
import {ResetPasswordComponent} from './reset-password/reset-password.component';

const routes: Routes = [
  {path: '', redirectTo: 'login', pathMatch: 'full'},
  {path: 'login', component: LoginComponent},
  {path: 'signup', component: SignupComponent},
  {path: 'email-confirmation', component: EmailConfirmationComponent},
  {path: 'forgot-password', component: ForgotPasswordComponent},
  {path: 'reset-password', component: ResetPasswordComponent},
  {path: 'administrateur/register', component: SignupComponent},
  {
    path: 'dashboard',
    loadChildren: () =>
      import('./dashboard/dashboard.module').then((m) => m.DashboardModule),
  },
  {path: '**', component: PageNotFoundComponent},
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {
}
