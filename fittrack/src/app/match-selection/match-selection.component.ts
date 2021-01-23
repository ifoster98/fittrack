import { Component, OnInit } from '@angular/core';
import { FittrackService } from '../fittrack.service';
import { Match, PsuedoNgrxService } from '../psuedo-ngrx.service';

@Component({
  selector: 'app-match-selection',
  templateUrl: './match-selection.component.html',
  styleUrls: ['./match-selection.component.scss']
})
export class MatchSelectionComponent implements OnInit {
  matches: Match[] | null = null;

  constructor(private _ngrx: PsuedoNgrxService, private _fittrack: FittrackService) {}

  ngOnInit(): void {
    this._fittrack.getMatches().subscribe(response => {
      this.matches = response.body;
    });
  }

  chooseMatch(match: Match | null) {
    if(match === null) return;
    this._ngrx.chooseMatch(match);
  }

}
