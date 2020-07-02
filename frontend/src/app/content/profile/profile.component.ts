import { Component, OnInit, ViewChild, AfterContentInit, OnDestroy } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import Swiper from 'swiper';

@Component({
	selector: 'atz-profile',
	templateUrl: './profile.component.html',
	styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
	@ViewChild('wrapper', { static: true })
	contentWrapper;

	public content = null;
	public activeSection = null;
	private sliders = [];
	private sections = {
		watchLater: {
			url: "watchLater",
			isBusy: false,
			pag: 1,
		},
		favCourses: {
			url: "favorite/courses",
			isBusy: false,
			pag: 1,
		}
	}

	constructor (
		public auth: AuthService, 
		private http: HttpClient,
		private router: Router
	) { }
		
	ngOnInit(): void {
		this.http.get("/api/profile")
			.subscribe(data => { 
				this.content = data;
			});
	}

	createSlider(element){
		const slider = new Swiper(element, {
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

		this.sliders.push(slider);
		return slider;
	}
	
	navigate(lesson = undefined){
		this.router.navigate(["view"], { queryParams: { lesson }})
	}

	fetchData(type){
		const section = this.sections[type];
		section.isBusy = true;
		this.http.get("/api/profile/" + section.url + "?pag=" + section.pag)
			.subscribe((data: any[]) => {
				if(data.length){
					this.content[type].push(...data);
					section.isBusy = false;
					section.pag++;
				}
			})
	}

	toggleActive(value){
		if(!this.activeSection){
			this.activeSection = value
			this.content[value].length = 0;
			this.fetchData(value)

		} else {
			this.content[this.activeSection].length = 3;
			this.sections[this.activeSection].isBusy = false;
			this.sections[this.activeSection].pag = 1;
			this.activeSection = null;
		}
	}

	scrollHandler(evnt){
		if(!this.activeSection) return;
		const wrapper = this.contentWrapper.nativeElement;
		const target = evnt.target;

		if(target.offsetHeight + target.scrollTop > wrapper.offsetTop + wrapper.offsetHeight){
			if(this.activeSection && !this.sections[this.activeSection].isBusy){
				this.fetchData(this.activeSection);
			}
		}
	}

	sliderChangeDetection(evnt){
		const parent = evnt.target.parentNode;
		const slider = this.sliders.find(item => item.el === parent);

		if(slider){
			slider.update();
		} else {
			this.createSlider(parent);
		}
	}
}
