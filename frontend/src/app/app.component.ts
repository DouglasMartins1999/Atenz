import { Component, OnInit, OnDestroy } from '@angular/core';
import { ModalService, ModalData } from './services/modal.service';
import { LoadingService } from './services/loading.service';
import { AuthService } from './services/auth.service';

@Component({
	selector: 'atz-root',
	templateUrl: './app.component.html',
	styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {
	public modalContent: ModalData;
	public loadingStatus: boolean;
	
	constructor(
		private modalService: ModalService,
		private loadingService: LoadingService,
		public authService: AuthService
	){}
	
	ngOnInit(){
		this.modalService.content
			.asObservable()
			.subscribe(data => this.modalContent = data);

		this.loadingService.visibility
			.asObservable()
			.subscribe(data => this.loadingStatus = data);
	}

	ngOnDestroy(){
		this.modalService.content.unsubscribe();
		this.loadingService.visibility.unsubscribe();
	}
}