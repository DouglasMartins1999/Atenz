import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
	providedIn: 'root'
})
export class LoadingService {
	public visibility = new BehaviorSubject<boolean>(false);
	
	changeVisibility(value: boolean){
		this.visibility.next(value)
		return this;
	}
}