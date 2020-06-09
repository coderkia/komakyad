import { Component, OnInit } from '@angular/core';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-collectionmanagement',
  templateUrl: './CollectionManagement.component.html',
  styleUrls: ['./CollectionManagement.component.css']
})
export class CollectionManagementComponent implements OnInit {

  constructor(private alertify: AlertifyService) { }

  ngOnInit() {
  }

  getCollections() {

  }
}
