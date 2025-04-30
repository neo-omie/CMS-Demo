import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EscalationMatrixMouComponent } from './escalation-matrix-mou.component';

describe('EscalationMatrixMouComponent', () => {
  let component: EscalationMatrixMouComponent;
  let fixture: ComponentFixture<EscalationMatrixMouComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EscalationMatrixMouComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EscalationMatrixMouComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
