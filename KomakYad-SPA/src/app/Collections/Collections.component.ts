import { Component, OnInit, Output } from '@angular/core';
import { AlertifyService } from '../_services/alertify.service';
import { CollectionResponse } from '../_models/collectionResponse';
import { Pagination } from '../_models/filters/pagination';

@Component({
  selector: 'app-collections',
  templateUrl: './Collections.component.html',
  styleUrls: ['./Collections.component.css']
})
export class CollectionsComponent implements OnInit {
  isLoading: boolean;
  collectionList: Array<CollectionResponse>;
  constructor(private alertify: AlertifyService) { }

  ngOnInit() {
  }

  getLoadingStatus(isLoading: boolean) {
    this.isLoading = isLoading;
  }

  getSearchResult(result: any) {
    this.collectionList = result.result;
  }
}
