import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class RouterService {
  constructor(private route:Router) { }

  goToLogin() {
    this.route.navigate(['/']);
  }
  goToDashboard() {
    this.route.navigate(['/dashboard']);
  }
  notFoundPage() {
    this.route.navigate(['/404NotFound']);
  }
}
