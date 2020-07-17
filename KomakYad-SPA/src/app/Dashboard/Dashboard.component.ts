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

  followedCollections: Array<ReadCollectionResponse>;
  selectedCollection: ReadCollectionResponse = null;

  constructor(private alertify: AlertifyService, private authService: AuthService, private readService: ReadService) { }


  ngOnInit() {
    this.getFollowedCollections(1);
  }

  getFollowedCollections(currentPage: number) {
    this.readService.getAllFollowedCollections(this.authService.currentUser.id, currentPage, 10)
      .subscribe(response => {
        this.followedCollections = response.result;
      }, error => {
        this.alertify.error(error);
      });
  }

  showDetails(selectedCollection: ReadCollectionResponse) {
    this.selectedCollection = selectedCollection;
  }
  hideDetails(){
    this.selectedCollection = null;
  }
}
