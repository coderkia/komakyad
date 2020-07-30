import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { TextStyle } from '../_models/textStyle';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-text-style',
  templateUrl: './textStyle.component.html',
  styleUrls: ['./textStyle.component.css']
})
export class TextStyleComponent implements OnInit {
  @Output() styleChanged = new EventEmitter();
  @Input() currentStyle: TextStyle;
  constructor(private alertify: AlertifyService) { }

  ngOnInit() {
    console.log('text style', this.currentStyle);

    if (this.currentStyle === undefined) {
      this.currentStyle = {
        align: 'left',
        direction: 'ltr',
      };
    }
  }

  setStyle(style: string) {
    switch (style) {
      case 'rtl':
      case 'ltr':
        if (this.currentStyle.direction === style) {
          return;
        }
        this.currentStyle.direction = style;
        break;
      case 'right':
      case 'center':
      case 'left':
      case 'justify':
        if (this.currentStyle.align === style) {
          return;
        }
        this.currentStyle.align = style;
        break;
      default:
        this.alertify.error('Invalid text style');
        console.log('Invalid Style ' + style);
        break;
    }
    this.styleChanged.emit(this.currentStyle);
  }
}
