import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'atz-course-card',
  templateUrl: './course-card.component.html',
  styleUrls: ['./course-card.component.scss']
})
export class CourseCardComponent implements OnInit {
  @Input() course;

  constructor() { }

  ngOnInit(): void {
  }

}
