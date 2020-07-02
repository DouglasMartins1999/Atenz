import { Component, OnInit, Input, HostBinding } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'atz-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  @Input("bg-color")
  @HostBinding('style.background')
  background: string = 'transparent';
  isFormActive: boolean = false;

  constructor(private router: Router) { }

  ngOnInit(): void {
  }

  search(event, q){
    event.preventDefault();
    this.router.navigate(['search'], { queryParams: { q }})
  }

  showInput(item){
    this.isFormActive = true;
    item.focus();
  }

  hideInput(){

  }
}
