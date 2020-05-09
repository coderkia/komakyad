/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { CardExploringComponent } from './CardExploring.component';

describe('CardExploringComponent', () => {
  let component: CardExploringComponent;
  let fixture: ComponentFixture<CardExploringComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CardExploringComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CardExploringComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
