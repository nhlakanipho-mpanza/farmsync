import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment';
import {
  Employee,
  CreateEmployeeDTO,
  UpdateEmployeeDTO,
  EmergencyContact,
  CreateEmergencyContactDTO,
  BankDetails,
  CreateBankDetailsDTO,
  BiometricEnrolment,
  CreateBiometricEnrolmentDTO
} from '../models/hr.model';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  private apiUrl = `${environment.apiUrl}/employees`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Employee[]> {
    return this.http.get<Employee[]>(this.apiUrl);
  }

  getActive(): Observable<Employee[]> {
    return this.http.get<Employee[]>(`${this.apiUrl}/active`);
  }

  getById(id: string): Observable<Employee> {
    return this.http.get<Employee>(`${this.apiUrl}/${id}`);
  }

  getByEmployeeNumber(employeeNumber: string): Observable<Employee> {
    return this.http.get<Employee>(`${this.apiUrl}/number/${employeeNumber}`);
  }

  create(employee: CreateEmployeeDTO): Observable<Employee> {
    return this.http.post<Employee>(this.apiUrl, employee);
  }

  update(id: string, employee: UpdateEmployeeDTO): Observable<Employee> {
    return this.http.put<Employee>(`${this.apiUrl}/${id}`, employee);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  // Bank Details
  addBankDetails(employeeId: string, bankDetails: CreateBankDetailsDTO): Observable<BankDetails> {
    return this.http.post<BankDetails>(`${this.apiUrl}/${employeeId}/bank-details`, bankDetails);
  }

  updateBankDetails(employeeId: string, bankDetails: CreateBankDetailsDTO): Observable<BankDetails> {
    return this.http.put<BankDetails>(`${this.apiUrl}/${employeeId}/bank-details`, bankDetails);
  }

  // Biometric Enrolment
  enrollBiometric(employeeId: string, enrolment: CreateBiometricEnrolmentDTO): Observable<BiometricEnrolment> {
    return this.http.post<BiometricEnrolment>(`${this.apiUrl}/${employeeId}/biometric`, enrolment);
  }

  // Emergency Contacts
  addEmergencyContact(employeeId: string, contact: CreateEmergencyContactDTO): Observable<EmergencyContact> {
    return this.http.post<EmergencyContact>(`${this.apiUrl}/${employeeId}/emergency-contacts`, contact);
  }

  deleteEmergencyContact(contactId: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/emergency-contacts/${contactId}`);
  }
}
