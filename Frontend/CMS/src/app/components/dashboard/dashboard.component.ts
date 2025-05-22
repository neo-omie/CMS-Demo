import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { LoaderComponent } from '../UtilComponents/loader/loader.component';
import { CommonModule } from '@angular/common';
import { ContractsService } from '../../services/contracts.service';
import { ContractsCount } from '../../models/contracts-count';
import { Alert } from '../../utils/alert';
import { TYPE } from '../auth/login/values.constants';
import { ClassifiedContractsService } from '../../services/classified-contracts.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [LoaderComponent, CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {
  loading: boolean = false;
  errorMsg: string = '';
  constructor(private title: Title,
              private contractsService:ContractsService, 
              private classifiedContractsService:ClassifiedContractsService) {
    this.title.setTitle("Dashboard - CMS");
  }

  pieSegments = [
    { color: '#4caf50', percentage: 40 },
    { color: '#2196f3', percentage: 40 },
    { color: '#ff9800', percentage: 10 },
    { color: '#f44336', percentage: 10 }
  ];

  pieChartBackground = '';

  ngOnInit() {
    this.pieChartBackground = this.generateConicGradient(this.pieSegments);
    this.GetContractsCount();
    this.GetClassifiedContractsCount();
  }
  GetContractsCount() {
    this.contractsService.getContractCounts().subscribe({
      next: (response: ContractsCount) => {
        console.log('Contracts', response);
        
      }, error: (error) => {
        this.errorMsg = error.error.message;
        Alert.toast(TYPE.ERROR, true, this.errorMsg);
      }
    });
  }
  GetClassifiedContractsCount() {
    this.classifiedContractsService.getClassifiedContractCounts().subscribe({
      next: (response: ContractsCount) => {
        console.log('Classified Contracts', response);
        
      }, error: (error) => {
        this.errorMsg = error.error.message;
        Alert.toast(TYPE.ERROR, true, this.errorMsg);
      }
    });
  }

  generateConicGradient(segments: { color: string; percentage: number }[]): string {
    let gradient = 'conic-gradient(';
    let currentPercent = 0;

    for (let i = 0; i < segments.length; i++) {
      const start = currentPercent;
      const end = start + segments[i].percentage;
      gradient += `${segments[i].color} ${start}% ${end}%`;
      if (i < segments.length - 1) {
        gradient += ', ';
      }
      currentPercent = end;
    }

    gradient += ')';
    return gradient;
  }
}
