import { Component, OnInit } from '@angular/core';
import { Team, TeamService } from 'mosaico-base';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-section-our-team',
  templateUrl: './section-our-team.component.html',
  styleUrls: ['./section-our-team.component.scss']
})
export class SectionOurTeamComponent implements OnInit {

  private sub: SubSink = new SubSink();
  teams: Team[] = [];
  constructor(private teamService: TeamService) { }

  ngOnInit(): void {
    this.sub.sink = this.teamService.getTeamMembers().subscribe((res) => {
      this.teams = res?.data;
    });
  }

}
