import { Component, Input, OnDestroy, OnInit, Output, EventEmitter } from '@angular/core';
import { ProjectPackage } from 'mosaico-project';
import { SubSink } from 'subsink';
import { PERMISSIONS, ProjectPathEnum } from '../../../constants';
import { Store } from '@ngrx/store';
import { selectProjectPreviewToken } from '../../../store';
import { selectPreviewProjectPermissions } from '../../../store/project.selectors';

@Component({
  selector: 'app-project-packages-row',
  templateUrl: './project-packages-row.component.html',
  styleUrls: ['./project-packages-row.component.scss']
})
export class ProjectPackagesRowComponent implements OnInit, OnDestroy {
  private subs = new SubSink();

  @Input() i: number;
  @Input() row: ProjectPackage;
  @Input() canEdit: boolean;
  @Input() projectId: string;
  @Input() pageId: string;
  @Output() onDeleted: EventEmitter<string> = new EventEmitter<string>();
  @Output() onOpenModalEdit: EventEmitter<string> = new EventEmitter<string>();

  title: string | null = '';
  benefits: string[] | null | undefined = [];

  showBenefits = false;

  projectPathEnum: typeof ProjectPathEnum = ProjectPathEnum;
  projectPath: ProjectPathEnum;

  tokenSymbol: string;
  canInvest = false;

  constructor(
    private store: Store,
  ) {
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.store.select(selectProjectPreviewToken).subscribe((t) => {
      this.tokenSymbol = t?.symbol;
    });

    this.subs.sink = this.store.select(selectPreviewProjectPermissions).subscribe((t) => {
      this.canInvest = t && t[PERMISSIONS.CAN_PURCHASE] === true;
    });

    if (this.row) {
      this.title = this.row.name;
      this.benefits = this.row.benefits ? [...this.row.benefits] : [];
    }
  }

  delete(id: string): void {
    this.onDeleted.emit(id);
  }

  openModalEdit(id: string): void {
    this.onOpenModalEdit.emit(id);
  }

}
