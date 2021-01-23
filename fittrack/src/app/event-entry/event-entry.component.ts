import { Component, OnInit } from '@angular/core';
import { FittrackService } from '../fittrack.service';
import { Fooble, PsuedoNgrxService } from '../psuedo-ngrx.service';
import { formatDate } from '@angular/common';

@Component({
  selector: 'app-event-entry',
  templateUrl: './event-entry.component.html',
  styleUrls: ['./event-entry.component.scss']
})
export class EventEntryComponent implements OnInit {
  currentMatch: string | undefined = '';
  events: Fooble[] | null = null;
  message: string = '';

  constructor(private _ngrx: PsuedoNgrxService, private _fittrack: FittrackService) {}

  ngOnInit(): void {
    this.currentMatch = this._ngrx.getMatch()?.name;
    this._fittrack.getEvents().subscribe(response => {
      this.events = response.body;
    });
  }

  editEntries() {
    this._fittrack.getMatchEvents(this._ngrx.getUser()?.id, this._ngrx.getMatch()?.id).subscribe(response => {
      console.log(response);
      this._ngrx.setMatchEvents(response);
    });
  }

  endMatch() {
    this._ngrx.endMatch();
  }

  registerEvent(bar: Fooble) {
    let eventTime = formatDate(new Date(), 'yyyy-MM-ddTHH:mm:ss', 'en');
    this._fittrack.saveEvent(this._ngrx.getUser()?.id, this._ngrx.getMatch()?.id, eventTime, bar.id).subscribe(result => {
      this.message = 'Entry saved successfully.';
    }, error => {
      this.message = 'Error saving entry.';
    });
  }

}
