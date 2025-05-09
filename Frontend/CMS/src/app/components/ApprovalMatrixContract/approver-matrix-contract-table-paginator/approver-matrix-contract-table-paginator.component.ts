import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-approver-matrix-contract-table-paginator',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './approver-matrix-contract-table-paginator.component.html',
  styleUrl: './approver-matrix-contract-table-paginator.component.css'
})
export class ApproverMatrixContractTablePaginatorComponent {
  @Input() maxPage:number = 1;
  @Input() records:number = 1;
  @Input() pageNumbers: number[] = [];
  @Input() GetPage!:(pgNumber:number) => void;
}
