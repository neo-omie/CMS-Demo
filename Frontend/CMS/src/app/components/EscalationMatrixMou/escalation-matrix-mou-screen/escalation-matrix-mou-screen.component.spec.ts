import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EscalationMatrixMouScreenComponent } from './escalation-matrix-mou-screen.component';

describe('EscalationMatrixMouScreenComponent', () => {
  let component: EscalationMatrixMouScreenComponent;
  let fixture: ComponentFixture<EscalationMatrixMouScreenComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EscalationMatrixMouScreenComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EscalationMatrixMouScreenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
