import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-pagination',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './pagination.component.html',
  styleUrl: './pagination.component.css'
})
export class PaginationComponent {
  @Input() maxPage:number = 1;
  @Input() records:number = 1;
  @Input() pageNumbers: number[] = [];
  @Input() GetPage!:(pgNumber:number) => void;
}
