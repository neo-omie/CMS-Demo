import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-masters-screen',
  standalone: true,
  imports: [RouterModule, CommonModule],
  templateUrl: './masters-screen.component.html',
  styleUrl: './masters-screen.component.css'
})
export class MastersScreenComponent {
  constructor(private title:Title) {
    this.title.setTitle("All Masters - CMS");
  }
  cardInfo = [
    {
      url:'/masters/employeeMasters',
      title:'Employee Masters',
      iconClass:'fa-solid fa-users icon'
    },
    {
      url:'/masters/departmentMasters',
      title:'Department Masters',
      iconClass:'fa-solid fa-landmark icon'
    },
    {
      url:'/masters/companyMasters',
      title:'Company Masters',
      iconClass:'fa-solid fa-building icon'
    },
    {
      url:'/masters/documentMasters',
      title:'Document Masters',
      iconClass:'fa-solid fa-file-invoice icon'
    },{
      url:'/masters/apostilleMasters',
      title:'Apostille Master',
      iconClass:'fa-solid fa-certificate icon'
    },
    {
      url:'/masters/contractTypeMasters',
      title:'Contract Type Master',
      iconClass:'fa-solid fa-file-contract icon'
    },
    {
      url:'/masters/escalationContracts',
      title:'Escalation Matrix - Contract',
      iconClass:'fa-solid fa-arrow-trend-up icon'
    },
    {
      url:'/masters/approval-matrix-contract',
      title:'Approval Matrix - Contract',
      iconClass:'fa-solid fa-file-circle-check icon'
    },
    {
      url:'/masters/escalationMOUs',
      title:'Escalation Matrix - MOU',
      iconClass:'fa-solid fa-arrow-trend-up icon'
    },
    {
      url:'/masters/approval-matrix-mou',
      title:'Approval Matrix - MOU',
      iconClass:'fa-solid fa-file-circle-check icon'
    }
  ]
}
