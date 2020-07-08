import { Component, OnInit, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-card-delete',
  templateUrl: './cardDelete.component.html',
  styleUrls: ['./cardDelete.component.css']
})
export class CardDeleteComponent implements OnInit {
  @Output() response = new EventEmitter();
  constructor() { }

  ngOnInit() {
  }

  getResult(userAnswer) {
    this.response.emit(userAnswer);
  }
}
