import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { LoaderComponent } from '../loader/loader.component';
import { MasterEscalationMatrixMouDto, UpdateMatrixMouDto } from '../../models/master-escalation-matrix-mou-dto';
import { EscalationMatrixMouService } from '../../services/escalation-matrix-mou.service';
import { Pagination } from '../../utils/pagination';

@Component({
  selector: 'app-escalation-matrix-mou',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterModule, LoaderComponent],
  templateUrl: './escalation-matrix-mou.component.html',
  styleUrl: './escalation-matrix-mou.component.css'
})
export class EscalationMatrixMouComponent {
loading: boolean = true;
  maxPage = 1;
  pageNumbers = [1, 1, 2, 3, 4, 5];
  // matrixContracts : MasterEscalationMatrixContractDto []=;
  matrixMous?: MasterEscalationMatrixMouDto[];
  escalationMatrixMou? : MasterEscalationMatrixMouDto;
  updateMatrixMou?: UpdateMatrixMouDto;
  errorMsg ?: string

  ngOnInit(): void {
    this.getMatrixMous(1, 10);
  }
  constructor(
    private escalationService: EscalationMatrixMouService,
    private router: Router
  ) {}

  getMatrixMous(pageNumber: number, pageSize: number) {
    this.escalationService
      .getAllMatrixMou(pageNumber, pageSize)
      .subscribe((res) => {
        this.loading = false;
        
        this.matrixMous = res;
        this.pageNumbers[0] = pageNumber;
        if (this.matrixMous != undefined && this.matrixMous.length > 0) {
          let result = Pagination.paginator(pageNumber,this.matrixMous[0].totalRecords,pageSize)
          this.maxPage = result.maxPage;
          this.pageNumbers = result.pageNumbers;
        }
      });
  }

  GetPage(pgNumber:number){
    if(this.maxPage >= pgNumber && pgNumber >= 1){
      this.getMatrixMous(pgNumber, 10);
    }
  }
  GetMatrixMouById(valueId:number){
    this.escalationService.getMatrixMouById(valueId).subscribe({
          next:(response:MasterEscalationMatrixMouDto) => {
            this.escalationMatrixMou = response;
            console.log(response)
          }, 
          error:(error) => {
            console.error('Error :(', error);
            if(error.message !== undefined){
              this.errorMsg = JSON.stringify(error.error.message);
              console.log(this.errorMsg);
            }
            else{
              this.errorMsg = JSON.stringify(error.message);
              console.log(this.errorMsg);
            }
        }});
  }
}
