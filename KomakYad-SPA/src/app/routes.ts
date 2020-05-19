import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { DashboardComponent } from './Dashboard/Dashboard.component';
import { CardManagementComponent } from './CardManagement/CardManagement.component';
import { CollectionManagementComponent } from './CollectionManagement/CollectionManagement.component';

export const appRoutes: Routes = [
    { path: 'home', component: HomeComponent },
    { path: 'dashboard', component: DashboardComponent },
    { path: 'card', component: CardManagementComponent },
    { path: 'collection', component: CollectionManagementComponent },
    { path: '**', redirectTo: 'home', pathMatch: 'full' },
];
