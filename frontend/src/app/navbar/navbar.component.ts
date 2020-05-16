import { Component, OnInit } from '@angular/core';

@Component({
	selector: 'atz-navbar',
	templateUrl: './navbar.component.html',
	styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
	isEnabled: Boolean = true;
	user: String = "H"

	constructor() {}
	
	toggleNavbar(){
		this.isEnabled = !this.isEnabled;
	}
	
	ngOnInit(): void {}
}
