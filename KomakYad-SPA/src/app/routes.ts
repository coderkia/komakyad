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
import { ProfileComponent } from './profile/profile.component';
import { ManagementComponent } from './management/management.component';
import { LoginComponent } from './Auth/login/login.component';
import { ConfirmEmailComponent } from './Auth/confirm-email/confirm-email.component';
import { EmailConfirmationComponent } from './Auth/email-confirmation/email-confirmation.component';

export const appRoutes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'login', component: LoginComponent },
    { path: 'confirmEmail', component: ConfirmEmailComponent },
    { path: 'emailConfirmation', component: EmailConfirmationComponent },
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            { path: 'profile', component: ProfileComponent },
            { path: 'management', component: ManagementComponent },
            { path: 'dashboard', component: DashboardComponent },
            { path: 'collections', component: CollectionsComponent },
            { path: 'collections/create', component: CollectionCreateComponent },
            { path: 'collections/:id/update', component: CollectionEditComponent, resolve: { collection: CollectionResolver } },
            {
                path: 'collections/:id/cards', component: CardsComponent,
                resolve: { collection: CollectionResolver }
            },
            {
                path: 'collections/:id/card', component: CardCreateComponent,
                resolve: { collection: CollectionResolver }
            },
        ]
    },
    { path: '**', redirectTo: '', pathMatch: 'full' },
];
