import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PostTerminationNoticeComponent } from './post-termination-notice.component';

describe('PostTerminationNoticeComponent', () => {
  let component: PostTerminationNoticeComponent;
  let fixture: ComponentFixture<PostTerminationNoticeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PostTerminationNoticeComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PostTerminationNoticeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
