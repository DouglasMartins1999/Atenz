import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { ActivatedRoute, Router, NavigationEnd } from '@angular/router';
import { filter, mergeMap, map } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { ModalService, ModalData } from 'src/app/services/modal.service';

@Component({
	selector: 'atz-lesson',
	templateUrl: './lesson.component.html',
	styleUrls: ['./lesson.component.scss']
})
export class LessonComponent implements OnInit {
	lesson: Lesson;
	course: Course;
	module: ModuleLesson[];
	
	constructor(
		private router: Router,
		private route: ActivatedRoute,
		private http: HttpClient,
		private modal: ModalService,
		private host: ElementRef
	) {}
		
	ngOnInit(): void {
		this.route.queryParams
			.pipe(
				filter(params => params.lesson),
				mergeMap(params => this.http.get("/api/courses/module/lesson/" + params.lesson))
			)
			.subscribe((resp: Lesson) => {
				this.lesson = resp;
				this.lesson.video = [{ src: resp.link }];
			})

		this.route.queryParams
			.pipe(
				filter(params => params.course),
				mergeMap(params => this.http.get<Course>("/api/courses/" + params.course)),
				mergeMap(resp => {
					this.course = resp;
					return this.fetchModuleLesson(resp.modules[0].id)
				})
			)
			.subscribe();

		this.router.events.subscribe((evt) => {
			if (!(evt instanceof NavigationEnd)) return;
			console.log(this.host.nativeElement)
			this.host.nativeElement.scrollTo(0,0)
		});
	}

	nextLesson(){
		this.router.navigate(['view'], { queryParams: { lesson: this.lesson.id + 1 }})
	}

	prevLesson(){
		this.router.navigate(['view'], { queryParams: { lesson: this.lesson.id - 1 }})
	}

	toLesson(lesson){
		this.router.navigate(['view'], { queryParams: { course: this.course.id, lesson }})
	}

	addToWatchLater(){
		const data = new ModalData().fromService(this.modal).setCloseAction();

		this.http.post(`/api/courses/module/lesson/${this.lesson.id}/watchlater`, undefined)
			.subscribe((result: { added: boolean }) => {
				if(result.added){
					data.setTitle("Perfeito!")
						.setDescription("Essa aula foi adicionada para visualização posterior. Pode conferir todas as aulas a partir do seu perfil, na página inicial")
						.setVisibility(true);
				} else {
					data.setTitle("Oops...")
						.setDescription("Não foi possível adicionar essa aula na lista de assistir posteriormente. Talvez ela já esteja cadastrada ou estamos com problemas no momento. Se possível, tente novamente mais tarde")
						.setVisibility(true);
				}

				return this.modal.changeModalContent(data);
			})
	}

	fetchModuleLesson(id){
		return this.http.get<ModuleLesson[]>("api/courses/module/" + id)
			.pipe(map((resp: ModuleLesson[]) => {
				this.module = resp;
			}))
	}
}

interface Lesson {
	id: number,
	position: number,
	name: string,
	module: string,
	duration: string,
	link: string,
	size: number,
	video: Plyr.Source[]
}

interface Course {
	id: number,
	name: string,
	teacher: string,
	description: string,
	banner: string,
	keywords: string[],
	modules: {
		id: number,
		name: string,
		duration: string,
		lessonsAmount: string
	}[]
}

interface ModuleLesson {
	id: number,
	name: string,
	duration: string,
	wasWatched: boolean
}