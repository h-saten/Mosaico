import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-action-success',
  templateUrl: './action-success.component.html',
  styleUrls: ['./action-success.component.scss']
})
export class ActionSuccessComponent implements OnInit {
  @Input() successText: string;
  @Input() detailsText: string;

  constructor(public activeModal: NgbActiveModal) { }

  ngOnInit(): void {
  }

}
