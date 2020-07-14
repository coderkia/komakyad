import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ReadCollectionAddRequest } from '../_models/readCollectionAddRequest';
import { map } from 'rxjs/operators';
import { PaginatedResult } from '../_models/filters/pagination';
import { ReadCollectionResponse } from '../_models/readCollectionResponse';

@Injectable({
  providedIn: 'root'
})
export class ReadService {

  baseUrl = environment.apiUrl + 'readcollection/';
  constructor(private http: HttpClient) { }

  addToReadCollection(collectionId: number, userId: number, isReversed: boolean, readPerDay: number) {
    const body: ReadCollectionAddRequest = { isReversed, readPerDay: +readPerDay };
    console.log(readPerDay);
    return this.http.post(this.baseUrl + 'collection(' + collectionId + ')/user(' + userId + ')', body);
  }

  getAllFollowedCollections(userId: number, currentPage: number, itemPerPage: number) {
    const paginatedResult: PaginatedResult<ReadCollectionResponse[]> = new PaginatedResult<ReadCollectionResponse[]>();
    let params = new HttpParams();

    if (currentPage != null) {
      params = params.append('pageNumber', currentPage.toString());
    }
    if (itemPerPage != null) {
      params = params.append('pageSize', itemPerPage.toString());
    }

    return this.http.get<ReadCollectionResponse[]>(this.baseUrl + 'All', { observe: 'response', params })
      .pipe(
        map(response => {
          paginatedResult.result = response.body;
          if (response.headers.get('pagination') != null) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
          }
          return paginatedResult;
        })
      );
  }
}
