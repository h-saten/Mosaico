import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Faq } from 'mosaico-project';

@Component({
  selector: 'app-faq-row',
  templateUrl: './faq-row.component.html',
  styleUrls: ['./faq-row.component.scss']
})
export class FaqRowComponent implements OnInit {

  showDescription: boolean[] = new Array<boolean>();
  expandedRow: string[] = new Array<string>();

  title: string | null = '';
  description = '';
  isHide = false;

  @Input() i: number; // kolejny numer wiersza na liście
  @Input() row: Faq;
  @Input() canEdit: boolean;
  @Input() projectId: string;
  @Input() pageId: string;
  @Output() onDeleted: EventEmitter<string> = new EventEmitter<string>();
  @Output() onOpenModalEdit: EventEmitter<string> = new EventEmitter<string>();

  constructor() {
  }

  ngOnInit(): void {
    this.title = this.row.title;
    this.description = this.row.content;
  }

  showHideDescription(i: number): void {
    this.showDescription[i] = !this.showDescription[i];
    if (this.showDescription[i] === true) {
      this.expandedRow[i] = 'expanded';

    } else {
      this.expandedRow[i] = '';
    }
  }

  delete(id: string): void {
    this.onDeleted.emit(id);
  }

  openModalEdit(id: string): void {
    this.onOpenModalEdit.emit(id);
  }

  onHide(value: boolean) {
    this.isHide = value;
  }

}
