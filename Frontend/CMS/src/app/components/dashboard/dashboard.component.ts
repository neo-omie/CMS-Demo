import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { LoaderComponent } from '../UtilComponents/loader/loader.component';
import { CommonModule } from '@angular/common';
import { ContractsService } from '../../services/contracts.service';
import { ContractsCount } from '../../models/contracts-count';
import { Alert } from '../../utils/alert';
import { TYPE } from '../auth/login/values.constants';
import { ClassifiedContractsService } from '../../services/classified-contracts.service';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { HighchartsChartModule } from 'highcharts-angular';
import Highcharts from 'highcharts';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [LoaderComponent, CommonModule, NgxChartsModule,
    HighchartsChartModule
  ],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {
  loading: boolean = false;
  viewportWidth:number = window.innerWidth
  updateFlag1:boolean = false;
  updateFlag2:boolean = false;
  Highcharts: typeof Highcharts = Highcharts;

  contractchartOptions: Highcharts.Options = {
    credits: { enabled: false },
    chart: { type: 'pie' },
    title: { text: '' },
    tooltip: {
      pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
    },
    plotOptions: {
      pie: {
        allowPointSelect: true,
        cursor: 'pointer',
        dataLabels: { enabled: false },
        showInLegend: true
      }
    },
    series: [{
      name: 'Contracts',
      type: 'pie',
      data: []
    }]
  };

  classifiedcontractchartOptions: Highcharts.Options = {
    credits: { enabled: false },
    chart: { type: 'pie' },
    title: { text: '' },
    tooltip: {
      pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
    },
    plotOptions: {
      pie: {
        allowPointSelect: true,
        cursor: 'pointer',
        dataLabels: { enabled: false },
        showInLegend: true
      }
    },
    series: [{
      name: 'Classified contracts',
      type: 'pie',
      data: []
    }]
  };

  constructor(private title: Title,
    private contractsService: ContractsService,
    private classifiedContractsService: ClassifiedContractsService) {
    this.title.setTitle("Dashboard - CMS");
  }


  ngOnInit() {
    this.GetContractsCount();
    this.GetClassifiedContractsCount();
  }

  ngAfterViewInit() {
    setTimeout(() => this.onResize(), 0);
  }

  @HostListener('window:resize', ['$event'])
  onResize() {
    this.viewportWidth = window.innerWidth;
  }

  GetContractsCount() {
    this.contractsService.getContractCounts().subscribe({
      next: (response: ContractsCount) => {
        const formatted = []
        const keys = Object.keys(response);
        let sum = 0;
        const values = Object.values(response);
        for (let index = 0; index < keys.length; index++) {
          if (keys[index] != 'allContractsCount'){
            sum += values[index];
            formatted.push({
              name: keys[index],
              y: values[index],
            })
          }

        }
        formatted.push({
          name: "Others",
          y: response.allContractsCount - sum
        });
        // Set the series data dynamically
        (this.contractchartOptions.series as Highcharts.SeriesOptionsType[])[0] = {
          type: 'pie',
          name: 'Contracts',
          data: formatted
        };
        this.updateFlag1 = true;
        setTimeout(() => this.updateFlag1 = false, 100);
      }, error: (error) => {
        Alert.toast(TYPE.ERROR, true, error.error.message);
      }
    });
  }
  GetClassifiedContractsCount() {
    this.classifiedContractsService.getClassifiedContractCounts().subscribe({
      next: (response: ContractsCount) => {
        const formatted = []
        const keys = Object.keys(response);
        const values = Object.values(response);
        let sum = 0;
        for (let index = 0; index < keys.length; index++) {
          if (keys[index] != 'allContractsCount'){
            sum += values[index];
            formatted.push({
              name: keys[index],
              y: values[index],
            })
          }

        }
        formatted.push({
          name: "Others",
          y: response.allContractsCount - sum
        });
        // Set the series data dynamically
        (this.classifiedcontractchartOptions.series as Highcharts.SeriesOptionsType[])[0] = {
          type: 'pie',
          name: 'Classified contracts',
          data: formatted
        };
        this.updateFlag2 = true;
        setTimeout(() => this.updateFlag2 = false, 100);
      }, error: (error) => {
        Alert.toast(TYPE.ERROR, true, error.error.message);
      }
    });
  }
}
