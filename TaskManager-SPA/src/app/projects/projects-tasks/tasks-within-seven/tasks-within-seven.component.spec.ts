/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { TasksWithinSevenComponent } from './tasks-within-seven.component';

describe('TasksWithinSevenComponent', () => {
  let component: TasksWithinSevenComponent;
  let fixture: ComponentFixture<TasksWithinSevenComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TasksWithinSevenComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TasksWithinSevenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
