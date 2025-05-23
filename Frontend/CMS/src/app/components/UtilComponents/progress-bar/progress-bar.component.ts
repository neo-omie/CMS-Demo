import { Component, Input } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { BrowserModule } from '@angular/platform-browser';
import { MatIconModule } from '@angular/material/icon';
import { HelloComponent } from './hello.component';
import { AppComponent } from '../../../app.component';

@Component({
  selector: 'app-progress-bar',
  standalone: true,
  imports: [BrowserModule, FormsModule, MatButtonModule, MatIconModule, HelloComponent],
  templateUrl: './progress-bar.component.html',
  styleUrl: './progress-bar.component.css'
})
export class ProgressBarComponent {
  @Input() progress!: number;
  step = 0;

  ngOnInit() {}

  startStep() {
    this.step = this.step + (this.step < 4 ? 1 : 0);
  }
}
