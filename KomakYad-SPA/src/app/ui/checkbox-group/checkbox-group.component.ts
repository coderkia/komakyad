import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-checkbox-group',
  templateUrl: './checkbox-group.component.html',
  styleUrls: ['./checkbox-group.component.css']
})
export class CheckboxGroupComponent implements OnInit {
  randomNumber: number;
  @Input() items: { id: string, name: string, checked?: boolean }[];
  @Output() changed = new EventEmitter<{ id: string, name: string, checked?: boolean }>();
  constructor() { }

  ngOnInit() {
    this.generateRandomNumber();
    console.log(this.items);
  }

  checkboxToggled(item: { id: string, name: string, checked?: boolean }, checked: boolean) {
    item.checked = checked;
    this.changed.emit(item);
  }
  generateRandomNumber() {
    const num = Math.random() * 10000;
    this.randomNumber = Math.floor(num);
  }
}
