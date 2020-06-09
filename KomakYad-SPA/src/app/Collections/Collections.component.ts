import { Component, OnInit, Output } from '@angular/core';
import { AlertifyService } from '../_services/alertify.service';
import { CollectionResponse } from '../_models/collectionResponse';
import { Pagination } from '../_models/filters/pagination';
import { CollectionService } from '../_services/collection.service';

@Component({
  selector: 'app-collections',
  templateUrl: './Collections.component.html',
  styleUrls: ['./Collections.component.css']
})
export class CollectionsComponent implements OnInit {

  collectionList: Array<CollectionResponse>;
  pagination: Pagination;
  constructor(private alertify: AlertifyService) { }

  ngOnInit() {
  }

  getSearchResult(result: any) {
    this.collectionList = result.result;
    this.pagination = result.pagination;
    console.log(this.pagination);
    console.log(this.collectionList);
  }
}
