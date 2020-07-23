import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { LoginComponent } from './Auth/login/login.component';
import { HttpClientModule } from '@angular/common/http';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { JwtModule } from '@auth0/angular-jwt';
import { ProgressbarModule } from 'ngx-bootstrap/progressbar';

import { AppComponent } from './app.component';
import { DashboardComponent } from './Dashboard/Dashboard.component';
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
import { CardCreateComponent } from './Cards/cardCreate/cardCreate.component';
import { CollectionDetailsComponent } from './Cards/collectionDetails/collectionDetails.component';
import { CardItemComponent } from './Cards/cardItem/cardItem.component';
import { CardEditComponent } from './Cards/cardEdit/cardEdit.component';
import { CardDeleteComponent } from './Cards/cardDelete/cardDelete.component';
import { ReadCollectionDetailsComponent } from './Dashboard/readCollectionDetails/readCollectionDetails.component';
import { ReadCollectionItemComponent } from './Dashboard/readCollectionItem/readCollectionItem.component';
import { ReadComponent } from './Dashboard/read/read.component';
import { ReadCardComponent } from './Dashboard/readCard/readCard.component';
import { TextStyleComponent } from './textStyle/textStyle.component';

export function tokenGetter() {
   return localStorage.getItem('token');
}

@NgModule({
   declarations: [
      AppComponent,
      LoginComponent,
      DashboardComponent,
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
      CardsComponent,
      CardCreateComponent,
      CollectionDetailsComponent,
      CardItemComponent,
      CardEditComponent,
      CardDeleteComponent,
      ReadCollectionDetailsComponent,
      ReadCollectionItemComponent,
      ReadComponent,
      ReadCardComponent,
      TextStyleComponent
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
      ProgressbarModule.forRoot(),
      ButtonsModule.forRoot(),
      BrowserAnimationsModule,
      RouterModule.forRoot(appRoutes),
      JwtModule.forRoot({
         config: {
            tokenGetter,
            whitelistedDomains: ['localhost:5000'],
            blacklistedRoutes: ['localhost:5000/api/auth']
         }
      }),
      TabsModule.forRoot(),
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
