import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {HttpClientModule, HTTP_INTERCEPTORS} from '@angular/common/http';
import {AppComponent} from './app.component';
import {AppRoutingModule} from './app-routing.module';
import {ForgotPasswordComponent} from './forgot-password/forgot-password.component';
import {LoginComponent} from './login/login.component';
import {DashboardModule} from './dashboard/dashboard.module';
import {SignupComponent} from './signup/signup.component';

import {PageNotFoundComponent} from './page-not-found/page-not-found.component';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {MatDialogModule} from '@angular/material/dialog';
import {MatStepperModule} from '@angular/material/stepper';
import {EmailConfirmationComponent} from './email-confirmation/email-confirmation.component';
import {ResetPasswordComponent} from './reset-password/reset-password.component';
import {MatButtonModule} from '@angular/material/button';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatInputModule} from '@angular/material/input';
import {TokenInterceptorService} from './shared/services/token-interceptor.service';
import { CommonModule } from '@angular/common';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    ForgotPasswordComponent,
    SignupComponent,
    PageNotFoundComponent,
    EmailConfirmationComponent,
    ResetPasswordComponent,
  ],

  imports: [
    BrowserModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    DashboardModule,
    AppRoutingModule,
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
