import { Component, OnInit, Input, EventEmitter, Output, SimpleChanges, OnChanges } from '@angular/core';

@Component({
  selector: 'app-show-more-row',
  templateUrl: './show-more-row.component.html',
  styleUrls: ['./show-more-row.component.scss']
})
export class ShowMoreRowComponent implements OnInit, OnChanges {

  @Input() totalPages: number;
  @Input() totalItems: number;

  @Input() rowsPerPage: number; // default number of lines per page
  @Input() currentListSize: number; // the current number of lines on the page

  @Input() showMoreName: string;
  @Input() itemsOf: string;

  currentPageNumber = 1; // default current page number

  nextPageLoading = false;

  showMoreFrom = 0;
  showMoreTo = 0;

  totalItemsLeft = 0;

  @Output() getListNextEvent: EventEmitter<number> = new EventEmitter<number>();

  constructor() { }

  ngOnInit(): void {

    // Show more: per item {{buyOrdersListCurrent.length + 1}} to {{buyOrdersListCurrent.length + pageSize}}

    this.calculatePageNumbers();
  }

  ngOnChanges(changes: SimpleChanges): void {

    // here you get feedback that there has been a change to the current number of lines on the page
    if (changes.currentListSize) {

      this.calculatePageNumbers();
      if(this.currentListSize === this.rowsPerPage)
        this.currentPageNumber = 1;

      this.nextPageLoading = false;
    }
  }

  private calculatePageNumbers (): void {

    if (this.totalItems > (this.currentListSize + this.rowsPerPage)) {

      // this.showMoreFrom = this.currentListSize + 1;
      // this.showMoreTo = this.currentListSize + this.rowsPerPage;

      this.totalItemsLeft = this.totalItems - this.currentListSize;

    } else {

      // this.showMoreFrom = this.currentListSize + 1;
      // this.showMoreTo = this.totalItems;

      this.totalItemsLeft = this.totalItems - this.currentListSize;
    }
  }

  getListNext(skipRow: number): void {

    this.nextPageLoading = true;

    this.getListNextEvent.next(skipRow);

    this.currentPageNumber = this.currentPageNumber + 1; // to start with 1
  }

}
