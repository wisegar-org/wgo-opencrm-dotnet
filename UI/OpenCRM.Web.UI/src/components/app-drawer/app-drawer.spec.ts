import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AppDrawer } from './app-drawer';

describe('AppDrawer', () => {
  let component: AppDrawer;
  let fixture: ComponentFixture<AppDrawer>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AppDrawer]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AppDrawer);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
