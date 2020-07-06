import { Injectable } from '@angular/core';
import { CardRequest } from '../_models/cardRequest';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Pagination, PaginatedResult } from '../_models/filters/pagination';
import { CardResponse } from '../_models/cardResponse';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class CardService {

  baseUrl = environment.apiUrl + 'card';
  constructor(private http: HttpClient) { }

  create(card: CardRequest) {
    return this.http.post(this.baseUrl, card);
  }

  search(collectionId: any, currentPage, itemPerPage, answer: string, question: string) {
    let params = new HttpParams();

    if (currentPage != null) {
      params = params.append('pageNumber', currentPage);
    }
    if (itemPerPage != null) {
      params = params.append('pageSize', itemPerPage);
    }

    const paginatedResult: PaginatedResult<CardResponse[]> = new PaginatedResult<CardResponse[]>();
    if (answer) {
      params = params.append('answer', answer);
    }
    if (question) {
      params = params.append('answer', question);
    }
    params = params.append('collectionId', collectionId);
    return this.http.get<CardResponse[]>(this.baseUrl, { observe: 'response', params })
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
