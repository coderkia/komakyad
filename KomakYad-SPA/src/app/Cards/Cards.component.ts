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
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';

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
  filterInput: string;

  constructor(private alertify: AlertifyService, private route: ActivatedRoute, private formbuilder: FormBuilder,
    private router: Router, private cardService: CardService, private authService: AuthService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.collection = data.collection;
    });
    this.resetFilters();
    this.createSearchForm();
    this.search();
  }

  resetFilters() {
    this.pagination.currentPage = 1;
    this.pagination.itemsPerPage = 9;
  }

  createSearchForm() {
    this.searchForm = this.formbuilder.group({
      searchInput: [''],
      orderBy: ['newest']
    });
  }

  search(currentPage?: number) {
    this.isLoading = true;
    let answer: string;
    let question: string;
    if (this.filterInput === 'answer') {
      answer = this.searchForm.value.searchInput;
    } else {
      question = this.searchForm.value.searchInput;
    }
    this.cardService.search(this.collection.id, currentPage, this.pagination.itemsPerPage, answer, question)
      .subscribe(response => {
        this.cards = response.result;
        this.pagination = response.pagination;
        this.isLoading = false;
      }, error => {
        this.alertify.error(error);
        this.isLoading = false;
      });
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.search(this.pagination.currentPage);
  }
}
