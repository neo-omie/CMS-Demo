import { CommonModule } from '@angular/common';
import { Component, Input, SimpleChanges, TemplateRef, ViewChild } from '@angular/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';

@Component({
  selector: 'app-table',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatSortModule,
    MatFormFieldModule,
    MatInputModule
  ],
  templateUrl: './table.component.html',
  styleUrl: './table.component.css'
})
export class TableComponent {
  @Input() data:any[] = [];
  @Input() displayedColumns:string[] = [];
  @Input() columnsInfo:{[key:string]:{
    'title' ?: string,
    'isSort' ?: boolean,
    'templateRef' : TemplateRef<any> | null,
  }} = {};

  @ViewChild(MatSort) sort!: MatSort;
    
  dataSource = new MatTableDataSource<any>();
  
  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['data']) {
      this.dataSource.data = this.data;
    }
  }
}
