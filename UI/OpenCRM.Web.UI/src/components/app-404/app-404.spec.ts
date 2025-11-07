import { ComponentFixture, TestBed } from '@angular/core/testing';

import { App404 } from './app-404';

describe('App404', () => {
  let component: App404;
  let fixture: ComponentFixture<App404>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [App404]
    })
    .compileComponents();

    fixture = TestBed.createComponent(App404);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
