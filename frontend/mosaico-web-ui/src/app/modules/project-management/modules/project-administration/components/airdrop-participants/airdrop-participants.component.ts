import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { AirdropParticipant, AirdropService } from 'mosaico-project';
import { ActivatedRoute } from '@angular/router';
import { SubSink } from 'subsink';
import { Store } from '@ngrx/store';
import { selectProjectPreview } from '../../../../store/project.selectors';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { ErrorHandlingService } from 'mosaico-base';
import { FileUpload } from 'primeng/fileupload';
import { LazyLoadEvent } from 'primeng/api';

@Component({
  selector: 'app-airdrop-participants',
  templateUrl: './airdrop-participants.component.html',
  styleUrls: ['./airdrop-participants.component.scss']
})
export class AirdropParticipantsComponent implements OnInit, OnDestroy {
  participants: AirdropParticipant[] = [];
  subs = new SubSink();
  projectId: string;
  airdropId: string;
  isLoading = true;
  currentSkip: number = 0;
  currentTake: number = 10;
  page = 1;
  pageSize = 100;
  totalRecords: number;
  @ViewChild('importFileUploader') importFileUploader: FileUpload;

  constructor(private route: ActivatedRoute, private store: Store, private service: AirdropService, private toastr: ToastrService,
    private handler: ErrorHandlingService, private translate: TranslateService) { }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.store.select(selectProjectPreview).subscribe((p) => {
      this.projectId = p?.project?.id;
      this.airdropId = this.route.snapshot.paramMap.get('id');
      if (this.airdropId && this.airdropId.length > 0 && this.projectId && this.projectId.length > 0) {
       this.getParticipants();
      }
    });
  }

  private getParticipants(): void {
    this.subs.sink = this.service.getParticipants(this.projectId, this.airdropId, this.currentTake, this.currentSkip).subscribe((res) => {
      this.isLoading = false;
      this.participants = res?.data?.entities ?? [];
      this.totalRecords = res?.data?.total ?? 0;
    }, (error) => { });
  }

  onCopied(): void {
    this.toastr.success('Copied!');
  }

  fetchParticipants(event: LazyLoadEvent): void {
    this.isLoading = true;
    this.currentSkip = event.first;
    this.currentTake = event.rows;
    this.getParticipants();
  }

  onImport(e: any): void {
    if(e && e.files && e.files.length > 0) {
      const file = e.files[0] as File;
      this.subs.sink = this.service.import(this.projectId, this.airdropId, file).subscribe((res) => {
        this.toastr.success(this.translate.instant('PROJECT_AIRDROPS.ALERTS.SUCCESS_MSG'));
        this.getParticipants();
        this.cleanFileImport();
      }, (error) => {
        this.toastr.error(this.translate.instant('PROJECT_AIRDROPS.ALERTS.' + error.error.code));
        this.cleanFileImport();
      });
    }
  }

  private cleanFileImport(): void {
    this.importFileUploader.clear();
  }

}
