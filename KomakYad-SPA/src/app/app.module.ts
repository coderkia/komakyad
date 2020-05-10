import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { LoginComponent } from './Auth/login/login.component';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { DashboardComponent } from './Dashboard/Dashboard.component';
import { CardManagementComponent } from './CardManagement/CardManagement.component';
import { ReadCardComponent } from './ReadCard/ReadCard.component';
import { CollectionManagementComponent } from './CollectionManagement/CollectionManagement.component';
import { CardExploringComponent } from './CardExploring/CardExploring.component';
import { ResetPassComponent } from './ResetPass/ResetPass.component';
import { NavComponent } from './nav/nav.component';
import {FormsModule} from '@angular/forms';
import { AuthService } from './_services/auth.service';
import { RegisterComponent } from './Auth/register/register.component';
import { HomeComponent } from './home/home.component';

@NgModule({
   declarations: [
      AppComponent,
      LoginComponent,
      DashboardComponent,
      CardManagementComponent,
      ReadCardComponent,
      CollectionManagementComponent,
      CardExploringComponent,
      ResetPassComponent,
      NavComponent,
      RegisterComponent,
      HomeComponent
   ],
   imports: [
      BrowserModule,
      HttpClientModule,
      FormsModule
   ],
   providers: [
      AuthService
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
