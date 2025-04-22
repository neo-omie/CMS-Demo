import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MasterDocumentComponent } from './master-document.component';

describe('MasterDocumentComponent', () => {
  let component: MasterDocumentComponent;
  let fixture: ComponentFixture<MasterDocumentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MasterDocumentComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MasterDocumentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
