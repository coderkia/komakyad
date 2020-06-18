import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { ReadCollectionAddRequest } from '../_models/readCollectionAddRequest';

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

}
