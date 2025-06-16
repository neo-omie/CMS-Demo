import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PostTerminationNoticeModalComponent } from './post-termination-notice-modal.component';

describe('PostTerminationNoticeModalComponent', () => {
  let component: PostTerminationNoticeModalComponent;
  let fixture: ComponentFixture<PostTerminationNoticeModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PostTerminationNoticeModalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PostTerminationNoticeModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
