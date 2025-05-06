import { Component } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-masters-screen',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './masters-screen.component.html',
  styleUrl: './masters-screen.component.css'
})
export class MastersScreenComponent {
  constructor(private title:Title) {
    this.title.setTitle("All Masters - CMS");
  }
}
