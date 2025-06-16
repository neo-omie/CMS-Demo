import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TerminationWithdrawNoticeComponent } from './termination-withdraw-notice.component';

describe('TerminationWithdrawNoticeComponent', () => {
  let component: TerminationWithdrawNoticeComponent;
  let fixture: ComponentFixture<TerminationWithdrawNoticeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TerminationWithdrawNoticeComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TerminationWithdrawNoticeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
