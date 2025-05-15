import { Component } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { LoaderComponent } from '../loader/loader.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [LoaderComponent, CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent {
  loading:boolean = false;
  constructor(private title:Title) {
    this.title.setTitle("Dashboard - CMS");
  }
  public pie_ChartData = [
    ['Status', 'Hours per Day'],
    ['Pending Approval', 11],
    ['Pending Termination', 2],
    ['Expired', 2],
    ['Active', 2],
    ['Terminated', 7]
  ];
  public pie_ChartOptions = {
    title: 'My Daily Activities',
    width: 900,
    height: 500,
    pieHole: 0.1,
    is3D: true
  };
}
