import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { CollectionResponse } from '../_models/collectionResponse';
import { CollectionService } from '../_services/collection.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class CollectionResolver implements Resolve<CollectionResponse>{
    constructor(private collectionService: CollectionService, private router: Router, private alertify: AlertifyService) { }

    resolve(route: ActivatedRouteSnapshot): Observable<CollectionResponse> {
        return this.collectionService.getCollectionById(route.params.id).pipe(
            catchError(error => {
                this.alertify.error(error);
                this.router.navigate(['/collections']);
                return of(null);
            })
        );
    }
}