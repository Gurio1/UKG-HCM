import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule, NavigationStart } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
import { User } from '../../../models/auth.model';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
})
export class NavbarComponent implements OnInit {
  currentUser: User | null = null;
  showMenu = false; // for toggling user dropdown

  constructor(private authService: AuthService, private router: Router) {
    // close menu when navigating
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationStart) {
        this.showMenu = false;
      }
    });
  }

  ngOnInit(): void {
    this.authService.currentUser$.subscribe((user) => {
      this.currentUser = user;
    });
  }

  toggleMenu(): void {
    this.showMenu = !this.showMenu;
  }

  logout(): void {
    this.authService.logout().subscribe(() => {
      // optionally navigate to login or home
      this.router.navigate(['/login']);
    });
  }

  canViewPersons(): boolean {
    return (
      this.currentUser?.role === 'HrAdmin' ||
      this.currentUser?.role === 'Manager'
    );
  }

  canCreatePersons(): boolean {
    return this.currentUser?.role === 'HrAdmin';
  }

  getRoleDisplayName(role: string): string {
    switch (role) {
      case 'HrAdmin':
        return 'HR Administrator';
      case 'Manager':
        return 'Manager';
      case 'Employee':
        return 'Employee';
      default:
        return role;
    }
  }
}
