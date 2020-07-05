import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { filter, distinctUntilChanged, mergeMap } from 'rxjs/operators';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

@Component({
	selector: 'atz-book',
	templateUrl: './book.component.html',
	styleUrls: ['./book.component.scss']
})
export class BookComponent implements OnInit {
	link: SafeResourceUrl;
	hideDetails = false;
	content;
	
	constructor(
		private http: HttpClient,
		private route: ActivatedRoute,
		private sanitizer: DomSanitizer
	) { }
	
	ngOnInit(): void {
		this.route.queryParams
			.pipe(
				filter(params => params.book),
				distinctUntilChanged(),
				mergeMap(params => this.http.get("/api/books/" + params.book))
			)
			.subscribe((resp: any) => {
				this.content = resp;
				this.link = this.sanitizer.bypassSecurityTrustResourceUrl(resp.link);
			})
	}
}
