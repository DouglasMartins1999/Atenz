import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { ModalService, ModalData } from 'src/app/services/modal.service';

@Component({
  selector: 'atz-watching',
  templateUrl: './watching.component.html',
  styleUrls: ['./watching.component.scss']
})
export class WatchingComponent implements OnInit {
  @Input('latest')
  content;

  constructor(private router: Router, private http: HttpClient, private modal: ModalService) { }

  navigate(course = undefined, lesson = undefined){
    this.router.navigate(["view"], { queryParams: { lesson, course }})
  }

  watchLater(id){
    const data = new ModalData().fromService(this.modal).setCloseAction();

		this.http.post(`/api/courses/module/lesson/${id}/watchlater`, undefined)
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

  ngOnInit(): void {
  }

}
