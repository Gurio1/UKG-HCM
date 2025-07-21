import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import {
  ReactiveFormsModule,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { PersonService } from '../../../services/person.service';
import { AuthService } from '../../../services/auth.service';
import { Location } from '@angular/common';

@Component({
  selector: 'app-person-form',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule],
  templateUrl: './person-form.component.html',
  styleUrls: ['./person-form.component.scss'],
})
export class PersonFormComponent implements OnInit {
  personForm: FormGroup;
  isLoading = false;
  isEditMode = false;
  personId: string | null = null;
  hidePassword = true;

  /** Tracks if user has tried to submit, used to show validation errors */
  submitted = false;

  departments = ['IT', 'HR', 'Finance', 'Marketing', 'Operations', 'Sales'];
  roles = ['Employee', 'Manager', 'HrAdmin'];

  constructor(
    private fb: FormBuilder,
    private personService: PersonService,
    private authService: AuthService,
    private router: Router,
    private route: ActivatedRoute,
    private location: Location
  ) {
    this.personForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.maxLength(50)]],
      lastName: ['', [Validators.required, Validators.maxLength(50)]],
      email: ['', [Validators.required, Validators.email]],
      jobTitle: ['', [Validators.required, Validators.maxLength(100)]],
      salary: [
        '',
        [Validators.required, Validators.min(0), Validators.max(1000000)],
      ],
      department: ['', [Validators.required]],
      role: ['Employee', [Validators.required]],
      password: [
        '',
        [Validators.required, Validators.minLength(8), this.passwordValidator],
      ],
    });
  }

  ngOnInit(): void {
    this.personId = this.route.snapshot.paramMap.get('id');
    this.isEditMode = !!this.personId;

    if (this.isEditMode) {
      this.personForm.removeControl('password');
      this.loadPerson();
    }
  }

  /** Handles form submission for create or update */
  onSubmit(): void {
    this.submitted = true;

    if (this.personForm.invalid) {
      return;
    }

    this.isLoading = true;
    const formValue = this.personForm.value;

    if (this.isEditMode && this.personId) {
      // Update existing person
      const updateReq = { personId: this.personId, ...formValue };
      this.personService.updatePerson(updateReq).subscribe({
        next: () => {
          this.router.navigate(['/persons']);
        },
        error: (err) => this.handleError(err, 'Update failed'),
      });
    } else {
      // Create new person
      this.personService.createPerson(formValue).subscribe({
        next: () => {
          this.router.navigate(['/persons']);
        },
        error: (err) => this.handleError(err, 'Creation failed'),
      });
    }
  }

  /** Loads an existing person's data into the form */
  private loadPerson(): void {
    if (!this.personId) return;

    this.isLoading = true;
    this.personService.getPersonById(this.personId).subscribe({
      next: (person) => {
        this.personForm.patchValue(person);
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
        this.router.navigate(['/persons']);
      },
    });
  }

  cancel(): void {
    this.location.back();
  }

  /** Determines if role field should be editable */
  canEditRole(): boolean {
    return this.authService.hasRole('HrAdmin');
  }

  /** Custom validator to require number and special char */
  private passwordValidator(control: any) {
    const value: string = control.value;
    if (!value) return null;
    const hasNumber = /\d/.test(value);
    const hasSpecial = /[!@#$%^&*()_+\[\]{}:;<>,.?/~\\-]/.test(value);
    return hasNumber && hasSpecial ? null : { invalidPassword: true };
  }

  /** Generic error handler for API calls */
  private handleError(err: any, fallbackMessage: string): void {
    this.isLoading = false;
    const msg = err.error?.Error || fallbackMessage;
    console.error('API Error:', msg);
  }

  // ── Getters for easy template access ─────────────────────────────────────
  get firstName() {
    return this.personForm.get('firstName')!;
  }
  get lastName() {
    return this.personForm.get('lastName')!;
  }
  get email() {
    return this.personForm.get('email')!;
  }
  get jobTitle() {
    return this.personForm.get('jobTitle')!;
  }
  get salary() {
    return this.personForm.get('salary')!;
  }
  get department() {
    return this.personForm.get('department')!;
  }
  get role() {
    return this.personForm.get('role')!;
  }
  get password() {
    return this.personForm.get('password')!;
  }
}
