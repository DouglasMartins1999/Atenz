import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'atz-course-card',
  templateUrl: './course-card.component.html',
  styleUrls: ['./course-card.component.scss']
})
export class CourseCardComponent implements OnInit {
  @Input() course;

  constructor(private router: Router) { }

  ngOnInit(): void {
  }

  navigate(course = undefined, lesson = undefined){
    this.router.navigate(["view"], { queryParams: { lesson, course }})
  }
}
