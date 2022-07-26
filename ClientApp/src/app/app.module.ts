import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {HttpClientModule} from '@angular/common/http';
import {AppComponent} from './app.component';
import {NavMenuComponent} from './nav-menu/nav-menu.component';
import {HomeComponent} from './home/home.component';
import {AppRoutingModule} from './app-routing.module';
import {ForgotPasswordComponent} from './forgot-password/forgot-password.component';
import {LoginComponent} from './login/login.component';
import {AdministrateurModule} from './administrateur/administrateur.module';
import {SignupComponent} from './signup/signup.component';
import {GlobalComponent} from './global/global.component';

import {PageNotFoundComponent} from './page-not-found/page-not-found.component';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {TransactionPopupComponent} from './transaction-popup/transaction-popup.component';
import {MatDialogModule} from '@angular/material/dialog';
import {MatStepperModule} from '@angular/material/stepper';
import {ProfileComponent} from './profile/profile.component';
import {RequestPopupComponent} from './request-popup/request-popup.component';
import {EmailConfirmationComponent} from './email-confirmation/email-confirmation.component';
import {ResetPasswordComponent} from './reset-password/reset-password.component';
import {MatButtonModule} from '@angular/material/button';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatInputModule} from '@angular/material/input';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    LoginComponent,
    ForgotPasswordComponent,
    SignupComponent,
    GlobalComponent,
    PageNotFoundComponent,
    TransactionPopupComponent,
    ProfileComponent,
    RequestPopupComponent,
    EmailConfirmationComponent,
    ResetPasswordComponent,
  ],

  imports: [
    BrowserModule.withServerTransition({appId: 'ng-cli-universal'}),
    HttpClientModule,
    FormsModule,
    AdministrateurModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatDialogModule,
    MatStepperModule,
    MatButtonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule
  ],
  providers: [AdministrateurModule],
  bootstrap: [AppComponent]
})
export class AppModule {
}
