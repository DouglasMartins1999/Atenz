import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { filter, distinctUntilChanged, mergeMap } from 'rxjs/operators';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { ModalService, ModalData } from 'src/app/services/modal.service';

@Component({
	selector: 'atz-book',
	templateUrl: './book.component.html',
	styleUrls: ['./book.component.scss']
})
export class BookComponent implements OnInit {
	hideDetails = false;
	content;
	
	constructor(
		private http: HttpClient,
		private route: ActivatedRoute,
		private modal: ModalService,
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
				this.content.link = this.sanitizer.bypassSecurityTrustResourceUrl(resp.link);
			})
	}

	shareIt(){
		try {
			(navigator as any).share({
				title: `${this.content.title}: ${this.content.author} - Atenz`,
				text: 'Tenha acesso ao seu material de estudo sempre que precisar. Acesse esse livro no Atenz!',
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

	favoriteIt(){
		this.http.post(`api/books/${this.content.id}/favorite`, {})
			.subscribe((resp: boolean ) => {
				const data = new ModalData()
					.fromService(this.modal)
					.setCloseAction()
					.setVisibility(true);

				if(resp) data
					.setTitle("Sucesso!")
					.setDescription("Adicionamos esse livro a sua lista de favoritos, você poderá encontrá-lo a partir da tela inicial")

				else data
					.setTitle("Oops...")
					.setDescription("Houve um erro ao adicionar esse livro a sua lista de favoritos, talvez ele já tenha sido adicionado ou estamos com problemas temporarios. Confira a lista e tente novamente mais tarde");
				
				this.modal.changeModalContent(data);
			})
	}

	markItAsReaded(){
		this.http.post(`api/books/${this.content.id}/read`, {})
			.subscribe((resp: boolean) => {
				const data = new ModalData()
					.fromService(this.modal)
					.setCloseAction()
					.setVisibility(true);

				if(resp) data
					.setTitle("Sucesso!")
					.setDescription("Parabéns por concluir mais uma leitura! Estamos orgulhosos por te ajudar nessa caminhada. Continue assim!")

				else data
					.setTitle("Oops...")
					.setDescription("Estamos com problemas, talvez esse livro já tenha sido marcado anteriormente ou precisamos dar uma arrumada na casa. Tente mais tarde, okay?");
				
				this.modal.changeModalContent(data);
			})
	}
}
