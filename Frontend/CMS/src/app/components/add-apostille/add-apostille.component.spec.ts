import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddApostilleComponent } from './add-apostille.component';

describe('AddApostilleComponent', () => {
  let component: AddApostilleComponent;
  let fixture: ComponentFixture<AddApostilleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddApostilleComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddApostilleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
