import { Component, OnInit, EventEmitter, Output, Input } from '@angular/core';

@Component({
  selector: 'app-edit-details',
  templateUrl: './edit-details.component.html',
  styleUrls: ['./edit-details.component.scss']
})
export class EditDetailsComponent implements OnInit {
  @Output() isHidden: EventEmitter<boolean> = new EventEmitter(false);
  @Output() openModal: EventEmitter<boolean> = new EventEmitter(false);
  @Output() confirmDelete: EventEmitter<boolean> = new EventEmitter(false);
  @Input() isHide: boolean = false;

  constructor() { }

  ngOnInit(): void {
  }

  onHide() {
    this.isHide = !this.isHide;
    this.isHidden.emit(this.isHide);
  }

  openModalEdit() {
    this.openModal.emit(true);
  }

  confirmDeleteDialog() {
    this.confirmDelete.emit(true);
  }
}
