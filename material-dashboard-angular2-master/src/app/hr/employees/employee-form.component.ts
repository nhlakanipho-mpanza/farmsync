import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { EmployeeService } from '../../core/services/employee.service';
import { ReferenceDataService } from '../../core/services/reference-data.service';

@Component({
  selector: 'app-employee-form',
  templateUrl: './employee-form.component.html',
  styleUrls: ['./employee-form.component.css']
})
export class EmployeeFormComponent implements OnInit {
  employeeForm: FormGroup;
  loading = false;
  isEditMode = false;
  employeeId: string;
  activeTab = 0;

  // Reference data
  positions: any[] = [];
  employmentTypes: any[] = [];
  roleTypes: any[] = [];

  constructor(
    private fb: FormBuilder,
    private employeeService: EmployeeService,
    private referenceDataService: ReferenceDataService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.initForm();
    this.loadReferenceData();

    this.route.params.subscribe(params => {
      if (params['id']) {
        this.isEditMode = true;
        this.employeeId = params['id'];
        this.loadEmployee(this.employeeId);
      }
    });

    // Watch position changes to update hourly rate
    this.employeeForm.get('positionId').valueChanges.subscribe(positionId => {
      const position = this.positions.find(p => p.id === positionId);
      if (position) {
        this.employeeForm.patchValue({ hourlyRate: position.rate });
      }
    });
  }

  initForm() {
    this.employeeForm = this.fb.group({
      // Personal Details
      fullName: ['', Validators.required],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      idNumber: ['', Validators.required],
      dateOfBirth: [''],
      gender: ['', Validators.required],
      
      // Contact Details
      contactNumber: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      address: ['', Validators.required],
      city: ['', Validators.required],
      postalCode: [''],
      
      // Banking Details
      accountHolderName: [''],
      bank: [''],
      accountNumber: [''],
      accountType: [''],
      branchCode: [''],
      
      // Position/Role
      positionId: ['', Validators.required],
      hourlyRate: [{ value: '', disabled: true }],
      employeeNumber: ['', Validators.required],
      hireDate: [''],
      employmentTypeId: [null],
      roleTypeId: [null],
      
      // Emergency Contact
      emergencyName: [''],
      emergencyRelationship: [''],
      emergencyMobile: [''],
      emergencyAddress: [''],
      emergencyConsent: [false],
      
      // Other
      isActive: [true],
      terminationDate: [null]
    });
  }

  loadReferenceData() {
    this.referenceDataService.getPositions().subscribe(data => {
      this.positions = data;
    });

    this.referenceDataService.getEmploymentTypes().subscribe(data => {
      this.employmentTypes = data;
    });

    this.referenceDataService.getRoleTypes().subscribe(data => {
      this.roleTypes = data;
    });
  }

  loadEmployee(id: string) {
    this.loading = true;
    this.employeeService.getById(id).subscribe({
      next: (employee) => {
        // Split full name into first and last name
        const nameParts = employee.fullName?.split(' ') || [];
        const firstName = nameParts[0] || '';
        const lastName = nameParts.slice(1).join(' ') || '';

        this.employeeForm.patchValue({
          ...employee,
          firstName,
          lastName,
          dateOfBirth: employee.dateOfBirth ? new Date(employee.dateOfBirth) : null,
          hireDate: employee.hireDate ? new Date(employee.hireDate) : null,
          terminationDate: employee.terminationDate ? new Date(employee.terminationDate) : null
        });
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading employee:', error);
        this.loading = false;
      }
    });
  }

  setTab(index: number) {
    // Validate current tab before moving
    if (index > this.activeTab && !this.isTabValid(this.activeTab)) {
      return;
    }
    this.activeTab = index;
  }

  nextTab() {
    if (this.isTabValid(this.activeTab) && this.activeTab < 3) {
      this.activeTab++;
    }
  }

  prevTab() {
    if (this.activeTab > 0) {
      this.activeTab--;
    }
  }

  isFirst(): boolean {
    return this.activeTab === 0;
  }

  isLast(): boolean {
    return this.activeTab === 3;
  }

  isTabValid(tabIndex: number): boolean {
    switch (tabIndex) {
      case 0: // Personal Details
        return this.employeeForm.get('firstName').valid &&
               this.employeeForm.get('lastName').valid &&
               this.employeeForm.get('idNumber').valid &&
               this.employeeForm.get('gender').valid &&
               this.employeeForm.get('contactNumber').valid &&
               this.employeeForm.get('email').valid &&
               this.employeeForm.get('address').valid &&
               this.employeeForm.get('city').valid;
      
      case 1: // Banking Details (optional but if filled, must be valid)
        const bankFields = ['accountHolderName', 'bank', 'accountNumber', 'accountType'];
        const anyBankFieldFilled = bankFields.some(field => this.employeeForm.get(field).value);
        if (!anyBankFieldFilled) return true; // All empty is OK
        return bankFields.every(field => this.employeeForm.get(field).valid);
      
      case 2: // Position/Role
        return this.employeeForm.get('positionId').valid &&
               this.employeeForm.get('employeeNumber').valid;
      
      case 3: // Emergency Contact (optional)
        return true;
      
      default:
        return true;
    }
  }

  private formatDate(date: Date | string | null): string | null {
    if (!date) return null;
    const d = new Date(date);
    const year = d.getFullYear();
    const month = String(d.getMonth() + 1).padStart(2, '0');
    const day = String(d.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  onSubmit() {
    if (!this.employeeForm.valid) {
      console.log('Form is invalid');
      return;
    }

    this.loading = true;
    const formValue = this.employeeForm.getRawValue();

    // Combine first and last name into full name
    formValue.fullName = `${formValue.firstName} ${formValue.lastName}`.trim();

    // Format dates
    formValue.dateOfBirth = this.formatDate(formValue.dateOfBirth);
    formValue.hireDate = this.formatDate(formValue.hireDate);
    formValue.terminationDate = this.formatDate(formValue.terminationDate);

    const employeeData = {
      ...formValue,
      tenantId: 1 // TODO: Get from auth service
    };

    console.log('Submitting employee data:', employeeData);

    if (this.isEditMode) {
      this.employeeService.update(this.employeeId, employeeData).subscribe({
        next: () => {
          this.loading = false;
          this.router.navigate(['/hr/employees']);
        },
        error: (error) => {
          console.error('Error updating employee:', error);
          console.log('Error response:', error.error);
          console.log('Error status:', error.status);
          this.loading = false;
        }
      });
    } else {
      this.employeeService.create(employeeData).subscribe({
        next: () => {
          this.loading = false;
          this.router.navigate(['/hr/employees']);
        },
        error: (error) => {
          console.error('Error creating employee:', error);
          console.log('Error response:', error.error);
          console.log('Error status:', error.status);
          this.loading = false;
        }
      });
    }
  }

  onCancel() {
    this.router.navigate(['/hr/employees']);
  }
}
