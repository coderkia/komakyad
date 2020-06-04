import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { CollectionFilter } from 'src/app/_models/filters/collectionFilters';
import { FormBuilder, FormGroup } from '@angular/forms';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { CollectionResponse } from 'src/app/_models/collectionResponse';
import { CollectionService } from 'src/app/_services/collection.service';

@Component({
  selector: 'app-collection-search',
  templateUrl: './CollectionSearch.component.html',
  styleUrls: ['./CollectionSearch.component.css']
})
export class CollectionSearchComponent implements OnInit {
  @Output() collectionList = new EventEmitter();
  baseUrl = environment.apiUrl + 'collection/search';
  searchForm: FormGroup;
  constructor(private formbuilder: FormBuilder, private alertify: AlertifyService, private collectionService: CollectionService) { }

  ngOnInit() {
    this.createSearchForm();
  }

  createSearchForm() {
    this.searchForm = this.formbuilder.group({
      maxPageSize: [],
      orderBy: [],
      orderByDesc: [],
      pageSize: [],
      pageNumber: [],
      authorId: [],
      title: [],
      includePrivateCollections: [],
    });
  }

  search() {
    this.collectionService.getCollections(1, 10, this.searchForm.value).subscribe(response => {
      this.collectionList.emit(response);
    }, error => {
      this.alertify.error(error);
    });
  }

}
