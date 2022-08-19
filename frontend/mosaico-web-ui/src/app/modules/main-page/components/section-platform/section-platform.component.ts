import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { ViewportScroller } from '@angular/common';
import { Router } from '@angular/router';

interface PlatformItem {
  title: string;
  description: string;
  btnText: string;
  action: string;
}

@Component({
  selector: 'app-section-platform',
  templateUrl: './section-platform.component.html',
  styleUrls: ['./section-platform.component.scss']
})
export class SectionPlatformComponent implements OnInit {
  @Output() activeTab: EventEmitter<number> = new EventEmitter();

  platformDetails: PlatformItem[] = [
    {
      title: 'PF.title_1',
      description: 'PF.desc_1',
      btnText: 'PF.action_1',
      action: 'createProjects' 
    },
    {
      title: 'PF.title_2',
      description: 'PF.desc_2',
      btnText: 'PF.action_2',
      action: 'projects'
    }
  ];

  constructor(
    private viewportScroller: ViewportScroller,
    private router: Router
  ) { }

  ngOnInit(): void {
  }

  onNavigate(title: string) {
    switch(title) {
      case 'projects':
        this.router.navigate(['/projects']);
        break;
      case 'createProjects':
        this.viewportScroller.setOffset([0, 90]);
        this.viewportScroller.scrollToAnchor(title);
        this.activeTab.emit(1);
        break;
    }
  }

}
