import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'atz-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  recents: number[] = [1, 2, 3];
  reads: number[] = [1, 2, 3, 4];

  constructor() { }

  ngOnInit(): void {
  }

}
