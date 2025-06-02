import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AuditScreenComponent } from './audit-screen.component';

describe('AuditScreenComponent', () => {
  let component: AuditScreenComponent;
  let fixture: ComponentFixture<AuditScreenComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AuditScreenComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AuditScreenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
