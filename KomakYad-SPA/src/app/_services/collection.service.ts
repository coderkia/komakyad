import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { CollectionResponse } from '../_models/collectionResponse';
import { PaginatedResult } from '../_models/filters/pagination';

const httpOptions = {
  headers: new HttpHeaders({
    Authorization: 'Bearer ' + localStorage.getItem('token')
  })
};
@Injectable({
  providedIn: 'root'
})
export class CollectionService {
  baseUrl = environment.apiUrl + 'collection';
  constructor(private http: HttpClient) { }

  getCollections(page?, itemPerPage?, collectionParams?: any): Observable<PaginatedResult<CollectionResponse[]>> {
    const paginatedResult: PaginatedResult<CollectionResponse[]> = new PaginatedResult<CollectionResponse[]>();
    let params = new HttpParams();

    if (page != null && itemPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemPerPage);
    }

    if (collectionParams != null) {
      if (collectionParams.authorId != null) {
        params = params.append('authorId', collectionParams.authorId);
      }
      if (collectionParams.title != null) {
        params = params.append('title', collectionParams.title);
      }
      if (collectionParams.title != null) {
        params = params.append('includePrivateCollections', collectionParams.includePrivateCollections);
      }
    }

    return this.http.get<CollectionResponse[]>(this.baseUrl, { observe: 'response', params, headers: httpOptions.headers })
      .pipe(
        map(response => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
          }
          return paginatedResult;
        })
      );
  }
}
