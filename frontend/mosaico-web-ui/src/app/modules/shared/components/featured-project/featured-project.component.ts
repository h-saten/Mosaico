import { Component, EventEmitter, HostListener, Input, OnInit, Output } from '@angular/core';
import { ProjectsList } from 'mosaico-project';

@Component({
  selector: 'app-featured-project',
  templateUrl: './featured-project.component.html',
  styleUrls: ['./featured-project.component.scss']
})
export class FeaturedProjectComponent implements OnInit {
  @Output() showProjectCard: EventEmitter<string | null> = new EventEmitter(null);
  
  @Input() project: ProjectsList;
  @Input() isProjectItem: boolean = false;
  @Input() isLandingPage: boolean = false;
  @Input() isOverlapOpen: boolean = true;
  @Input() isFirst: boolean = false;

  constructor() { }

  ngOnInit(): void {
  }

  @HostListener('mouseenter')
  onMouseEnter() {
    this.showProjectCard.emit(this.project.id);
  }

  @HostListener('mouseleave')
  onMouseLeave() {
    this.showProjectCard.emit(null);
  }

}
