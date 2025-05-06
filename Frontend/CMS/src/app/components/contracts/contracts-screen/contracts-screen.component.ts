import { Component } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-contracts-screen',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './contracts-screen.component.html',
  styleUrl: './contracts-screen.component.css'
})
export class ContractsScreenComponent {
  constructor(private title:Title) {
    this.title.setTitle("Contracts - CMS");
  }
}
