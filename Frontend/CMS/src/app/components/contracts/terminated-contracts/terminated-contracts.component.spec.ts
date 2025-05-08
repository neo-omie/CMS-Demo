import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TerminatedContractsComponent } from './terminated-contracts.component';

describe('TerminatedContractsComponent', () => {
  let component: TerminatedContractsComponent;
  let fixture: ComponentFixture<TerminatedContractsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TerminatedContractsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TerminatedContractsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
