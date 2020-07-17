import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { ReadCollectionResponse } from 'src/app/_models/readCollectionResponse';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ReadCard } from 'src/app/_models/readCard';
import { ReadService } from 'src/app/_services/read.service';
import { AuthService } from 'src/app/_services/auth.service';
import { map } from 'rxjs/operators';
import { ReadComponent } from '../read/read.component';

@Component({
  selector: 'app-read-collection-details',
  templateUrl: './readCollectionDetails.component.html',
  styleUrls: ['./readCollectionDetails.component.css']
})
export class ReadCollectionDetailsComponent implements OnInit {
  @Input() readCollection: ReadCollectionResponse;
  @ViewChild(ReadComponent) readComponent;

  decks: any = [
    { deck: 0, class: 'deck-gray' },
    { deck: 1, class: 'deck-orange' },
    { deck: 2, class: 'deck-sky' },
    { deck: 3, class: 'deck-orangered' },
    { deck: 4, class: 'deck-yellow' },
    { deck: 6, class: 'deck-green' },
  ];
  currentDeck: number = 5;
  readCards: Array<ReadCard>;
  readModeActive: boolean;
  constructor(private alertify: AlertifyService, private readService: ReadService, private authService: AuthService) { }

  ngOnInit() {
  }

  startReading(deck: number) {
    this.readService.getReadCards(deck, this.readCollection.id, this.authService.currentUser.id)
      .subscribe(response => {
        this.currentDeck = deck;
        this.readModeActive = true;
        this.readCards = response;
      }, error => {
        this.alertify.error(error);
      });

  }

}
