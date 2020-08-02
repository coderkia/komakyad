import { Component, OnInit } from '@angular/core';
import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service';
import { ReadService } from '../_services/read.service';
import { ReadCollectionResponse } from '../_models/readCollectionResponse';

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

  constructor(private alertify: AlertifyService, private authService: AuthService, private readService: ReadService) { }


  ngOnInit() {
    this.getFollowedCollections(1);
  }

  getFollowedCollections(currentPage: number) {
    this.loading = true;
    this.readService.getAllFollowedCollections(this.authService.currentUser.id, currentPage, 10)
      .subscribe(response => {
        this.followedCollections = response.result;
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
}
