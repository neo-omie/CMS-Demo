import { Component } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { Router } from '@angular/router';
import { ContractStatus } from '../../../utils/constants';

@Component({
  selector: 'app-contracts-screen',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './contracts-screen.component.html',
  styleUrl: './contracts-screen.component.css'
})
export class ContractsScreenComponent {
  contractStatu = ContractStatus;

  constructor(private title: Title,
    private router: Router) {
    this.title.setTitle("Contracts - CMS");
  }

  goToTable(status:ContractStatus) {
    this.router.navigate(['/contracts/allContracts'], {
      queryParams: { status }
    });
  }
}
