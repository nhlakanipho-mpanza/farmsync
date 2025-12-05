import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment';
import {
  Team,
  CreateTeamDTO,
  UpdateTeamDTO,
  TeamMember,
  CreateTeamMemberDTO
} from '../models/hr.model';

@Injectable({
  providedIn: 'root'
})
export class TeamService {
  private apiUrl = `${environment.apiUrl}/teams`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Team[]> {
    return this.http.get<Team[]>(this.apiUrl);
  }

  getActive(): Observable<Team[]> {
    return this.http.get<Team[]>(`${this.apiUrl}/active`);
  }

  getById(id: string): Observable<Team> {
    return this.http.get<Team>(`${this.apiUrl}/${id}`);
  }

  getWithMembers(id: string): Observable<Team> {
    return this.http.get<Team>(`${this.apiUrl}/${id}/with-members`);
  }

  getByLeader(leaderId: string): Observable<Team[]> {
    return this.http.get<Team[]>(`${this.apiUrl}/leader/${leaderId}`);
  }

  create(team: CreateTeamDTO): Observable<Team> {
    return this.http.post<Team>(this.apiUrl, team);
  }

  update(id: string, team: UpdateTeamDTO): Observable<Team> {
    return this.http.put<Team>(`${this.apiUrl}/${id}`, team);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  // Team Members
  getTeamMembers(teamId: string): Observable<TeamMember[]> {
    return this.http.get<TeamMember[]>(`${this.apiUrl}/${teamId}/members`);
  }

  addTeamMember(teamId: string, member: CreateTeamMemberDTO): Observable<TeamMember> {
    return this.http.post<TeamMember>(`${this.apiUrl}/${teamId}/members`, member);
  }

  removeTeamMember(teamId: string, memberId: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${teamId}/members/${memberId}`);
  }
}
