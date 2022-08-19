import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-company-structure-card',
  templateUrl: './company-structure-card.component.html',
  styleUrls: ['./company-structure-card.component.scss']
})
export class CompanyStructureCardComponent implements OnInit {
  @Input() teamDetails;

  constructor() { }

  ngOnInit(): void {
  }

}
