import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'atz-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  content;

  constructor(
    public auth: AuthService, 
    private http: HttpClient,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.http.get("/api/profile")
      .subscribe(data => this.content = data);
  }

  navigate(lesson = undefined){
    this.router.navigate(["view"], { queryParams: { lesson }})
  }
}
