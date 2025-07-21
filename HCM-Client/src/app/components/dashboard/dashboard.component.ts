import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { User } from '../../models/auth.model';
import { CommonModule } from '@angular/common';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { RouterLink } from '@angular/router';

@Component({
  imports: [CommonModule, MatProgressSpinnerModule, RouterLink],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
  standalone: true,
})
export class DashboardComponent implements OnInit {
  currentUser: User | null = null;
  isLoading = true;

  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    this.currentUser = this.authService.getCurrentUser();
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
