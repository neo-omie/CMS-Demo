import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MasterApostilleComponent } from './master-apostille.component';

describe('MasterApostilleComponent', () => {
  let component: MasterApostilleComponent;
  let fixture: ComponentFixture<MasterApostilleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MasterApostilleComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MasterApostilleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
