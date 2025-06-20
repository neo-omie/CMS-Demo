import { Component, ElementRef, ViewChild, AfterViewInit, HostListener, ChangeDetectorRef, OnInit, effect, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { SideBarComponent } from './components/side-bar/side-bar.component';
import { DecodeToken } from './utils/decodeToken';
import { TYPE } from './components/auth/login/values.constants';
import { Alert } from './utils/alert';
import { RouterService } from './services/router.service';
import { CommonModule } from '@angular/common';
 
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, SideBarComponent, CommonModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements AfterViewInit {
  title = 'CMS';
  username: string | null = null;
  mainContainerMinHeight = signal(0);
  viewportHeight: number = window.innerHeight;
 
  @ViewChild('footer', { static: false }) footer!: ElementRef;
  @ViewChild('navbar', { static: false }) navbar!: ElementRef;

  showScrollTopBtn:boolean = false;
  @HostListener('window:scroll', [])
  onWindowScroll() {
    const scrollY = window.scrollY || document.documentElement.scrollTop;
    this.showScrollTopBtn = scrollY > 100;  // Toggle at 100px scroll
  }
  scrollToSection() {
    this.navbar.nativeElement.scrollIntoView({behaviour: 'smooth'});
  }
 
  constructor(private route: RouterService) { }
 
  ngAfterViewInit() {
    if (this.checkLogin()) {
      setTimeout(() => {
        this.calculateMinHeight();
      },0);
    }
  }
 
  @HostListener('window:resize', ['$event'])
  onResize() {
    this.viewportHeight = window.innerHeight;
    if (this.checkLogin()) {
      this.calculateMinHeight();
    }
  }
 
  calculateMinHeight() {
    if (this.navbar && this.footer) {
      const navbarHeight = this.navbar.nativeElement.offsetHeight;
      const footerHeight = this.footer.nativeElement.offsetHeight;
 
      this.mainContainerMinHeight.set(this.viewportHeight - navbarHeight - footerHeight);
    }
  }
 
  checkLogin(): boolean {
    if (sessionStorage.getItem('token') != null) {
      DecodeToken.decodeJWTToken(String(sessionStorage.getItem('token')));
      this.username = DecodeToken.sub;
      return true;
    }
    return false;
  }
 
  logoutUser() {
    if (sessionStorage.getItem('token') != null) {
      sessionStorage.clear();
      sessionStorage.clear();
      DecodeToken.clearUserCredentials();
      Alert.toast(TYPE.SUCCESS, true, "You've been logged out successfully!");
      this.route.goToLogin();
    }
  }
}