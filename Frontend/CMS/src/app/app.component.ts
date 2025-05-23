import { Component, ElementRef, ViewChild, AfterViewInit, HostListener, ChangeDetectorRef } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { SideBarComponent } from './components/side-bar/side-bar.component';
import { jwtDecode } from 'jwt-decode';
import { DecodeToken } from './utils/decodeToken';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, SideBarComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements AfterViewInit {
  title = 'CMS';
  username: string | null = null;
  mainContainerMinHeight: number = 0;
  viewportHeight: number = window.innerHeight;

  @ViewChild('footer', { static: false }) footer!: ElementRef;
  @ViewChild('sidebar', { static: false }) sidebar!: any;


  constructor(private cdr: ChangeDetectorRef) { }

  ngAfterViewInit() {
    setTimeout(() => this.calculateMinHeight());
  }

  @HostListener('window:resize', ['$event'])
  onResize() {
    this.viewportHeight = window.innerHeight;
    this.calculateMinHeight();
  }

  calculateMinHeight() {
    if (this.sidebar?.navbar && this.footer) {
      const navbarHeight = this.sidebar.navbar.nativeElement.offsetHeight;
      const footerHeight = this.footer.nativeElement.offsetHeight;

      this.mainContainerMinHeight = this.viewportHeight - navbarHeight - footerHeight;

      this.cdr.detectChanges(); // Safe here after setTimeout or resize
    } else {
      console.log('Navbar or footer not available yet');
    }
  }

  checkLogin(): boolean {
    if (localStorage.getItem('token') != null) {
      DecodeToken.decodeJWTToken(String(localStorage.getItem('token')));
      this.username = DecodeToken.sub;
      return true;
    }
    return false;
  }

}
