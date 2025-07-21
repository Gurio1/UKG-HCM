import {
  Component,
  OnInit,
  OnDestroy,
  HostListener,
  ElementRef,
} from '@angular/core';
import { Router, NavigationStart, RouterLink } from '@angular/router';
import { PersonService } from '../../../services/person.service';
import { AuthService } from '../../../services/auth.service';
import { PagedResponse, Person } from '../../../models/person.model';
import { Subscription } from 'rxjs';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-person-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './person-list.component.html',
  styleUrls: ['./person-list.component.scss'],
})
export class PersonListComponent implements OnInit, OnDestroy {
  persons: PagedResponse | null = null;
  isLoading = true;

  // track which person's menu is open
  private openMenuForId: string | null = null;

  private routerSub: Subscription;

  constructor(
    private personService: PersonService,
    private authService: AuthService,
    private router: Router,
    private elementRef: ElementRef
  ) {
    // Close any open menu on route change
    this.routerSub = this.router.events.subscribe((event) => {
      if (event instanceof NavigationStart) {
        this.openMenuForId = null;
      }
    });
  }

  ngOnInit(): void {
    this.loadPersons();
  }

  ngOnDestroy(): void {
    this.routerSub.unsubscribe();
  }

  /** Load a page of persons */
  loadPersons(page: number = 1, pageSize: number = 10): void {
    this.isLoading = true;
    this.openMenuForId = null; // close any open menu
    this.personService.getPersons(page, pageSize).subscribe({
      next: (data) => {
        this.persons = data;
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
        alert('Failed to load persons');
      },
    });
  }

  toggleMenu(person: Person, event: Event): void {
    event.stopPropagation();
    this.openMenuForId = this.openMenuForId === person.id ? null : person.id;
  }

  isMenuOpenFor(person: Person): boolean {
    return this.openMenuForId === person.id;
  }

  /** Close menu when clicking outside */
  @HostListener('document:click', ['$event.target'])
  onClickOutside(target: HTMLElement): void {
    if (this.openMenuForId && !this.elementRef.nativeElement.contains(target)) {
      this.openMenuForId = null;
    }
  }

  /** Pagination helpers */
  prevPage(): void {
    if (this.persons && this.persons.page > 1) {
      this.loadPersons(this.persons.page - 1, this.persons.pageSize);
    }
  }

  nextPage(): void {
    if (this.persons && this.persons.page < this.totalPages()) {
      this.loadPersons(this.persons.page + 1, this.persons.pageSize);
    }
  }

  totalPages(): number {
    return this.persons?.totalPages ?? 1;
  }

  /** Authorization helpers */
  canEdit(person: Person): boolean {
    return this.authService.canEditPerson(person);
  }

  canDelete(): boolean {
    return this.authService.hasRole('HrAdmin');
  }

  canCreate(): boolean {
    return this.authService.hasRole('HrAdmin');
  }

  /** Delete with confirmation prompt */
  deletePerson(person: Person): void {
    if (confirm(`Delete ${person.firstName} ${person.lastName}?`)) {
      this.personService.deletePerson(person.id).subscribe({
        next: () =>
          this.loadPersons(this.persons?.page, this.persons?.pageSize),
        error: () => alert('Delete failed'),
      });
    }
  }

  /** Display role nicely */
  getRoleDisplayName(role: string): string {
    switch (role) {
      case 'HrAdmin':
        return 'HR Admin';
      case 'Manager':
        return 'Manager';
      case 'Employee':
        return 'Employee';
      default:
        return role;
    }
  }
}
