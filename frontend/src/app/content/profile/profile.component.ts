import { Component, OnInit, AfterViewChecked, AfterViewInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

import Swiper from 'swiper';

@Component({
  selector: 'atz-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit, AfterViewInit {
  content;
  sliders = ["#mostreaded-books", "#recents-books"]

  constructor(
    public auth: AuthService, 
    private http: HttpClient,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.http.get("/api/profile")
      .subscribe(data => this.content = data);
  }

  ngAfterViewInit(){
    for(let s of this.sliders){ 
      new Swiper(s, {
        direction: 'horizontal',
        spaceBetween: 20,
        slidesPerView: 4,
        breakpoints: {
          360: { slidesPerView: 1 },
          480: { slidesPerView: 2 },
          1130: { slidesPerView: 3 },
          1300: { slidesPerView: 4 }
        }
      })
    }
  }

  navigate(lesson = undefined){
    this.router.navigate(["view"], { queryParams: { lesson }})
  }
}
