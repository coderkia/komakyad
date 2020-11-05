import { Component, OnInit } from '@angular/core';
import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service';
import { ReadService } from '../_services/read.service';
import { ReadCollectionResponse } from '../_models/readCollectionResponse';
import { Pagination } from '../_models/filters/pagination';

@Component({
  selector: 'app-dashboard',
  templateUrl: './Dashboard.component.html',
  styleUrls: ['./Dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  loading = true;
  failedToLoadData = false;
  followedCollections: Array<ReadCollectionResponse>;
  selectedCollection: ReadCollectionResponse = null;
  showDeletedItems = false;
  pagination: Pagination = new Pagination();

  constructor(private alertify: AlertifyService, private authService: AuthService, private readService: ReadService) { }


  ngOnInit() {
    this.getFollowedCollections(1);
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.getFollowedCollections(this.pagination.currentPage);
  }

  getFollowedCollections(currentPage: number) {
    this.loading = true;
    this.readService.getAllFollowedCollections(this.authService.currentUser.id, currentPage, this.pagination.itemsPerPage, this.showDeletedItems)
      .subscribe(response => {
        this.followedCollections = response.result;
        this.pagination = response.pagination;
        this.getOverview();
        this.loading = false;
      }, error => {
        this.alertify.error(error);
        this.loading = false;
        this.failedToLoadData = true;
      });
  }

  getOverview() {
    this.followedCollections.forEach(item => {
      this.readService.getTodayReadCollectionOverview(item.id, this.authService.currentUser.id)
        .subscribe(response => {
          item.overview = response;
        }, error => {
          this.alertify.error(error);
        });
    });
  }

  showDetails(selectedCollection: ReadCollectionResponse) {
    this.selectedCollection = selectedCollection;
  }
  hideDetails() {
    this.selectedCollection = null;
  }

  remove(item: ReadCollectionResponse) {
    item.deleted = true;
    this.readService.remove(item.id).subscribe(response => {
      this.alertify.success('Item is deleted');
    }, error => {
      this.alertify.error(error);
    });
  }

  restoreDeletedItem(item: ReadCollectionResponse) {
    item.deleted = false;
    this.readService.restore(item.id).subscribe(response => {
      this.alertify.success('Item is restored');
    }, error => {
      this.alertify.error(error);
    });;
  }

  toggleDeletedItems() {
    this.showDeletedItems = !this.showDeletedItems;
    this.getFollowedCollections(this.pagination.currentPage);
  }
}
