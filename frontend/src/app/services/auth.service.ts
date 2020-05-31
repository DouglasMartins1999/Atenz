import { Router } from '@angular/router';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { ModalService } from './modal.service';

@Injectable({
	providedIn: 'root'
})
export class AuthService {
	private token: string;
	private user: User;

	constructor(
		private http: HttpClient, 
		private modal: ModalService,
		private router: Router
	) {
		this.fetchToken().decodeToken();
	}

	get isAuth(){
		return !!this.retrieveToken()
	}

	get authUser(){
		return this.user
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
		if(!this.token) return null;
		this.user = JSON.parse(atob(this.token.split('.')[1]))
		return this.user;
	}

	signin(username: string, password: string): Observable<User> {
		return this.http.post<{ token: string }>("/api/signin", { username, password })
			.pipe(
				map(item => {
					this.token = item?.token;
					return this.storeToken().decodeToken();
				})
			)
	}

	signout(){
		this.token = null;
		localStorage.removeItem("token");
		this.router.navigate(['login'])
		return this;
	}
}

interface User {
	nameid: string;
	unique_name: string;
	role: string;
}