import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-no-items',
  templateUrl: './no-items.component.html',
  styleUrls: ['./no-items.component.scss']
})
export class NoItemsComponent implements OnInit {
  @Input() title: string;
  @Input() subTitle: string;
  @Input() description: string;
  @Input() btnText: string;

  constructor() { }

  ngOnInit(): void {
  }

}
