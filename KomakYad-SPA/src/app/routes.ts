import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { DashboardComponent } from './Dashboard/Dashboard.component';
import { CardManagementComponent } from './CardManagement/CardManagement.component';
import { CollectionManagementComponent } from './CollectionManagement/CollectionManagement.component';
import { AuthGuard } from './_guards/auth.guard';
import { CollectionsComponent } from './Collections/Collections.component';

export const appRoutes: Routes = [
    { path: '', component: HomeComponent },
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard] },
            { path: 'cards', component: CardManagementComponent, canActivate: [AuthGuard] },
            { path: 'collections', component: CollectionsComponent},
        ]
    },
    { path: '**', redirectTo: '', pathMatch: 'full' },
];
