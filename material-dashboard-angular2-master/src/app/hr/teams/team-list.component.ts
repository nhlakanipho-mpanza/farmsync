import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { TeamService } from 'app/core/services/team.service';
import { Team } from 'app/core/models/hr.model';

@Component({
  selector: 'app-team-list',
  templateUrl: './team-list.component.html',
  styleUrls: ['./team-list.component.css']
})
export class TeamListComponent implements OnInit {
  teams: Team[] = [];
  filteredTeams: Team[] = [];
  loading = false;
  searchTerm = '';
  showActiveOnly = true;

  constructor(
    private teamService: TeamService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit() {
    this.loadTeams();
  }

  loadTeams() {
    this.loading = true;
    const request = this.showActiveOnly 
      ? this.teamService.getActive() 
      : this.teamService.getAll();
    
    request.subscribe({
      next: (teams) => {
        this.teams = teams;
        this.filteredTeams = teams;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading teams:', error);
        this.snackBar.open('Error loading teams', 'Close', { duration: 3000 });
        this.loading = false;
      }
    });
  }

  filterTeams() {
    this.filteredTeams = this.teams.filter(team => {
      const matchesSearch = !this.searchTerm || 
        team.name.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        team.description?.toLowerCase().includes(this.searchTerm.toLowerCase());

      return matchesSearch;
    });
  }

  onSearch() {
    this.filterTeams();
  }

  onActiveToggle() {
    this.loadTeams();
  }

  addTeam() {
    this.router.navigate(['/hr/teams/create']);
  }

  viewTeam(id: string) {
    this.router.navigate(['/hr/teams', id]);
  }

  editTeam(id: string) {
    this.router.navigate(['/hr/teams/edit', id]);
  }

  assignMembers(id: string) {
    this.router.navigate(['/hr/teams', id, 'assign']);
  }

  deleteTeam(id: string, name: string) {
    if (confirm(`Are you sure you want to delete team "${name}"?`)) {
      this.teamService.delete(id).subscribe({
        next: () => {
          this.snackBar.open('Team deleted successfully', 'Close', { duration: 3000 });
          this.loadTeams();
        },
        error: (error) => {
          console.error('Error deleting team:', error);
          this.snackBar.open('Error deleting team', 'Close', { duration: 3000 });
        }
      });
    }
  }
}
