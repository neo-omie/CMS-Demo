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
import { ContractStatus } from '../../utils/constants';
import { Router } from '@angular/router';
import { DecodeToken } from '../../utils/decodeToken';

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
  userRole: string | null = '';
  contractTotal : number = 0;
  contractActive : number = 0;
  contractTerminated : number = 0;
  contractExpired : number = 0;
  contractRenew0 : number = 0;
  contractRenew30 : number = 0;
  contractRenew60 : number = 0;
  contractRenew90 : number = 0;
  classifiedContractTotal : number = 0;
  classifiedContractActive : number = 0;
  classifiedContractTerminated : number = 0;
  classifiedContractExpired : number = 0;
  classifiedContractRenew0 : number = 0;
  classifiedContractRenew30 : number = 0;
  classifiedContractRenew60 : number = 0;
  classifiedContractRenew90 : number = 0;
  contractStatu = ContractStatus;
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
    private router: Router,
    private contractsService: ContractsService,
    private classifiedContractsService: ClassifiedContractsService) {
    this.title.setTitle("Dashboard - CMS");
  }


  ngOnInit() {
    this.GetContractsCount();
    this.GetClassifiedContractsCount();
    this.GetContractCounts();
    this.GetClassifiedContractsCounts();
  }

  ngAfterViewInit() {
    setTimeout(() => this.onResize(), 0);
  }

  @HostListener('window:resize', ['$event'])
  onResize() {
    this.viewportWidth = window.innerWidth;
  }

  checkLogin(): boolean {
      if (localStorage.getItem('token')) {
        this.userRole = DecodeToken.ERole;
        return true;
      }
      return false;
    }

  GetContractCounts(){
    this.contractsService.getContracts({
      PageNumber : 1,
      PageSize : 1000,
      ContractStatus : ContractStatus.Active,
    }).subscribe({
      next:(res) => { this.contractActive = res.length; },
      error:(err) => { Alert.toast(TYPE.ERROR, true, err.error.message); }
    })

    this.contractsService.getContracts({
      PageNumber : 1,
      PageSize : 1000,
      ContractStatus : ContractStatus.Expired,
    }).subscribe({
      next:(res) => { this.contractExpired = res.length; },
      error:(err) => { Alert.toast(TYPE.ERROR, true, err.error.message); }
    })

    this.contractsService.getContracts({
      PageNumber : 1,
      PageSize : 1000,
      ContractStatus : ContractStatus.Terminated,
    }).subscribe({
      next:(res) => { this.contractTerminated = res.length; },
      error:(err) => { Alert.toast(TYPE.ERROR, true, err.error.message); }
    })

    this.contractsService.getContracts({
      PageNumber : 1,
      PageSize : 1000,
      RenewalDueIn : 0,
    }).subscribe({
      next:(res) => { this.contractRenew0 = res.length; },
      error:(err) => { Alert.toast(TYPE.ERROR, true, err.error.message); }
    })

    this.contractsService.getContracts({
      PageNumber : 1,
      PageSize : 1000,
      RenewalDueIn : 30,
    }).subscribe({
      next:(res) => { this.contractRenew30 = res.length; },
      error:(err) => { Alert.toast(TYPE.ERROR, true, err.error.message); }
    })

    this.contractsService.getContracts({
      PageNumber : 1,
      PageSize : 1000,
      RenewalDueIn : 60,
    }).subscribe({
      next:(res) => { this.contractRenew60 = res.length; },
      error:(err) => { Alert.toast(TYPE.ERROR, true, err.error.message); }
    })

    this.contractsService.getContracts({
      PageNumber : 1,
      PageSize : 1000,
      RenewalDueIn : 90,
    }).subscribe({
      next:(res) => { this.contractRenew90 = res.length; },
      error:(err) => { Alert.toast(TYPE.ERROR, true, err.error.message); }
    })
  }

  GetClassifiedContractsCounts(){
    this.classifiedContractsService.getContracts({
      PageNumber : 1,
      PageSize : 1000,
      ContractStatus : ContractStatus.Active,
    }).subscribe({
      next:(res) => { this.classifiedContractActive = res.length; },
      error:(err) => { Alert.toast(TYPE.ERROR, true, err.error.message); }
    })

    this.classifiedContractsService.getContracts({
      PageNumber : 1,
      PageSize : 1000,
      ContractStatus : ContractStatus.Expired,
    }).subscribe({
      next:(res) => { this.classifiedContractExpired = res.length; },
      error:(err) => { Alert.toast(TYPE.ERROR, true, err.error.message); }
    })

    this.classifiedContractsService.getContracts({
      PageNumber : 1,
      PageSize : 1000,
      ContractStatus : ContractStatus.Terminated,
    }).subscribe({
      next:(res) => { this.classifiedContractTerminated = res.length; },
      error:(err) => { Alert.toast(TYPE.ERROR, true, err.error.message); }
    })

    this.classifiedContractsService.getContracts({
      PageNumber : 1,
      PageSize : 1000,
      RenewalDueIn : 0,
    }).subscribe({
      next:(res) => { this.classifiedContractRenew0 = res.length; },
      error:(err) => { Alert.toast(TYPE.ERROR, true, err.error.message); }
    })

    this.classifiedContractsService.getContracts({
      PageNumber : 1,
      PageSize : 1000,
      RenewalDueIn : 30,
    }).subscribe({
      next:(res) => { this.classifiedContractRenew30 = res.length; },
      error:(err) => { Alert.toast(TYPE.ERROR, true, err.error.message); }
    })

    this.classifiedContractsService.getContracts({
      PageNumber : 1,
      PageSize : 1000,
      RenewalDueIn : 60,
    }).subscribe({
      next:(res) => { this.classifiedContractRenew60 = res.length; },
      error:(err) => { Alert.toast(TYPE.ERROR, true, err.error.message); }
    })

    this.classifiedContractsService.getContracts({
      PageNumber : 1,
      PageSize : 1000,
      RenewalDueIn : 90,
    }).subscribe({
      next:(res) => { this.classifiedContractRenew90 = res.length; },
      error:(err) => { Alert.toast(TYPE.ERROR, true, err.error.message); }
    })
  }

  GetContractsCount() {
    this.contractsService.getContractCounts().subscribe({
      next: (response: ContractsCount) => {
        const formatted = []
        const keys = Object.keys(response);
        const values = Object.values(response);
        this.contractTotal = response.allContractsCount;
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
        this.classifiedContractTotal = response.allContractsCount
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

  goToTable(status:ContractStatus) {
    this.router.navigate(['/contracts/allContracts'], {
      queryParams: { status }
    });
  }

  goToTableClassified(status:ContractStatus) {
    this.router.navigate(['/classifiedContracts/allContracts'], {
      queryParams: { status }
    });
  }

  goToTableRenewalIn(renewalIn:string) {
    this.router.navigate(['/contracts/allContracts'], {
      queryParams: { renewalIn }
    });
  }

  goToTableClassifiedRenewalIn(renewalIn:string) {
    this.router.navigate(['/classifiedContracts/allContracts'], {
      queryParams: { renewalIn }
    });
  }

  go(){
    this.router.navigate(['/contracts/allContracts']);
  }
  goClassified(){
    this.router.navigate(['/classifiedContracts/allContracts']);
  }
}
