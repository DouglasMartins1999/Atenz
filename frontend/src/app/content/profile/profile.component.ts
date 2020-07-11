import { Component, OnInit, ViewChild } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { HttpClient } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import { ModalData, ModalService } from 'src/app/services/modal.service';
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
	public isModalOpen = false;
	private sliders = [];
	private sections = {
		watchLater: {
			url: "watchLater",
			isBusy: false,
			pag: 2,
		},
		favCourses: {
			url: "favorite/courses",
			isBusy: false,
			pag: 2,
		},
		favBooks: {
			url: "favorite/books",
			isBusy: false,
			pag: 2,
		},
		recentBooks: {
			url: "recents/books",
			isBusy: false,
			pag: 2
		},
		recentCourses: {
			url: "recents/courses",
			isBusy: false,
			pag: 2
		}
	}

	constructor (
		public auth: AuthService, 
		private http: HttpClient,
		private router: Router,
		private route: ActivatedRoute,
		private modal: ModalService
	) { }
		
	ngOnInit(): void {
		this.http.get("/api/profile")
			.subscribe(data => { 
				this.content = data;
				this.route.params.subscribe(param => {
					if(param['section'] === "home"){
						this.activeSection = null;
						return;
					}
		
					this.toggleActive(param['section']);
				})
			});
	}

	createSlider(element){
		const slider = new Swiper(element, {
			direction: 'horizontal',
			spaceBetween: 20,
			slidesPerView: 4,
			breakpoints: {
				0: { slidesPerView: 1 },
				480: { slidesPerView: 2 },
				1130: { slidesPerView: 3 },
				1300: { slidesPerView: 4 }
			}
		})

		slider.on('reachEnd', () => {
			this.fetchData(slider.$el[0].id)
				.add(() => slider.update())
				.unsubscribe();
		})

		this.sliders.push(slider);
		return slider;
	}
	
	navigate(lesson = undefined, book = undefined){
		if(lesson){
			this.router.navigate(["view"], { queryParams: { lesson }});
			return;
		}

		if(book){
			this.router.navigate(["read"], { queryParams: { book }});
			return;
		}
	}

	fetchData(type){
		const section = this.sections[type];
		if(section.isBusy) return;

		section.isBusy = true;
		return this.http.get("/api/profile/" + section.url + "?pag=" + section.pag)
			.subscribe((data: any[]) => {
				if(data.length){
					this.content[type].push(...data);
					section.isBusy = false;
					section.pag++;
				}
			})
	}

	clearHistory(mindate, maxdate, lesson, books){
		const modal = new ModalData().setCloseAction().setVisibility(true).fromService(this.modal);
		const url = "/api/profile/history" +
					"?mindate=" + mindate + 
					"&maxdate=" + maxdate +
					"&lessons=" + lesson + 
					"&books=" + books;

		this.isModalOpen = false;
		this.http.delete(url, {})
			.subscribe((data: any) => {
				if(data.changes){
					modal.setTitle("Sucesso").setDescription(`Removemos ${data.changes} iten${data.changes == 1 ? "" : "s"} do seu histórico entre o período solicitado. Essa ação não afeta suas listas de favoritos e visualização posterior`)
				} else {
					modal.setTitle("Oops...").setDescription("Não houve alterações no seu histórico, talvez não houve visualizações no período informado. Tente um valor diferente");
				}
				this.modal.changeModalContent(modal);
			})
	}

	toggleActive(value){
		console.log(value)
		if(this.activeSection !== value){
			this.activeSection = value
			this.fetchData(value)

		} else {
			this.activeSection = null;
		}
	}

	scrollHandler(evnt){
		if(!this.activeSection) return;
		const wrapper = this.contentWrapper.nativeElement;
		const target = evnt.target;

		if(target.offsetHeight + target.scrollTop > wrapper.offsetTop + wrapper.offsetHeight){
			if(this.activeSection){
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
