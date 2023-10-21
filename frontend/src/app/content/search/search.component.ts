import { Component, OnInit, HostListener, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { filter, mergeMap, map } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { Title } from '@angular/platform-browser';

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
		mobileTab: "VID",
		favoriteds: false,
		toWatchLater: false,
		watcheds: false,
		isBusy: {
			onBooks: false,
			onCourses: false
		}
	}

	content = [];
	books = [];

	@ViewChild("search_wrapper") wrapper;
	@HostListener("scroll", ["$event.target"])
	scrollHandler(target){
		const wrapper = this.wrapper.nativeElement;
		const scrollPosition = target.offsetHeight + target.scrollTop;
		const totalScroll = wrapper.offsetTop + wrapper.offsetHeight;

		console.log(scrollPosition, totalScroll);

		if(scrollPosition > totalScroll && !(this.opts.isBusy.onBooks && this.opts.isBusy.onCourses)){
			this.updateOptions({ booksPag: this.opts.booksPag + 1, coursesPag: this.opts.coursesPag + 1 });
			console.log(this.opts);
		}
	}

	constructor(
		private router: Router,
		private route: ActivatedRoute,
		private http: HttpClient,
		private title: Title
	) {}

	updateOptions(data){
		this.opts = { ...this.opts, ...data };
		
		if(this.opts.toWatchLater || this.opts.watcheds){
			if(this.opts.tab !== "l"){
				this.updateOptions({ tab: "l" });
			}

			if(data.tab) return;
		}

		this.opts.isBusy.onBooks = !!data.booksPag;
		this.opts.isBusy.onCourses = !!data.coursesPag;

		if(!data.booksPag && !data.coursesPag){
			this.content = []
			this.books = [];
			this.opts.booksPag = 1;
			this.opts.coursesPag = 1;
		}
		
		this.searchCourses().subscribe();
		this.searchBooks().subscribe();
	}

	ngOnInit(): void {
		this.route.queryParams
			.pipe(filter(params => params.q))
			.subscribe(params => {
				this.query = params.q;
				this.title.setTitle("Busca por " + params.q + " - ");
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
			this.opts.isBusy.onCourses = !(data.length > 0);
			this.opts.coursesPag = opts.coursesPag;
		}));
	}

	searchBooks(){
		const opts = { ...this.opts }
		const url = "/api/books/search?q=" + this.query +
				"&lim=" + 9 +
				"&pag=" + opts.booksPag +
				"&fav=" + opts.favoriteds +
				"&read=" + opts.watcheds;

		return this.http.get(url).pipe(map((data: any[]) => {
			this.books.push(...data);
			this.opts.isBusy.onBooks = !(data.length > 0);
			this.opts.booksPag = opts.booksPag;
		}));
	}

	goToItem(item){
		console.log(item);
		const nav: any = {}
		if(item.lesson && typeof item.lesson == "number") nav.lesson = item.lesson;
		if(item.module && typeof item.module == "number") nav.module = item.module;
		if(item.course && typeof item.course == "number") nav.course = item.course;
		// this.router.navigate(['view'], { queryParams: nav });

		const params = new URLSearchParams(nav).toString()
		window.open('view?' + params, '_blank')
	}

	goToBook(book){
		// this.router.navigate(['read'], { queryParams: { book }});
		window.open('read?book=' + book, '_blank')
	}
}
