import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { ErrorHandlingService, PatchModel } from 'mosaico-base';
import { Page, TokenPageService } from 'mosaico-project';
import { ToastrService } from 'ngx-toastr';

import { SubSink } from 'subsink';
import { selectProjectPage } from '../../store';

@Component({
  // selector: 'app-edit-main-colors',
  templateUrl: './edit-main-colors.component.html',
  styleUrls: ['./edit-main-colors.component.scss']
})
export class EditMainColorsComponent implements OnInit {

  dataSavingRequestInProgress = false;
  isEditingIsInProgress = false;

  displayColor1 = '';
  private selectedColor1 = '';

  displayColor2 = '';
  private selectedColor2 = '';

  displayColor3 = '';
  private selectedColor3 = '';

  private subs = new SubSink();
  private page: Page;

  constructor(
    public activeModal: NgbActiveModal,
    private pageService: TokenPageService,
    private store: Store,
    private translateService: TranslateService,
    private toastrService: ToastrService,
    private errorHandling: ErrorHandlingService
  ) {}

  ngOnInit(): void {
    this.subs.sink = this.store.select(selectProjectPage).subscribe((res) => {
      if (res) {
        this.page = res;
        this.getColors();
      }
    });
  }

  getColors(): void {
    if (this.page) {
      this.displayColor1 = this.page.primaryColor ? this.page.primaryColor : '#494642';
      this.displayColor2 = this.page.secondaryColor ? this.page.secondaryColor : '#FFFFFF';
      this.displayColor3 = this.page.coverColor ? this.page.coverColor : '#FFFFFF';
    }
  }

  pickerOpenedEvent(event: boolean): void {
    this.isEditingIsInProgress = event;
  }

  selectedColor1Event(color: string): void {
    this.selectedColor1 = color;
  }

  selectedColor2Event(color: string): void {
    this.selectedColor2 = color;
  }

  selectedColor3Event(color: string): void {
    this.selectedColor3 = color;
  }

  saveColors(): void {
    if (this.selectedColor1 === '') {
      this.selectedColor1 = this.displayColor1;
    }

    if (this.selectedColor2 === '') {
      this.selectedColor2 = this.displayColor2;
    }

    if (this.selectedColor3 === '') {
      this.selectedColor3 = this.displayColor3;
    }

    if (this.selectedColor1 && this.selectedColor2 && this.selectedColor3) {

      if (this.page && this.page.id) {
        this.dataSavingRequestInProgress = true;

        const updateExpression: PatchModel[] = [
          {
            "value": this.selectedColor1,
            "path": "/PrimaryColor",
            "op": "add"
          },
          {
            "value": this.selectedColor2,
            "path": "/SecondaryColor",
            "op": "add"
          },
          {
            "value": this.selectedColor3,
            "path": "/CoverColor",
            "op": "add"
          }
        ];

        this.subs.sink = this.pageService.patchPage(this.page.id, updateExpression).subscribe(() => {
          this.subs.sink = this.translateService.get('PROJECT_OVERVIEW.MESSAGES.COLORS_UPDATED').subscribe((res) => {
            this.toastrService.success(res);
          });

          // do i need to keep additional colors for storage?
          // this.store.dispatch(setPageShortDescription({shortDescription: command.shortDescription}));

          this.activeModal.close();
        }, (error) => {this.errorHandling.handleErrorWithToastr(error); this.dataSavingRequestInProgress = false;} );

      } else {
        // this.subs.sink = this.translateService.get('PROJECT_OVERVIEW.MESSAGES.INVALID_FORM').subscribe((res) => {
        //   this.toastrService.error(res);
        // });
      }
    }
  }

}
