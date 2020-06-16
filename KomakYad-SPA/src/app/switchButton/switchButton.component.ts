import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-switchButton',
  templateUrl: './switchButton.component.html',
  styleUrls: ['./switchButton.component.css']
})
export class SwitchButtonComponent implements OnInit {
 @Input() value: boolean;
 @Input() name: string;
  constructor() { }

  ngOnInit() {
    console.log(name);
  }

}
