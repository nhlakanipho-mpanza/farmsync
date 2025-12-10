import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { EmployeeService } from '../../core/services/employee.service';
import { ReferenceDataService } from '../../core/services/reference-data.service';
import { DocumentUploadComponent } from '../../components/document-upload/document-upload.component';
import { forkJoin, of } from 'rxjs';
import { switchMap } from 'rxjs/operators';

interface Permission {
  name: string;
  label: string;
  module: string;
  selected: boolean;
}

@Component({
  selector: 'app-employee-form',
  templateUrl: './employee-form.component.html',
  styleUrls: ['./employee-form.component.css']
})
export class EmployeeFormComponent implements OnInit {
  @ViewChild(DocumentUploadComponent) documentUpload: DocumentUploadComponent;

  employeeForm: FormGroup;
  loading = false;
  isEditMode = false;
  employeeId: string;
  activeTab = 0;

  // Reference data
  positions: any[] = [];
  employmentTypes: any[] = [];
  roles: any[] = [];
  banks: any[] = [];
  accountTypes: any[] = [];
  selectedPosition: any = null;

  // Permissions
  permissionModules: { [key: string]: Permission[] } = {};
  allPermissions: Permission[] = [
    // Inventory
    { name: 'inventory.view', label: 'View Inventory', module: 'Inventory', selected: false },
    { name: 'inventory.create', label: 'Create Inventory Items', module: 'Inventory', selected: false },
    { name: 'inventory.edit', label: 'Edit Inventory Items', module: 'Inventory', selected: false },
    { name: 'inventory.delete', label: 'Delete Inventory Items', module: 'Inventory', selected: false },
    // Procurement
    { name: 'procurement.view', label: 'View Procurement', module: 'Procurement', selected: false },
    { name: 'procurement.create', label: 'Create Purchase Orders', module: 'Procurement', selected: false },
    { name: 'procurement.edit', label: 'Edit Purchase Orders', module: 'Procurement', selected: false },
    { name: 'procurement.approve', label: 'Approve Purchase Orders', module: 'Procurement', selected: false },
    { name: 'procurement.receive', label: 'Receive Goods', module: 'Procurement', selected: false },
    { name: 'procurement.suppliers.view', label: 'View Suppliers', module: 'Procurement', selected: false },
    { name: 'procurement.suppliers.create', label: 'Create Suppliers', module: 'Procurement', selected: false },
    { name: 'procurement.suppliers.edit', label: 'Edit Suppliers', module: 'Procurement', selected: false },
    // HR
    { name: 'hr.view', label: 'View Employees', module: 'HR', selected: false },
    { name: 'hr.create', label: 'Create Employees', module: 'HR', selected: false },
    { name: 'hr.edit', label: 'Edit Employees', module: 'HR', selected: false },
    { name: 'hr.attendance.view', label: 'View Attendance', module: 'HR', selected: false },
    { name: 'hr.attendance.create', label: 'Record Attendance', module: 'HR', selected: false },
    { name: 'hr.tasks.view', label: 'View Tasks', module: 'HR', selected: false },
    { name: 'hr.tasks.create', label: 'Create Tasks', module: 'HR', selected: false },
    { name: 'hr.tasks.edit', label: 'Edit Tasks', module: 'HR', selected: false },
    { name: 'hr.issuing.view', label: 'View Issuing', module: 'HR', selected: false },
    { name: 'hr.issuing.create', label: 'Issue Items', module: 'HR', selected: false },
    { name: 'hr.teams.view', label: 'View Teams', module: 'HR', selected: false },
    { name: 'hr.teams.create', label: 'Create Teams', module: 'HR', selected: false },
    { name: 'hr.teams.edit', label: 'Edit Teams', module: 'HR', selected: false },
    // Fleet
    { name: 'fleet.view', label: 'View Fleet', module: 'Fleet', selected: false },
    { name: 'fleet.create', label: 'Create Vehicles', module: 'Fleet', selected: false },
    { name: 'fleet.edit', label: 'Edit Vehicles', module: 'Fleet', selected: false },
    { name: 'fleet.assign-driver', label: 'Assign Drivers', module: 'Fleet', selected: false },
    { name: 'fleet.gps.view', label: 'View GPS Tracking', module: 'Fleet', selected: false },
    // Administration
    { name: 'admin.users.view', label: 'View Users', module: 'Administration', selected: false },
    { name: 'admin.users.create', label: 'Create Users', module: 'Administration', selected: false },
    { name: 'admin.users.edit', label: 'Edit Users', module: 'Administration', selected: false },
    { name: 'admin.permissions', label: 'Manage Permissions', module: 'Administration', selected: false },
    // Reports
    { name: 'reports.view', label: 'View Reports', module: 'Reports', selected: false },
    { name: 'reports.financial', label: 'Financial Reports', module: 'Reports', selected: false },
    { name: 'reports.hr', label: 'HR Reports', module: 'Reports', selected: false },
  ];

  constructor(
    private fb: FormBuilder,
    private employeeService: EmployeeService,
    private referenceDataService: ReferenceDataService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.initForm();
    this.groupPermissionsByModule();
    this.loadReferenceData();

    this.route.params.subscribe(params => {
      if (params['id']) {
        this.isEditMode = true;
        this.employeeId = params['id'];
        this.loadEmployee(this.employeeId);
      }
    });
  }

  setupPositionChangeHandler() {
    // Watch position changes to update hourly rate and driver license requirement
    this.employeeForm.get('positionId').valueChanges.subscribe(positionId => {
      const position = this.positions.find(p => p.id === positionId);
      this.selectedPosition = position;
      if (position) {
        // Update hourly rate (setValue works even if disabled)
        this.employeeForm.get('hourlyRate').setValue(position.rate);
        
        // Update driver license validation
        const licenseExpiry = this.employeeForm.get('driverLicenseExpiryDate');
        
        if (position.isDriverPosition) {
          licenseExpiry.setValidators([Validators.required]);
        } else {
          licenseExpiry.clearValidators();
          licenseExpiry.setValue('');
        }
        licenseExpiry.updateValueAndValidity();
      }
    });
  }

  initForm() {
    this.employeeForm = this.fb.group({
      // Personal Details
      fullName: [''], // Not required - will be generated from firstName + lastName
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      idNumber: ['', Validators.required],
      dateOfBirth: [''],
      gender: ['', Validators.required],
      
      // Contact Details
      contactNumber: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      address: ['', Validators.required],


      
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
      roleId: ['', Validators.required],
      
      // Driver License (conditional based on position)
      driverLicenseExpiryDate: [''],
      driverLicenseDocumentId: [null],
      
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
      // Setup position change handler AFTER positions are loaded
      this.setupPositionChangeHandler();
    });

    this.referenceDataService.getEmploymentTypes().subscribe(data => {
      this.employmentTypes = data;
    });

    this.referenceDataService.getRoles().subscribe(data => {
      this.roles = data;
      console.log('Roles loaded:', this.roles);
    });

    this.referenceDataService.getBankNames().subscribe(data => {
      this.banks = data;
      console.log('Banks loaded:', this.banks);
    });

    this.referenceDataService.getAccountTypes().subscribe(data => {
      this.accountTypes = data;
      console.log('Account types loaded:', this.accountTypes);
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
          terminationDate: employee.terminationDate ? new Date(employee.terminationDate) : null,
          driverLicenseExpiryDate: employee.driverLicenseExpiryDate ? new Date(employee.driverLicenseExpiryDate) : null
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
    if (!this.isTabValid(this.activeTab)) {
      // Mark all controls in current tab as touched to show validation errors
      this.markTabControlsAsTouched(this.activeTab);
      return;
    }
    
    // Allow navigation to permissions tab if admin
    const maxTab = this.isAdmin() ? 4 : 3;
    if (this.activeTab < maxTab) {
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
    // If admin (permissions tab visible), last tab is 4, otherwise 3
    return this.activeTab === 4 || (this.activeTab === 3 && !this.isAdmin());
  }

  isAdmin(): boolean {
    // TODO: Check if current user is admin from auth service
    // For now, assume admin to show permissions tab
    return true;
  }

  markTabControlsAsTouched(tabIndex: number) {
    let fields: string[] = [];
    
    switch (tabIndex) {
      case 0: // Personal Details
        fields = ['firstName', 'lastName', 'idNumber', 'gender', 'contactNumber', 'email', 'address'];
        break;
      case 1: // Banking Details
        fields = ['accountHolderName', 'bank', 'accountNumber', 'accountType', 'branchCode'];
        break;
      case 2: // Position/Role
        fields = ['positionId', 'employeeNumber'];
        break;
      case 3: // Emergency Contact
        fields = ['emergencyName', 'emergencyRelationship', 'emergencyMobile'];
        break;
    }
    
    fields.forEach(field => {
      const control = this.employeeForm.get(field);
      if (control) {
        control.markAsTouched();
        control.updateValueAndValidity();
      }
    });
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
               this.employeeForm.get('address').valid;
      
      case 1: // Banking Details (optional but if filled, must be valid)
        const bankFields = ['accountHolderName', 'bank', 'accountNumber', 'accountType'];
        const anyBankFieldFilled = bankFields.some(field => this.employeeForm.get(field).value);
        if (!anyBankFieldFilled) return true; // All empty is OK
        return bankFields.every(field => this.employeeForm.get(field).valid);
      
      case 2: // Position/Role
        const positionValid = this.employeeForm.get('positionId').valid &&
                             this.employeeForm.get('employeeNumber').valid &&
                             this.employeeForm.get('roleId').valid;
        
        // If driver position, check license fields
        if (this.selectedPosition?.isDriverPosition) {
          return positionValid &&
                 this.employeeForm.get('driverLicenseExpiryDate').valid;
        }
        return positionValid;
      
      case 3: // Emergency Contact (optional)
        return true;
      
      case 4: // Permissions (optional)
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
    console.log('Submit button clicked');
    console.log('Form valid:', this.employeeForm.valid);
    console.log('Form errors:', this.getFormValidationErrors());
    
    if (!this.employeeForm.valid) {
      console.log('Form is invalid - cannot submit');
      // Mark all controls as touched to show validation errors
      Object.keys(this.employeeForm.controls).forEach(key => {
        const control = this.employeeForm.get(key);
        if (control) {
          control.markAsTouched();
          if (control.invalid) {
            console.log(`Invalid field: ${key}`, control.errors);
          }
        }
      });
      alert('Please fill in all required fields correctly before submitting.');
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
    formValue.driverLicenseExpiryDate = this.formatDate(formValue.driverLicenseExpiryDate);

    const employeeData = {
      ...formValue,
      tenantId: 1 // TODO: Get from auth service
    };

    console.log('Submitting employee data:', employeeData);
    console.log('Is edit mode:', this.isEditMode);

    if (this.isEditMode) {
      console.log('Calling update service with ID:', this.employeeId);
      this.employeeService.update(this.employeeId, employeeData).pipe(
        switchMap((employee) => {
          console.log('Employee updated successfully:', employee);
          // Update/save bank details and emergency contact
          return this.updateBankDetailsAndEmergencyContact(this.employeeId).pipe(
            switchMap(() => of(employee))
          );
        })
      ).subscribe({
        next: (employee) => {
          console.log('Employee and related data updated successfully:', employee);
          this.loading = false;
          alert('Employee updated successfully!');
          this.router.navigate(['/hr/employees']);
        },
        error: (error) => {
          console.error('Error updating employee:', error);
          console.log('Error response:', error.error);
          console.log('Error status:', error.status);
          console.log('Error message:', error.message);
          this.loading = false;
          alert(`Error updating employee: ${error.error?.message || error.message || 'Unknown error'}`);
        }
      });
    } else {
      console.log('Calling create service');
      this.employeeService.create(employeeData).pipe(
        switchMap((employee) => {
          console.log('Employee created successfully:', employee);
          this.employeeId = employee.id; // Set employeeId for document upload
          // Save bank details and emergency contact, then upload documents
          return this.saveBankDetailsAndEmergencyContact(employee.id).pipe(
            switchMap(() => this.uploadDocuments(employee.id)),
            switchMap(() => of(employee))
          );
        })
      ).subscribe({
        next: (employee) => {
          console.log('Employee and related data saved successfully:', employee);
          this.loading = false;
          alert('Employee created successfully!');
          this.router.navigate(['/hr/employees']);
        },
        error: (error) => {
          console.error('Error creating employee:', error.error);
          console.log('Full error object:', JSON.stringify(error, null, 2));
          console.log('Error response:', error.error);
          console.log('Error status:', error.status);
          console.log('Error message:', error.message);
          this.loading = false;
          
          let errorMessage = 'Unknown error occurred';
          if (error.status === 0) {
            errorMessage = 'Cannot connect to server. Please ensure the backend API is running.';
          } else if (error.status === 401) {
            errorMessage = 'Unauthorized. Please login again.';
          } else if (error.status === 400) {
            errorMessage = error.error?.message || 'Invalid data provided';
          } else if (error.error?.message) {
            errorMessage = error.error.message;
          } else if (error.message) {
            errorMessage = error.message;
          }
          
          alert(`Error creating employee: ${error.error}`);
        }
      });
    }
  }

  saveBankDetailsAndEmergencyContact(employeeId: string) {
    const requests = [];
    const formValue = this.employeeForm.getRawValue();

    // Save bank details if provided
    if (formValue.accountNumber) {
      const bankDetails = {
        employeeId: employeeId,
        accountHolder: formValue.accountHolderName || '',
        accountNumber: formValue.accountNumber,
        bankNameId: formValue.bank || null,
        accountTypeId: formValue.accountType || null,
        branchCode: formValue.branchCode || null
      };
      console.log('Saving bank details:', bankDetails);
      requests.push(this.employeeService.addBankDetails(employeeId, bankDetails));
    }

    // Save emergency contact if provided
    if (formValue.emergencyName && formValue.emergencyMobile) {
      const emergencyContact = {
        employeeId: employeeId,
        fullName: formValue.emergencyName,
        relationship: formValue.emergencyRelationship || '',
        contactNumber: formValue.emergencyMobile,
        alternateNumber: null,
        address: formValue.emergencyAddress || null
      };
      console.log('Saving emergency contact:', emergencyContact);
      requests.push(this.employeeService.addEmergencyContact(employeeId, emergencyContact));
    }

    // If there are requests, execute them in parallel, otherwise return completed observable
    return requests.length > 0 ? forkJoin(requests) : of(null);
  }

  updateBankDetailsAndEmergencyContact(employeeId: string) {
    const requests = [];
    const formValue = this.employeeForm.getRawValue();

    // Update/add bank details if provided
    if (formValue.accountNumber) {
      const bankDetails = {
        employeeId: employeeId,
        accountHolder: formValue.accountHolderName || '',
        accountNumber: formValue.accountNumber,
        bankNameId: formValue.bank || null,
        accountTypeId: formValue.accountType || null,
        branchCode: formValue.branchCode || null
      };
      console.log('Updating bank details:', bankDetails);
      requests.push(this.employeeService.updateBankDetails(employeeId, bankDetails));
    }

    // Update/add emergency contact if provided  
    if (formValue.emergencyName && formValue.emergencyMobile) {
      const emergencyContact = {
        employeeId: employeeId,
        fullName: formValue.emergencyName,
        relationship: formValue.emergencyRelationship || '',
        contactNumber: formValue.emergencyMobile,
        alternateNumber: null,
        address: formValue.emergencyAddress || null
      };
      console.log('Updating emergency contact:', emergencyContact);
      requests.push(this.employeeService.addEmergencyContact(employeeId, emergencyContact));
    }

    // If there are requests, execute them in parallel, otherwise return completed observable
    return requests.length > 0 ? forkJoin(requests) : of(null);
  }

  uploadDocuments(employeeId: string) {
    console.log('uploadDocuments called for employeeId:', employeeId);
    console.log('documentUpload component exists:', !!this.documentUpload);
    console.log('documentUpload.selectedFile exists:', this.documentUpload?.selectedFile);
    console.log('isDriverPosition:', this.isDriverPosition());
    console.log('selectedPosition:', this.selectedPosition);
    
    // Upload documents if any files are selected and we're uploading for a driver position
    if (this.documentUpload && this.documentUpload.selectedFile && this.isDriverPosition()) {
      console.log('Uploading document for driver license...');
      this.documentUpload.entityId = employeeId;
      return this.documentUpload.uploadDocument();
    }
    
    console.log('Document upload skipped - conditions not met');
    return of(null);
  }

  onDocumentUploaded(event: any) {
    console.log('Document uploaded:', event);
  }

  onBankChange(bankId: string) {
    const selectedBank = this.banks.find(b => b.id === bankId);
    if (selectedBank) {
      this.employeeForm.patchValue({
        branchCode: selectedBank.branchCode || ''
      });
      console.log('Selected bank changed:', selectedBank);
    }
  }

  onCancel() {
    this.router.navigate(['/hr/employees']);
  }

  groupPermissionsByModule() {
    this.permissionModules = {};
    this.allPermissions.forEach(permission => {
      if (!this.permissionModules[permission.module]) {
        this.permissionModules[permission.module] = [];
      }
      this.permissionModules[permission.module].push(permission);
    });
  }

  getModules(): string[] {
    return Object.keys(this.permissionModules);
  }

  togglePermission(permission: Permission) {
    permission.selected = !permission.selected;
  }

  selectAllInModule(module: string) {
    this.permissionModules[module].forEach(p => p.selected = true);
  }

  deselectAllInModule(module: string) {
    this.permissionModules[module].forEach(p => p.selected = false);
  }

  getSelectedPermissions(): string[] {
    return this.allPermissions
      .filter(p => p.selected)
      .map(p => p.name);
  }
bankchange() {
    const selectedBankName = this.employeeForm.get('bank').value;
    const selectedBank = this.banks.find(bank => bank.name === selectedBankName);
    this.employeeForm.patchValue({ branchCode: selectedBank ? selectedBank.branchCode : '' });
  }

  getFormValidationErrors() {
    const errors: any = {};
    Object.keys(this.employeeForm.controls).forEach(key => {
      const control = this.employeeForm.get(key);
      if (control && control.errors) {
        errors[key] = control.errors;
      }
    });
    return errors;
  }

  isDriverPosition(): boolean {
    return this.selectedPosition?.isDriverPosition === true;
  }
}
