import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
	providedIn: 'root'
})
export class ModalService {
	public content = new BehaviorSubject<ModalData>(new ModalData());

	changeModalContent(modal: ModalData){
		const content = Object.assign(new ModalData(), this.content.value, modal);
		this.content.next(content);
		return this;
	}

	toggleVisibility(){
		this.content.next(new ModalData().setVisibility(!this.content.value.isVisible))
		return this;
	}
}

export class ModalData {
	private service: ModalService;
	public actions: ModalAction[] = []

	constructor( 
		public title?: string, 
		public desc?: string, 
		public isVisible: boolean = false
	) {};

	fromService(service: ModalService){
		this.service = service;
		return this;
	}

	setTitle(title: string){
		this.title = title;
		return this;
	}

	setDescription(desc: string){
		this.desc = desc;
		return this;
	}

	setVisibility(visibility: boolean = true){
		this.isVisible = visibility;
		return this;
	}

	setAction(action: ModalAction): ModalData {
		this.actions.push(action);
		return this;
	}

	setCloseAction(title: string = "Ok, entendi", isPrimary: boolean = true){
		this.actions.push({ 
			title, isPrimary, 
			action: () => this.service.toggleVisibility()
		})
		return this;
	}
}

export interface ModalAction {
	title: string;
	isPrimary?: boolean;
	action?: (any) => void;
}