import { Component, ElementRef, ViewChild, AfterViewInit, HostListener, ChangeDetectorRef, OnInit, effect, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { SideBarComponent } from './components/side-bar/side-bar.component';
import { DecodeToken } from './utils/decodeToken';
import { TYPE } from './components/auth/login/values.constants';
import { Alert } from './utils/alert';
import { RouterService } from './services/router.service';

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
  mainContainerMinHeight = signal(0);
  viewportHeight: number = window.innerHeight;
 
  @ViewChild('footer', { static: false }) footer!: ElementRef;
  @ViewChild('navbar', { static: false }) navbar!: ElementRef;
 
 
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
    if (localStorage.getItem('token') != null) {
      DecodeToken.decodeJWTToken(String(localStorage.getItem('token')));
      this.username = DecodeToken.sub;
      return true;
    }
    return false;
  }

  logoutUser() {
    if (localStorage.getItem('token') != null) {
      localStorage.clear();
      sessionStorage.clear();
      DecodeToken.clearUserCredentials();
      Alert.toast(TYPE.SUCCESS, true, "You've been logged out successfully!");
      this.route.goToLogin();
    }
  }
}
