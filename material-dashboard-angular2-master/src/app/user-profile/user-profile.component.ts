import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit {
  activeSection: string = 'profile';

  constructor() { }

  ngOnInit() {
  }

  setActiveSection(section: string) {
    this.activeSection = section;
  }
}
