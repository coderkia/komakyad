import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { CollectionResponse } from '../_models/collectionResponse';
import { PaginatedResult } from '../_models/filters/pagination';
import { CollectionRequest } from '../_models/collectionRequest';

@Injectable({
  providedIn: 'root'
})
export class CollectionService {
  baseUrl = environment.apiUrl + 'collection';
  constructor(private http: HttpClient) { }

  getCollectionById(id: number): Observable<CollectionResponse> {
    return this.http.get<CollectionResponse>(this.baseUrl + '/' + id, { observe: 'response' })
      .pipe(
        map(response => {
          return response.body;
        })
      );
  }

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
      if (collectionParams.includePrivateCollections != null) {
        params = params.append('includePrivateCollections', collectionParams.includePrivateCollections);
      }
    }

    return this.http.get<CollectionResponse[]>(this.baseUrl, { observe: 'response', params })
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

  update(id: number, collectionRequest: CollectionRequest) {
    return this.http.put(this.baseUrl + '/' + id, collectionRequest, { observe: 'response' });
  }
  create(collectionRequest: CollectionRequest) {
    return this.http.post(this.baseUrl, collectionRequest, { observe: 'response' });
  }
}
