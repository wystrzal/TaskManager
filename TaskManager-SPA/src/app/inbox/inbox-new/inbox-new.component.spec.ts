/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { InboxNewComponent } from './inbox-new.component';

describe('InboxNewComponent', () => {
  let component: InboxNewComponent;
  let fixture: ComponentFixture<InboxNewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InboxNewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InboxNewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
