import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { LoginComponent } from './Auth/login/login.component';
import { HttpClientModule } from '@angular/common/http';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { JwtModule } from '@auth0/angular-jwt';

import { AppComponent } from './app.component';
import { DashboardComponent } from './Dashboard/Dashboard.component';
import { CardManagementComponent } from './CardManagement/CardManagement.component';
import { ReadCardComponent } from './ReadCard/ReadCard.component';
import { CardExploringComponent } from './CardExploring/CardExploring.component';
import { ResetPassComponent } from './ResetPass/ResetPass.component';
import { NavComponent } from './nav/nav.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from './_services/auth.service';
import { RegisterComponent } from './Auth/register/register.component';
import { HomeComponent } from './home/home.component';
import { ErrorInterceptorProvider } from './_services/error.intercptor';
import { appRoutes } from './routes';
import { CollectionsComponent } from './Collections/Collections.component';
import { CollectionSearchComponent } from './Collections/CollectionSearch/CollectionSearch.component';
import { CollectionItemComponent } from './Collections/CollectionItem/CollectionItem.component';
import { CollectionEditComponent } from './Collections/CollectionEdit/CollectionEdit.component';
import { CollectionResolver } from './_resolvers/collection.resolver';
import { CollectionCreateComponent } from './Collections/CollectionCreate/CollectionCreate.component';
import { CollectionAddToReadsComponent } from './Collections/CollectionAddToReads/CollectionAddToReads.component';
import { SwitchButtonComponent } from './switchButton/switchButton.component';
import { CardsComponent } from './Cards/Cards.component';

export function tokenGetter() {
   return localStorage.getItem('token');
}

@NgModule({
   declarations: [
      AppComponent,
      LoginComponent,
      DashboardComponent,
      CardManagementComponent,
      ReadCardComponent,
      CardExploringComponent,
      ResetPassComponent,
      NavComponent,
      RegisterComponent,
      HomeComponent,
      CollectionsComponent,
      CollectionSearchComponent,
      CollectionItemComponent,
      CollectionEditComponent,
      CollectionCreateComponent,
      CollectionAddToReadsComponent,
      SwitchButtonComponent,
      CardsComponent
   ],
   imports: [
      BrowserModule,
      HttpClientModule,
      FormsModule,
      ReactiveFormsModule,
      PaginationModule.forRoot(),
      BrowserAnimationsModule,
      BsDropdownModule.forRoot(),
      BsDatepickerModule.forRoot(),
      BrowserAnimationsModule,
      RouterModule.forRoot(appRoutes),
      JwtModule.forRoot({
         config: {
            tokenGetter,
            whitelistedDomains: ['localhost:5000'],
            blacklistedRoutes: ['localhost:5000/api/auth']
         }
      })
   ],
   providers: [
      AuthService,
      ErrorInterceptorProvider,
      CollectionResolver
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
