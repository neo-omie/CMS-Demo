import { CommonModule } from '@angular/common';
import { Component, Input, OnChanges } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { Observable, interval } from 'rxjs';
import { ProgressBarComponent } from './progress-bar.component';

@Component({
  selector: 'hello',
  standalone: true,
  imports: [CommonModule, MatIconModule],
  template: `
  <div class="stepper">
    <ng-container *ngFor="let phase of phases; let idx = index">
      <div class="step">
        <mat-icon>{{ getIcon(stepStatuses[idx]) }}</mat-icon>
        <div class="label-wrapper">
          <div *ngFor="let line of phase.split(' ')" class="step-label">{{ line }}</div>
        </div>
      </div>
      <div *ngIf="idx < phases.length - 1" class="line"></div>
    </ng-container>
  </div>
  `,
  styleUrls: ['./progress-bar.component.css'],
})
export class HelloComponent implements OnChanges {
  @Input() phases: string[] = [];
  @Input() stepStatuses:number[]=[];

  // getPercent():number {
  //   if(this.phases.length<=1){
  //     return 0
  //   }
  //   return (this.step / (this.phases.length - 1))*100;
  // }

  getIcon(status:number):string{
    switch(status){
      case 1: return 'hourglass_empty';         // PendingApproval
      case 2: return 'check_circle';            // Active
      case 3: return 'cancel';                  // Rejected
      case 4: return 'highlight_off';           // Terminated
      case 5: return 'event_busy';              // Expired
      case 6: return 'hourglass_top';           // PendingTermination
      case 7: return 'delete_outline';          // ApprovedForTermination
      case 8: return 'unpublished';             // PendingNoticeWithdrawn
      default: return 'radio_button_unchecked'; // Unknown/default
    }
  }

  ngOnChanges():void {
    console.log(`Current step: ${this.stepStatuses}`);
  }
}

