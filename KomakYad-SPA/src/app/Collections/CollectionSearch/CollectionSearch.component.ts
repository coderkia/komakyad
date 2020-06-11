import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { environment } from 'src/environments/environment';
import { CollectionService } from 'src/app/_services/collection.service';
import { Pagination } from 'src/app/_models/filters/pagination';

@Component({
  selector: 'app-collection-search',
  templateUrl: './CollectionSearch.component.html',
  styleUrls: ['./CollectionSearch.component.css']
})
export class CollectionSearchComponent implements OnInit {
  @Output() collectionList = new EventEmitter();
  @Output() loading = new EventEmitter();
  isLoading: boolean;
  pagination: Pagination = new Pagination();
  baseUrl = environment.apiUrl + 'collection/search';
  searchForm: FormGroup;

  constructor(private formbuilder: FormBuilder, private alertify: AlertifyService, private collectionService: CollectionService) { }

  ngOnInit() {
    this.resetFilters();
    this.createSearchForm();
    this.search();
  }

  resetFilters() {
    this.pagination.currentPage = 1;
    this.pagination.itemsPerPage = 2;
  }

  createSearchForm() {
    this.searchForm = this.formbuilder.group({
      authorId: [],
      title: [],
      includePrivateCollections: [],
    });
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.search(this.pagination.currentPage);
  }

  search(currentPage?: number) {
    this.isLoading = true;
    this.loading.emit(this.isLoading);
    this.collectionService.getCollections(currentPage ?? 1, this.pagination.itemsPerPage,
      this.searchForm.value).subscribe(response => {
        this.pagination = response.pagination;
        this.collectionList.emit(response);
        this.isLoading = false;
        this.loading.emit(false);
      }, error => {
        this.alertify.error(error);
        this.isLoading = false;
        this.loading.emit(false);
      });
  }

}
