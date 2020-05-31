import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'atz-watching',
  templateUrl: './watching.component.html',
  styleUrls: ['./watching.component.scss']
})
export class WatchingComponent implements OnInit {
  @Input('latest')
  content;

  constructor() { }

  ngOnInit(): void {
  }

}
