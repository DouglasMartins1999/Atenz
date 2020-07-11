import { Component, OnInit, HostListener, ViewChild } from '@angular/core';
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
	opts = {
		booksPag: 1,
		coursesPag: 1,
		tab: null,
		favoriteds: false,
		toWatchLater: false,
		watcheds: false,
		isBusy: false
	}

	content = [];
	books = [];
	featured;

	@ViewChild("search_wrapper") wrapper;
	@HostListener("scroll", ["$event.target"])
	scrollHandler(target){
		const wrapper = this.wrapper.nativeElement;
		if(target.offsetHeight + target.scrollTop > wrapper.offsetTop + wrapper.offsetHeight && !this.opts.isBusy){
			this.updateOptions({ booksPag: this.opts.booksPag + 1, coursesPag: this.opts.coursesPag + 1 });
			console.log(this.opts);
		}
	}

	constructor(
		private router: Router,
		private route: ActivatedRoute,
		private http: HttpClient
	) {}

	updateOptions(data){
		this.opts = { ...this.opts, isBusy: true, ...data };
		
		if(this.opts.toWatchLater || this.opts.watcheds){
			if(this.opts.tab !== "l"){
				this.updateOptions({ tab: "l" });
			}

			if(data.tab) return;
		}

		if(!data.booksPag && !data.coursesPag){
			this.featured = null;
			this.content = []
			this.books = [];
			this.opts.booksPag = 1;
			this.opts.coursesPag = 1;
		}

		if([null, "l"].includes(this.opts.tab)){
			this.searchFeatured().subscribe();
		}
		
		this.searchCourses().subscribe();
		this.searchBooks().subscribe();
	}

	ngOnInit(): void {
		this.route.queryParams
			.pipe(filter(params => params.q))
			.subscribe(params => {
				this.query = params.q;
				this.updateOptions({});
			});
	}

	searchCourses(){
		const opts = { ...this.opts }
		const url = "/api/courses/search?q=" + this.query +
					"&pag=" + opts.coursesPag +
					"&type=" + opts.tab +
					"&fav=" + opts.favoriteds +
					"&hist=" + opts.watcheds +
					"&watch=" + opts.toWatchLater;

		return this.http.get(url).pipe(map((data: any[]) => {
			this.content.push(...data);
			this.opts.isBusy = !(data.length > 0);
			this.opts.coursesPag = opts.coursesPag;
		}));
	}

	searchBooks(){
		const opts = { ...this.opts }
		const url = "/api/books/search?q=" + this.query +
				"&pag=" + opts.booksPag +
				"&fav=" + opts.favoriteds +
				"&read=" + opts.watcheds;

		return this.http.get(url).pipe(map((data: any[]) => {
			this.books.push(...data);
			this.opts.isBusy = !(data.length > 0);
			this.opts.booksPag = opts.booksPag;
		}));
	}

	searchFeatured(){
		const opts = { ...this.opts }
		const url = "/api/courses/search/featured?q=" + this.query +
			"&fav=" + opts.favoriteds +
			"&hist=" + opts.watcheds +
			"&watch=" + opts.toWatchLater;

		return this.http.get(url).pipe(map(item => this.featured = item));
	}

	goToItem(item){
		const nav: any = {}
		if(item.lesson && typeof item.lesson == "number") nav.lesson = item.lesson;
		if(item.module && typeof item.module == "number") nav.module = item.module;
		if(item.course && typeof item.course == "number") nav.course = item.course;
		if(item.id && typeof item.id == "number") nav.lesson = item.id;

		this.router.navigate(['view'], { queryParams: nav });
	}

	goToBook(book){
		this.router.navigate(['read'], { queryParams: { book }});
	}
}
