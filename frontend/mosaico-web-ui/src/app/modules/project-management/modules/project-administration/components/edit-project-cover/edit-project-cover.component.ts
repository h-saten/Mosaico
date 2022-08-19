import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngrx/store';
import { DEFAULT_MODAL_OPTIONS } from 'mosaico-base';
import { Page } from 'mosaico-project';
import { ProjectCoverUploadComponent } from 'src/app/modules/project-management/modals';
import { selectProjectPage } from 'src/app/modules/project-management/store';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-edit-project-cover',
  templateUrl: './edit-project-cover.component.html',
  styleUrls: ['./edit-project-cover.component.scss']
})
export class EditProjectCoverComponent implements OnInit, OnDestroy {
  subs = new SubSink();
  page: Page;
  backgroundImage: string;

  constructor(private store: Store, private modalService: NgbModal) { }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.subs.sink = this.store.select(selectProjectPage).subscribe((res) => {
      if (res) {
        this.page = res;
        this.backgroundImage = this.page.coverUrl && this.page.coverUrl.length > 0 ? this.page.coverUrl : '/assets/media/tokenpage/default_header.png';
      }
    });
  }

  openCoverEditingModal(): void {
    const modalRef = this.modalService.open(ProjectCoverUploadComponent, DEFAULT_MODAL_OPTIONS);
    modalRef.componentInstance.currentImgUrl = this.page.coverUrl;
    modalRef.componentInstance.pageId = this.page.id;
  }

}
