import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'atz-lesson',
  templateUrl: './lesson.component.html',
  styleUrls: ['./lesson.component.scss']
})
export class LessonComponent implements OnInit {
  lessons: number[] = [1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7]

  constructor() { }

  ngOnInit(): void {
  }

}
