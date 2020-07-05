import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';

@Component({
	selector: 'atz-navbar',
	templateUrl: './navbar.component.html',
	styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
	isEnabled: Boolean = true;
	user: String = "H"

	constructor(private auth: AuthService) {}
	
	toggleNavbar(){
		this.isEnabled = !this.isEnabled;
	}
	
	ngOnInit(): void {
		this.user = this.auth.authUser.unique_name.slice(0, 1).toUpperCase();
	}
}
