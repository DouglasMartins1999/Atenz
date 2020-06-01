import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { filter, mergeMap, map } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';

@Component({
	selector: 'atz-search',
	templateUrl: './search.component.html',
	styleUrls: ['./search.component.scss']
})
export class SearchComponent implements OnInit {
	query: string;
	content;
	featured;

	itens = [false, true, true, false]

	constructor(
		private router: Router,
		private route: ActivatedRoute,
		private http: HttpClient
	) {}

	ngOnInit(): void {
		this.route.queryParams
			.pipe(
				filter(params => params.q),
				mergeMap(params => {
					this.query = params.q;
					return this.search(params.q).pipe(map(item => { 
						this.content = item;
						return params;
					}))
				}),
				mergeMap(params => {
					return this.searchfeatured(params.q).pipe(map(item => {
						this.featured = item;
						return params;
					}))
				})
			)
			.subscribe();
	}

	search(data){
		return this.http.get("/api/courses/search?q=" + data)
	}

	searchfeatured(data){
		return this.http.get("/api/courses/search/featured?q=" + data)
	}

	goToItem(item){
		const nav: any = {}
		if(item.lesson && typeof item.lesson == "number") nav.lesson = item.lesson;
		if(item.module && typeof item.module == "number") nav.module = item.module;
		if(item.course && typeof item.course == "number") nav.course = item.course;
		if(item.id && typeof item.id == "number") nav.lesson = item.id;

		this.router.navigate(['view'], { queryParams: nav })
	}

	
}
