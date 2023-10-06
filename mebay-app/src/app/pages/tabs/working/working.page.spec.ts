import { ComponentFixture, TestBed } from '@angular/core/testing';
import { WorkingPage } from './working.page';

describe('WorkingPage', () => {
  let component: WorkingPage;
  let fixture: ComponentFixture<WorkingPage>;

  beforeEach(async () => {
    fixture = TestBed.createComponent(WorkingPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
