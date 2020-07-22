import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ReadCollectionAddRequest } from '../_models/readCollectionAddRequest';
import { map } from 'rxjs/operators';
import { PaginatedResult } from '../_models/filters/pagination';
import { ReadCollectionResponse } from '../_models/readCollectionResponse';
import { ReadCard } from '../_models/readCard';
import { IfStmt } from '@angular/compiler';
import { AlertifyService } from './alertify.service';
import { ReadResult } from '../_models/enums/readResult';
import { ReadOverview } from '../_models/readOverview';

@Injectable({
  providedIn: 'root'
})
export class ReadService {

  baseUrl = environment.apiUrl + 'readcollection/';
  readBaseUrl = environment.apiUrl + 'read/collection/';
  constructor(private http: HttpClient, private alertify: AlertifyService) { }

  addToReadCollection(collectionId: number, userId: number, isReversed: boolean, readPerDay: number) {
    const body: ReadCollectionAddRequest = { isReversed, readPerDay: +readPerDay };
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

  getReadCards(deck: number, readCollectionId: number, userId: number) {
    const url = this.readBaseUrl + readCollectionId + '/user/' + userId + '/deck/' + deck + '/cards'
    return this.http.get<ReadCard[]>(url, { observe: 'response' })
      .pipe(
        map(response => {
          return response.body;
        })
      );
  }

  moveCard(readCardId: number, userId: number, readResult: ReadResult) {
    if (readResult === ReadResult.NotRead) {
      this.alertify.error('Bad data. Error Code 01');
    }
    const status = readResult === ReadResult.Failed ? 'failed' : 'succeed';
    const url = environment.apiUrl + 'read/card/' + readCardId + '/user/' + userId + '/status/' + status + '/move';
    return this.http.patch(url, {});
  }

  getTodayReadCollectionOverview(readCollectionId: number, userId: number, deck?: number) {
    const url = deck >= 0
      ? this.baseUrl + readCollectionId + '/user/' + userId + '/deck/' + deck + '/overview'
      : this.baseUrl + readCollectionId + '/user/' + userId + '/overview';
    return this.http.get<ReadOverview>(url, { observe: 'response' }).pipe(
      map(response => {
        return response.body;
      })
    );
  }
}
