import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError, ObservableInput } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { ModalService, ModalData } from './modal.service';

@Injectable({
	providedIn: 'root'
})
export class AuthService {
	private token: string;
	private user: User;

	constructor(private http: HttpClient, private modal: ModalService) { }

	get isAuth(){
		return !!this.retrieveToken()
	}

	private fetchToken(){
		this.token = localStorage.getItem("token");
		return this;
	}

	storeToken(){
		localStorage.setItem("token", this.token);
		return this;
	}

	retrieveToken(){
		if(!this.token) this.fetchToken();
		return this.token;
	}

	decodeToken(): User {
		return JSON.parse(atob(this.token.split('.')[1]))
	}

	signin(username: string, password: string): Observable<User> {
		return this.http.post<{ token: string }>("/api/signin", { username, password })
			.pipe(
				map(item => {
					this.token = item?.token;
					this.user = this.decodeToken();

					return this.storeToken().user;
				})
			)
	}

	signout(){
		this.token = null;
		localStorage.removeItem("token");
		return this;
	}
}

interface User {
	nameid: string;
	unique_name: string;
	role: string;
}