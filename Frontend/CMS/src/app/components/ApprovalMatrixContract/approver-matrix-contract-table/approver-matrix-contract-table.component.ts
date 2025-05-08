import {
  AfterViewInit,
  Component,
  Input,
  ViewChild,
  OnChanges,
  SimpleChanges
} from '@angular/core';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { ApprovalMatrixContract } from '../../../models/approval-matrix-contract';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ApproverMatrixContractTablePaginatorComponent } from '../approver-matrix-contract-table-paginator/approver-matrix-contract-table-paginator.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-approver-matrix-contract-table',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatSortModule,
    MatFormFieldModule,
    MatInputModule,
    ApproverMatrixContractTablePaginatorComponent
  ],
  templateUrl: './approver-matrix-contract-table.component.html',
  styleUrl: './approver-matrix-contract-table.component.css'
})
export class ApproverMatrixContractTableComponent implements AfterViewInit, OnChanges {
  @Input() maxPage: number = 1;
  @Input() records: number = 1;
  @Input() pageNumbers: number[] = [];
  @Input() data:ApprovalMatrixContract[] = [];
  @Input() GetContract!: (id: number, isEdit: boolean) => void;
  @Input() GetPage!: (pgNumber: number) => void;
  
  @ViewChild(MatSort) sort!: MatSort;
  
  displayedColumns: string[] = ['departmentName', 'approverName1', 'approverName2', 'approverName3', 'action'];
  dataSource = new MatTableDataSource<ApprovalMatrixContract>();

  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['data']) {
      this.dataSource.data = this.data;
    }
  }
}