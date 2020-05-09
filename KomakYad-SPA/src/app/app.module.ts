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

@NgModule({
   declarations: [
      AppComponent,
      LoginComponent,
      DashboardComponent,
      CardManagementComponent,
      ReadCardComponent,
      CollectionManagementComponent,
      CardExploringComponent,
      ResetPassComponent
   ],
   imports: [
      BrowserModule,
      HttpClientModule
   ],
   providers: [],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
