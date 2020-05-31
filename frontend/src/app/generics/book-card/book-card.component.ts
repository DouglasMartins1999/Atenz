import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'atz-book-card',
  templateUrl: './book-card.component.html',
  styleUrls: ['./book-card.component.scss']
})
export class BookCardComponent implements OnInit {
  @Input()
  book;

  constructor() { }

  ngOnInit(): void {
  }

}
