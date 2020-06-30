import { Component, OnInit, AfterViewInit, Host, ViewChild } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

import Swiper from 'swiper';

@Component({
	selector: 'atz-profile',
	templateUrl: './profile.component.html',
	styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit, AfterViewInit {
	content;
	activeSection: string = null;
	sliders: string[] = ["#mostreaded-books", "#recents-books"];
	sections = {
		'watchLater': {
			url: "watchLater",
			pag: 1,
			isBusy: false
		},
		'favCourses': {
			url: "favorite/courses",
			pag: 1,
			isBusy: false
		}
	}

	@ViewChild('wrapper', { static: true })
	contentWrapper;
	
	constructor(
		public auth: AuthService, 
		private http: HttpClient,
		private router: Router
	) { }
		
	ngOnInit(): void {
		this.http.get("/api/profile")
			.subscribe(data => this.content = data);
	}

	fetchData(type){
		const section = this.sections[type];
		
		section.isBusy = true;
		this.http.get("/api/profile/" + section.url + "?pag=" + section.pag)
			.subscribe((data: any[]) => {
				if(data.length){
					this.content[type].push(...data);
					section.pag++;
					section.isBusy = false;

				} else {
					section.pag = null;
				}
			})
	}
	
	ngAfterViewInit(){
		for(let s of this.sliders){ 
			new Swiper(s, {
				direction: 'horizontal',
				spaceBetween: 20,
				slidesPerView: 4,
				breakpoints: {
					360: { slidesPerView: 1 },
					480: { slidesPerView: 2 },
					1130: { slidesPerView: 3 },
					1300: { slidesPerView: 4 }
				}
			})
		}
	}
	
	navigate(lesson = undefined){
		this.router.navigate(["view"], { queryParams: { lesson }})
	}

	toggleActive(value){
		if(!this.activeSection){
			this.content[value].length = 0;
			this.fetchData(value)

		} else {
			this.content[this.activeSection].length = 3;
			this.sections[this.activeSection] = {
				...this.sections[this.activeSection],
				isBusy: false,
				pag: 1
			}
		}

		this.activeSection = this.activeSection === value ? null : value;
	}

	scrollHandler(evnt){
		const section = this.activeSection;
		if(!section) return;

		const target = evnt.target;
		const wrapper = this.contentWrapper.nativeElement;

		if(target.offsetHeight + target.scrollTop > wrapper.offsetTop + wrapper.offsetHeight){
			if(section && !this.sections[section].isBusy){
				this.fetchData(section)
			}
		}
	}
}
