/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { TextStyleComponent } from './textStyle.component';

describe('TextStyleComponent', () => {
  let component: TextStyleComponent;
  let fixture: ComponentFixture<TextStyleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TextStyleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TextStyleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
