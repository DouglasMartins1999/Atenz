import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'atz-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss']
})
export class SearchComponent implements OnInit {
  query = "GraphQL"
  featured = [false, true]

  itens = [false, true, true, false]

  constructor() { }

  ngOnInit(): void {
  }

}
