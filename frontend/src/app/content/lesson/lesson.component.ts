import { Component, OnInit, ElementRef, Injectable } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { filter, mergeMap, map, distinctUntilKeyChanged } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { ModalService, ModalData } from 'src/app/services/modal.service';

@Component({
	selector: 'atz-lesson',
	templateUrl: './lesson.component.html',
	styleUrls: ['./lesson.component.scss']
})
@Injectable({
	providedIn: 'root'
})
export class LessonComponent implements OnInit {
	lesson: Lesson;
	course: Course;
	module: ModuleLesson[];

	lessonTabActive: boolean = false;
	
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
				distinctUntilKeyChanged("lesson"),
				mergeMap(params => this.http.get("/api/courses/module/lesson/" + params.lesson))
			)
			.subscribe((resp: Lesson) => {
				console.log("data")
				this.lesson = resp;
				this.lesson.video = [{ src: resp.link }];
				this.navigate(null, resp.moduleId, resp.courseId)
			})

		this.route.queryParams
			.pipe(
				filter(params => params.course),
				mergeMap(params => this.http.get<Course>("/api/courses/" + params.course))
			)
			.subscribe(resp => {
				this.course = resp;
			});

		this.route.queryParams
			.pipe(
				filter(params => params.module),
				mergeMap(params => this.fetchModuleLesson(params.module))
			)
			.subscribe()
	}

	navigate(lesson = null, module = null, course = null){
		const queryParams: any = {}
		
		if(lesson !== null){
			queryParams.lesson = lesson;
			this.scrollToTop();
		}

		if(module !== null)
		queryParams.module = module;

		if(course !== null)
		queryParams.course = course;

		this.router.navigate(['view'], { queryParams, queryParamsHandling: "merge" })
	}

	

	fetchModuleLesson(id){
		return this.http.get<ModuleLesson[]>("api/courses/module/" + id)
			.pipe(map((resp: ModuleLesson[]) => {
				this.module = resp;
				this.navigate(null, id, null);
			}))
	}

	shareIt(){
		try {
			(navigator as any).share({
				title: this.course.name + " - Atenz",
				text: 'Tenha acesso ao seu material de estudo sempre que precisar. Acesse esse curso no Atenz!',
				url: location.href,
			})
		} catch(e){
			const data = new ModalData()
				.fromService(this.modal)
				.setCloseAction()
				.setTitle("Oops...")
				.setDescription("Aparentemente seu dispositivo não suporta compartilhamento de links pelo navegador")
				.setVisibility(true);

			this.modal.changeModalContent(data);
		}
	}

	favoriteIt(remove = false){
		const deletion = {
			call: this.http.delete(`api/courses/${this.course.id}/favorite`, {}),
			success: "Removemos esse item da sua lista de favoritos. Quando quiser vê-lo novamente, basta fazer uma busca",
			error: "Houve um erro ao remover esse item da sua lista de favoritos, se ele de fato estiver presente, talvez estejamos passando por instabilidades. Tente mais tarde, okay?"
		}

		const addiction = {
			call: this.http.post(`api/courses/${this.course.id}/favorite`, {}),
			success: "Adicionamos esse item a sua lista de favoritos, você poderá encontrá-lo a partir da tela inicial",
			error: "Houve um erro ao adicionar esse item a sua lista de favoritos, talvez ele já tenha sido adicionado ou estamos com problemas temporarios. Confira a lista e tente novamente mais tarde"
		}

		const operation = remove ? deletion : addiction;
		
		operation.call
			.subscribe((resp: boolean ) => {
				const data = new ModalData()
					.fromService(this.modal)
					.setCloseAction()
					.setVisibility(true);

				if(!remove) {
					data.setAction({
						title: "Remover Favorito",
						isPrimary: false,
						action: () => {
							this.modal.toggleVisibility();
							this.favoriteIt(true)
						}
					});
				}

				if(resp) data
					.setTitle("Sucesso!")
					.setDescription(operation.success)

				else data
					.setTitle("Oops...")
					.setDescription(operation.error);
				
				this.modal.changeModalContent(data);
			})
	}

	addToWatchLater(id = this.lesson.id, remove = false){
		const deletion = {
			call: this.http.delete(`/api/courses/module/lesson/${id}/watchlater`, undefined),
			success: "Removemos a marcação dessa aula. Quando quiser vê-lo novamente, basta fazer uma busca. Você pode salvar o curso nos favoritos para facilitar",
			error: "Houve um erro ao remover esse item da sua lista. Se ele de fato estiver presente, talvez estejamos passando por instabilidades. Tente mais tarde, ok?"
		}

		const addiction = {
			call: this.http.post(`/api/courses/module/lesson/${id}/watchlater`, undefined),
			success: "Essa aula foi adicionada para visualização posterior. Pode conferir todas as aulas a partir do seu perfil, na página inicial",
			error: "Não foi possível adicionar essa aula na lista de assistir posteriormente. Talvez ela já esteja cadastrada ou estamos com problemas no momento. Se possível, tente novamente mais tarde"
		}

		const operation = remove ? deletion : addiction;
		
		operation.call
			.subscribe((resp: boolean ) => {
				const data = new ModalData()
					.fromService(this.modal)
					.setCloseAction()
					.setVisibility(true);

				if(!remove) {
					data.setAction({
						title: "Remover Favorito",
						isPrimary: false,
						action: () => {
							this.modal.toggleVisibility();
							this.addToWatchLater(id, true)
						}
					});
				}

				if(resp) data
					.setTitle("Sucesso!")
					.setDescription(operation.success)

				else data
					.setTitle("Oops...")
					.setDescription(operation.error);
				
				this.modal.changeModalContent(data);
			})
	}

	acessLesson(lesson, download = false){
		this.http.get("/api/courses/module/lesson/" + lesson)
			.subscribe((resp: Lesson) => {
				if(download) location.href = resp.link
				else {
					const data = new ModalData()
						.fromService(this.modal)
						.setCloseAction()
						.setVisibility(true)
						.setTitle("Sucesso")
						.setDescription("Acessamos essa aula e a marcamos como vista. Obrigado!");

					this.modal.changeModalContent(data);
				}
			})
	}

	scrollToTop(){
		this.host.nativeElement.scrollTo(0,0);
		return this;
	}

	breakBubbling = (evt: any) => evt.stopPropagation();
	getModuleDuration = () => this.course.modules.map(item => item.duration);
	getLessonAmount = () => this.course?.modules?.reduce((acc, x): number => acc + parseInt(x.lessonsAmount), 0);
}

interface Lesson {
	id: number,
	moduleId: number,
	courseId: number,
	position: number,
	name: string,
	module: string,
	duration: string,
	banner: string,
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