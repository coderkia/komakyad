import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { DashboardComponent } from './Dashboard/Dashboard.component';
import { AuthGuard } from './_guards/auth.guard';
import { CollectionsComponent } from './Collections/Collections.component';
import { CollectionEditComponent } from './Collections/CollectionEdit/CollectionEdit.component';
import { CollectionResolver } from './_resolvers/collection.resolver';
import { CollectionCreateComponent } from './Collections/CollectionCreate/CollectionCreate.component';
import { CardsComponent } from './Cards/Cards.component';
import { CardCreateComponent } from './Cards/cardCreate/cardCreate.component';

export const appRoutes: Routes = [
    { path: '', component: HomeComponent },
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard] },
            { path: 'collections', component: CollectionsComponent, canActivate: [AuthGuard] },
            { path: 'collections/create', component: CollectionCreateComponent, canActivate: [AuthGuard] },
            { path: 'collections/:id/update', component: CollectionEditComponent, resolve: { collection: CollectionResolver } },
            { path: 'collections/:id/cards', component: CardsComponent, canActivate: [AuthGuard] },
            { path: 'collections/:id/card', component: CardCreateComponent, canActivate: [AuthGuard] },
        ]
    },
    { path: '**', redirectTo: '', pathMatch: 'full' },
];
