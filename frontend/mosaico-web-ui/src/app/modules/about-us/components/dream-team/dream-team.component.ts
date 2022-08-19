import { Component, OnInit } from '@angular/core';

import { SocialIconLink } from 'src/app/modules/shared/models';

@Component({
  selector: 'app-dream-team',
  templateUrl: './dream-team.component.html',
  styleUrls: ['./dream-team.component.scss']
})
export class DreamTeamComponent implements OnInit {
  socialMediaIcons: SocialIconLink[] = [
    {
      icon: 'telegram',
      href: 'https://t.me/+iuwmBkNaLERjZjc0'
    },
    {
      icon: 'youTube',
      href: 'https://www.youtube.com/c/RahimBlak1'
    },
    {
      icon: 'linkedIn',
      href: 'https://pl.linkedin.com/in/rahimblak'
    },
    {
      icon: 'facebook',
      href: 'https://www.facebook.com/rahimblak/'
    },
    {
      icon: 'twitter',
      href: 'https://twitter.com/RahimBlak?ref_src=twsrc%5Egoogle%7Ctwcamp%5Eserp%7Ctwgr%5Eauthor'
    },
    {
      icon: 'instagram',
      href: 'https://www.instagram.com/rahimblak/'
    },
    {
      icon: 'medium',
      href: 'https://rahimblak.medium.com/'
    },
    {
      icon: 'tiktok',
      href: 'https://www.tiktok.com/@rahim_blak'
    }
  ];;

  constructor() { }

  ngOnInit(): void {
  }

}
