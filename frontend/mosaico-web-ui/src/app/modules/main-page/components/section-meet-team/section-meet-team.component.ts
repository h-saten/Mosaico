import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { SubSink } from 'subsink';

import { SocialIconLink } from 'src/app/modules/shared/models';

@Component({
  selector: 'app-section-meet-team',
  templateUrl: './section-meet-team.component.html',
  styleUrls: ['./section-meet-team.component.scss']
})
export class SectionMeetTeamComponent implements OnInit {
  subs = new SubSink();

  socialMediaIcons: SocialIconLink[] = [];
  telegramLink: string = '';

  constructor(private translateService: TranslateService) { }

  ngOnInit(): void {
    this.onLangChange();

    this.subs.sink = this.translateService.onLangChange.subscribe(_ => {
      this.onLangChange();
    })
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  onLangChange() {
    this.telegramLink = this.translateService.instant('TELEGRAM_SOCIAL_ICON')
    
    this.socialMediaIcons =  [
      {
        icon: 'telegram',
        href: 'MT.SOCIAL_LINKS.TELEGRAM'
      },
      {
        icon: 'youTube',
        href: 'MT.SOCIAL_LINKS.YOUTUBE'
      },
      {
        icon: 'linkedIn',
        href: 'MT.SOCIAL_LINKS.LINKEDIN'
      },
      {
        icon: 'facebook',
        href: 'MT.SOCIAL_LINKS.FACEBOOK'
      },
      {
        icon: 'twitter',
        href: 'MT.SOCIAL_LINKS.TWITTER'
      },
      {
        icon: 'instagram',
        href: 'MT.SOCIAL_LINKS.INSTAGRAM'
      },
      {
        icon: 'medium',
        href: 'MT.SOCIAL_LINKS.MEDIUM'
      },
      {
        icon: 'tiktok',
        href: 'MT.SOCIAL_LINKS.TIKTOK'
      }
    ];
  }

}
