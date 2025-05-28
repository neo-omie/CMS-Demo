import { CommonModule } from '@angular/common';
import { Component, Input, OnChanges } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { Observable, interval } from 'rxjs';

@Component({
  selector: 'hello',
  standalone: true,
  imports: [CommonModule, MatIconModule],
  template: `
  {{animation}}
  <div class="stepper">
    <div class="stepper-steps">
      <span *ngFor="let stepp of steps; let idx = index"><mat-icon>{{idx > step ? stepp : idx == step ? 'hourglass_empty' : finish}}</mat-icon></span>
    </div>
    <div class="stepper-bar">
      <span [ngStyle]="{width: getPercent() + '%'}" class="stepper-bar-content"></span>
    </div>
  </div>
  
  `,
  styleUrls: ['./progress-bar.component.css'],
})
export class HelloComponent implements OnChanges {
  @Input('step') step: number = 0;

  @Input('randomIncrease') randomIncrease?: boolean = true;

  animation = 0;
  getPercent() {
    let calc = (this.step * 100) / (this.steps.length - 1);
    return calc + this.animation;
  }

  finish = 'done_all';

  steps = ['check', 'check', 'check', 'check'];

  ngOnChanges() {
    console.log(this.step, 'changes');
    this.animation = 0;
    this.doAnimation(10);
  }

  doAnimation(timeoutInterval: number) {
    if (this.animation >= 100 / (this.steps ? this.steps.length : 100)) return;
    let timeout = setTimeout(() => {
      this.doAnimation(timeoutInterval);
    }, timeoutInterval);
    this.animation += this.randomIncrease ? Math.random() : 0.5;
    timeoutInterval += 5;
  }
}
