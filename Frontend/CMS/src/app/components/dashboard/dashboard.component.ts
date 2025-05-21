declare var google: any;
import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { LoaderComponent } from '../UtilComponents/loader/loader.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [LoaderComponent, CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {
  loading: boolean = false;
  constructor(private title: Title) {
    this.title.setTitle("Dashboard - CMS");
  }

  ngOnInit(): void {
    this.loadGoogleChart();
  }

  loadGoogleChart() {
    google.charts.load('current', { packages: ['corechart'] });
    google.charts.setOnLoadCallback(this.drawChart);
  }

  drawChart() {
    const data = google.visualization.arrayToDataTable([
      ['Status', 'Hours per Day'],
      ['Pending Approval', 11],
      ['Pending Termination', 2],
      ['Expired', 2],
      ['Active', 2],
      ['Terminated', 7]
    ]);

    const options = {
      title: 'Company Performance',
      is3D: true
    };

    const chart = new google.visualization.PieChart(document.getElementById('piechart_3d'));
    chart.draw(data, options);
  }
}
