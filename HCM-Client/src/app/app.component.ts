import { Component, OnInit } from '@angular/core';
import { AuthService } from './services/auth.service';
import { NavigationEnd, Router } from '@angular/router';
import { NavbarComponent } from './components/layout/navbar/navbar.component';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { combineLatest, filter, map, Observable, startWith } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  standalone: true,
  imports: [NavbarComponent, RouterOutlet, CommonModule],
})
export class AppComponent {
  title = 'HCM System';
  showNavbar$: Observable<boolean>;

  private publicRoutes = ['/login', '/signup'];

  constructor(private authService: AuthService, private router: Router) {
    const url$ = this.router.events.pipe(
      filter((e) => e instanceof NavigationEnd),
      map(() => this.router.url),
      startWith(this.router.url)
    );

    const isLoggedIn$ = this.authService.currentUser$.pipe(
      map((user) => !!user)
    );

    this.showNavbar$ = combineLatest([isLoggedIn$, url$]).pipe(
      map(([isAuth, url]) => isAuth && !this.isPublicRoute(url))
    );
  }

  private isPublicRoute(url: string): boolean {
    return this.publicRoutes.includes(url);
  }
}
