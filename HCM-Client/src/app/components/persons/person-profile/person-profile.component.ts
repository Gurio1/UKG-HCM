import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PersonService } from '../../../services/person.service';
import { Person } from '../../../models/person.model';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-person-profile',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './person-profile.component.html',
  styleUrls: ['./person-profile.component.scss'],
})
export class PersonProfileComponent implements OnInit {
  personId!: string;
  person: Person | null = null;
  isLoading = true;
  error: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private personService: PersonService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.personId = this.authService.getCurrentUser().personId;
    this.loadPerson();
  }

  private loadPerson(): void {
    this.isLoading = true;
    this.error = null;
    this.personService.getPersonById(this.personId).subscribe({
      next: (p: Person) => {
        this.person = p;
        this.isLoading = false;
      },
      error: () => {
        this.error = 'Failed to load person details.';
        this.isLoading = false;
      },
    });
  }

  goBack(): void {
    this.router.navigate(['/dashboard']);
  }

  edit(): void {
    this.router.navigate(['/persons', this.personId, 'edit']);
  }
}
