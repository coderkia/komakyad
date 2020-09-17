import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { CardService } from 'src/app/_services/card.service';

@Component({
  selector: 'app-move-cards',
  templateUrl: './move-cards.component.html',
  styleUrls: ['./move-cards.component.css']
})
export class MoveCardsComponent implements OnInit {
  @Output() done = new EventEmitter();
  @Input() readCollectionId: number;
  destinationDeck = 0;
  targetDeck = -1;
  constructor(private cardService: CardService, private alertify: AlertifyService) { }

  ngOnInit() {
  }

  cancel() {
    this.done.emit();
  }

  move() {
    this.cardService.moveCards(this.readCollectionId, this.targetDeck, this.destinationDeck).subscribe(response => {
      const msgProperty = 'message';
      this.alertify.success(response[msgProperty]);
    }, error => {
      this.alertify.error(error);
    });
    this.done.emit();
  }
}
