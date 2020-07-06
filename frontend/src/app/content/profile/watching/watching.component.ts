import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { ModalService } from 'src/app/services/modal.service';
import { LessonComponent } from '../../lesson/lesson.component';

@Component({
	selector: 'atz-watching',
	templateUrl: './watching.component.html',
	styleUrls: ['./watching.component.scss']
})
export class WatchingComponent implements OnInit {
	@Input('latest')
	content;
	
	constructor(
		private router: Router, 
		private http: HttpClient, 
		private modal: ModalService,
		public lesson: LessonComponent
	) { }
	
	ngOnInit(): void {
	}	
}
