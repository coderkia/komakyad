import { Injectable } from '@angular/core';
import { CardRequest } from '../_models/cardRequest';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class CardService {

  baseUrl = environment.apiUrl + 'card';
  constructor(private http: HttpClient) { }

  create(card: CardRequest) {
    return this.http.post(this.baseUrl, card);
  }


}
