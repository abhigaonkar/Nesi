# Frontend Coding Guidelines (Angular 18)

## Architecture Principles

### Standalone Components (Mandatory)
- Use standalone components (no NgModules except AppModule if needed)
- Explicit imports in component decorator
- Lazy loading with routes

```typescript
@Component({
  selector: 'app-timesheet',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatTableModule
  ],
  templateUrl: './timesheet.component.html'
})
export class TimesheetComponent { }
```

### Component Types

**Smart (Container) Components:**
- Manage state
- Call services
- Pass data to dumb components
- Handle navigation

**Dumb (Presentational) Components:**
- Only @Input and @Output
- No service dependencies
- Pure presentation logic
- Reusable

### Folder Structure (Mandatory)
```
src/app/
├── core/ (singleton services, guards, interceptors)
│   ├── guards/
│   ├── interceptors/
│   └── services/
├── shared/ (reusable components, pipes, directives)
│   ├── components/
│   ├── pipes/
│   └── directives/
├── features/ (feature modules)
│   ├── auth/
│   ├── dashboard/
│   └── timesheet/
└── models/ (interfaces, types)
```

## Code Style

### Naming Conventions
- **Components:** feature.type.ts (timesheet-list.component.ts)
- **Services:** feature.service.ts (timesheet.service.ts)
- **Interfaces:** PascalCase (TimesheetEntry, User)
- **Variables:** camelCase (userId, timesheetList)
- **Constants:** UPPER_CASE (API_BASE_URL, MAX_HOURS)
- **Observables:** suffix with $ (timesheets$, user$)

### TypeScript Strict Mode
- Enable strict mode in tsconfig.json
- No `any` type (use `unknown` if needed)
- Explicit return types on functions
- No implicit returns

```typescript
// ✅ Good
public getTimesheet(id: number): Observable<Timesheet> {
  return this.http.get<Timesheet>(`${this.apiUrl}/${id}`);
}

// ❌ Bad
public getTimesheet(id) {
  return this.http.get(`${this.apiUrl}/${id}`);
}
```

## Component Guidelines

### Component Structure
```typescript
@Component({ /* ... */ })
export class TimesheetComponent implements OnInit, OnDestroy {
  // 1. Public properties (inputs/outputs)
  @Input() userId!: number;
  @Output() saved = new EventEmitter<void>();
  
  // 2. Public properties (template bindings)
  timesheets: Timesheet[] = [];
  loading = false;
  
  // 3. Private properties
  private destroy$ = new Subject<void>();
  
  // 4. Constructor (DI only)
  constructor(
    private timesheetService: TimesheetService,
    private fb: FormBuilder
  ) {}
  
  // 5. Lifecycle hooks
  ngOnInit(): void {
    this.loadTimesheets();
  }
  
  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
  
  // 6. Public methods (template actions)
  onSubmit(): void {
    // ...
  }
  
  // 7. Private methods
  private loadTimesheets(): void {
    // ...
  }
}
```

### Forms (Reactive Forms Only)
- Always use Reactive Forms
- FormBuilder for creation
- Type-safe form groups
- Custom validators

```typescript
export class TimesheetFormComponent {
  timesheetForm = this.fb.nonNullable.group({
    date: [new Date(), Validators.required],
    hours: [0, [Validators.required, Validators.min(0.25), Validators.max(24)]],
    notes: ['', Validators.maxLength(500)]
  });
  
  constructor(private fb: FormBuilder) {}
  
  onSubmit(): void {
    if (this.timesheetForm.invalid) {
      return;
    }
    
    const value = this.timesheetForm.getRawValue();
    // value is type-safe
  }
}
```

## Services

### Service Structure
- Injectable with providedIn: 'root'
- Single responsibility
- Return Observables
- Handle errors

```typescript
@Injectable({ providedIn: 'root' })
export class TimesheetService {
  private readonly apiUrl = `${environment.apiBaseUrl}/timesheets`;
  
  constructor(private http: HttpClient) {}
  
  getTimesheets(): Observable<Timesheet[]> {
    return this.http.get<Timesheet[]>(this.apiUrl).pipe(
      catchError(error => {
        console.error('Error loading timesheets:', error);
        return throwError(() => new Error('Failed to load timesheets'));
      })
    );
  }
  
  createTimesheet(data: CreateTimesheetRequest): Observable<Timesheet> {
    return this.http.post<Timesheet>(this.apiUrl, data);
  }
}
```

## RxJS Best Practices

### Subscription Management
- Use async pipe in templates (preferred)
- Unsubscribe in ngOnDestroy
- Use takeUntil pattern

```typescript
// ✅ Good - async pipe (no subscription needed)
timesheets$ = this.timesheetService.getTimesheets();

// Template: <div *ngFor="let ts of timesheets$ | async">

// ✅ Good - manual subscription with cleanup
private destroy$ = new Subject<void>();

ngOnInit(): void {
  this.timesheetService.getTimesheets()
    .pipe(takeUntil(this.destroy$))
    .subscribe(timesheets => this.timesheets = timesheets);
}

ngOnDestroy(): void {
  this.destroy$.next();
  this.destroy$.complete();
}

// ❌ Bad - memory leak
ngOnInit(): void {
  this.timesheetService.getTimesheets()
    .subscribe(timesheets => this.timesheets = timesheets);
}
```

### Operators
- Use pipe for composing operators
- Prefer higher-order operators (switchMap, mergeMap)
- Use shareReplay for HTTP caching

```typescript
// ✅ Good
search$ = this.searchControl.valueChanges.pipe(
  debounceTime(300),
  distinctUntilChanged(),
  switchMap(term => this.searchService.search(term)),
  shareReplay(1)
);
```

## HTTP & API

### Interceptors
- Auth token injection
- Error handling
- Loading indicators

```typescript
export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const token = authService.getToken();
  
  if (token) {
    req = req.clone({
      setHeaders: { Authorization: `Bearer ${token}` }
    });
  }
  
  return next(req);
};
```

### Error Handling
- Centralized error handling
- User-friendly messages
- Retry logic for network errors

## Templates

### Syntax
- OnPush change detection where possible
- TrackBy for *ngFor
- Safe navigation operator (?.)
- Strict template checking

```html
<!-- ✅ Good -->
<div *ngFor="let item of items; trackBy: trackById">
  {{ item?.name }}
</div>

<!-- ❌ Bad -->
<div *ngFor="let item of items">
  {{ item.name }}
</div>
```

### Async Pipe
- Prefer async pipe over manual subscriptions
- Use ng-container for multiple async

```html
<!-- ✅ Good -->
<ng-container *ngIf="user$ | async as user">
  <h1>Welcome, {{ user.name }}!</h1>
  <p>{{ user.email }}</p>
</ng-container>
```

## Styling

### SCSS Organization
- Component-specific styles in component.scss
- Global styles in styles.scss
- Variables in _variables.scss
- BEM naming convention

```scss
// Component styles
.timesheet-list {
  &__header {
    display: flex;
    justify-content: space-between;
  }
  
  &__row {
    padding: 1rem;
    
    &--selected {
      background-color: var(--primary-color);
    }
  }
}
```

## Testing

### Unit Tests
- Test component logic
- Mock services
- Test user interactions

```typescript
describe('TimesheetComponent', () => {
  let component: TimesheetComponent;
  let service: jest.Mocked<TimesheetService>;
  
  beforeEach(() => {
    service = {
      getTimesheets: jest.fn().mockReturnValue(of([]))
    } as any;
    
    TestBed.configureTestingModule({
      imports: [TimesheetComponent],
      providers: [
        { provide: TimesheetService, useValue: service }
      ]
    });
    
    const fixture = TestBed.createComponent(TimesheetComponent);
    component = fixture.componentInstance;
  });
  
  it('should load timesheets on init', () => {
    component.ngOnInit();
    expect(service.getTimesheets).toHaveBeenCalled();
  });
});
```

## Performance

### Lazy Loading
- Feature-level lazy loading
- Preload strategy for common routes

```typescript
const routes: Routes = [
  {
    path: 'timesheet',
    loadComponent: () => import('./features/timesheet/timesheet.component')
      .then(m => m.TimesheetComponent)
  }
];
```

### Change Detection
- OnPush strategy for performance
- Immutable data patterns
- Avoid complex expressions in templates

## NPM Packages (Approved)
- @angular/core@18
- @angular/material@18
- @microsoft/signalr
- jwt-decode
- date-fns (prefer over moment.js)
- lodash-es (with tree-shaking)
