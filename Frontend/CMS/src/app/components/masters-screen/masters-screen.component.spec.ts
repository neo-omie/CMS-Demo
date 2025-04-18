import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MastersScreenComponent } from './masters-screen.component';

describe('MastersScreenComponent', () => {
  let component: MastersScreenComponent;
  let fixture: ComponentFixture<MastersScreenComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MastersScreenComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MastersScreenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
