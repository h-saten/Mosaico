import { Component, OnInit, Input } from '@angular/core';

import { SocialIconLink } from '../../models';

@Component({
  selector: 'app-meet-team-social-links',
  templateUrl: './meet-team-social-links.component.html',
  styleUrls: ['./meet-team-social-links.component.scss']
})
export class MeetTeamSocialLinksComponent implements OnInit {
  @Input() socialMediaIcons: SocialIconLink[] = [];

  constructor() { }

  ngOnInit(): void {
  }

}
