import { Routes } from '@angular/router';
import { LoginComponent } from './components/identity/login/login.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { PersonFormComponent } from './components/persons/person-form/person-form.component';
import { PersonListComponent } from './components/persons/person-list/person-list.component';
import { SignupComponent } from './components/identity/signup/signup.component';
import { AuthGuard } from './guards/auth.guard';
import { RoleGuard } from './guards/role.guard';
import { PersonProfileComponent } from './components/persons/person-profile/person-profile.component';

export const routes: Routes = [
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'signup', component: SignupComponent },
  {
    path: 'dashboard',
    component: DashboardComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'persons',
    component: PersonListComponent,
    canActivate: [AuthGuard, RoleGuard],
    data: { roles: ['HrAdmin', 'Manager'] },
  },
  {
    path: 'profile',
    component: PersonProfileComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'persons/create',
    component: PersonFormComponent,
    canActivate: [AuthGuard, RoleGuard],
    data: { roles: ['HrAdmin'] },
  },
  {
    path: 'persons/:id/edit',
    component: PersonFormComponent,
    canActivate: [AuthGuard],
  },
  { path: '**', redirectTo: '/dashboard' },
];
