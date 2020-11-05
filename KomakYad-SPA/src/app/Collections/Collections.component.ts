import { Component, OnInit, Output } from '@angular/core';
import { AlertifyService } from '../_services/alertify.service';
import { CollectionResponse } from '../_models/collectionResponse';
import { ReadCollectionResponse } from '../_models/readCollectionResponse';
import { ReadService } from '../_services/read.service';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-collections',
  templateUrl: './Collections.component.html',
  styleUrls: ['./Collections.component.css']
})
export class CollectionsComponent implements OnInit {
  isLoading = true;
  failedToLoadData = false;
  collectionList: Array<CollectionResponse>;
  followedCollections: Array<number> = new Array<number>();
  constructor(private alertify: AlertifyService, private readService: ReadService, private authService: AuthService) { }

  ngOnInit() {
    this.getFollowedCollections(1);
  }

  getFollowedCollections(currentPage: number) {
    this.failedToLoadData = false;
    this.readService.getAllFollowedCollections(this.authService.currentUser.id, currentPage, 50, true)
      .subscribe(response => {
        if (response.pagination.totalPages > currentPage) {
          currentPage++;
          this.getFollowedCollections(currentPage);
        }
        response.result.forEach(item => {
          this.followedCollections.push(item.collection.id);
        });
      }, error => {
        this.alertify.error(error);
        this.failedToLoadData = true;
      });
  }
  getLoadingStatus(isLoading: boolean) {
    this.isLoading = isLoading;
  }

  getSearchResult(result: any) {
    this.collectionList = result.result;
  }

}
