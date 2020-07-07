/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { CardEditComponent } from './cardEdit.component';

describe('CardEditComponent', () => {
  let component: CardEditComponent;
  let fixture: ComponentFixture<CardEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CardEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CardEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
