import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'atz-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  user;
  content;
  recents: number[] = [1, 2, 3];
  reads: number[] = [1, 2, 3, 4];

  constructor(public auth: AuthService, private http: HttpClient) { }

  ngOnInit(): void {
    this.http.get("/api/profile")
      .subscribe(data => this.content = data);
  }
}
