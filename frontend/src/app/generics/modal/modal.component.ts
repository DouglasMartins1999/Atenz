import { Component, Input } from '@angular/core';

@Component({
	selector: 'atz-modal',
	templateUrl: './modal.component.html',
	styleUrls: ['./modal.component.scss']
})
export class ModalComponent {
	@Input('value')
	public content;
}