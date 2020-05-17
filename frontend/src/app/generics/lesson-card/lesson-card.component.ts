import { Component, OnInit, Input, HostBinding } from '@angular/core';

@Component({
  selector: 'atz-lesson-card',
  templateUrl: './lesson-card.component.html',
  styleUrls: ['./lesson-card.component.scss']
})
export class LessonCardComponent implements OnInit {
  @Input()
  @HostBinding('style.width.%')
  width: number = 100;

  constructor() { }

  ngOnInit(): void {}
}
