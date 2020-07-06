import { Component, OnInit } from '@angular/core';
import { AlertifyService } from '../_services/alertify.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup } from '@angular/forms';
import { CardService } from '../_services/card.service';
import { AuthService } from '../_services/auth.service';
import { CollectionResponse } from '../_models/collectionResponse';
import { Pagination, PaginatedResult } from '../_models/filters/pagination';
import { environment } from 'src/environments/environment';
import { CardResponse } from '../_models/cardResponse';

@Component({
  selector: 'app-cards',
  templateUrl: './Cards.component.html',
  styleUrls: ['./Cards.component.css']
})
export class CardsComponent implements OnInit {

  collection: CollectionResponse;
  cards: CardResponse[];
  isLoading: boolean;
  pagination: Pagination = new Pagination();
  baseUrl = environment.apiUrl + 'collection/search';
  searchForm: FormGroup;

  constructor(private alertify: AlertifyService, private route: ActivatedRoute, private formbuilder: FormBuilder,
    private router: Router, private cardService: CardService, private authService: AuthService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.collection = data.collection;
    });
    this.createSearchForm();
    this.search();
  }

  createSearchForm() {
    this.searchForm = this.formbuilder.group({
      answer: [''],
      question: [''],
      orderBy: ['newest']
    });
  }

  search() {
    this.cardService.search(this.collection.id, this.pagination.currentPage, this.searchForm.value.answer, this.searchForm.value.question)
      .subscribe(response => {
        this.cards = response.result;
        this.pagination = response.pagination;
      });
  }
}
