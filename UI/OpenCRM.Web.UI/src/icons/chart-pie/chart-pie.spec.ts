import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChartPie } from './chart-pie';

describe('ChartPie', () => {
  let component: ChartPie;
  let fixture: ComponentFixture<ChartPie>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ChartPie]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ChartPie);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
