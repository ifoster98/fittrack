import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class PsuedoNgrxService {

  private user: User | undefined = undefined;
  private match: Match | undefined = undefined;
  private matchEvents: MatchEvent[] | undefined = undefined;

  constructor() { }

  login(userId: number | undefined) {
    if(userId !== undefined)
      this.user = {"id": userId};
  }

  logout() {
    this.user = undefined;
    this.match = undefined;
  }

  getUser(): User | undefined {
    return this.user;
  }

  isLoggedIn() : boolean {
    return this.user !== undefined;
  }

  chooseMatch(match: Match) {
    this.match = match;
  }

  endMatch() {
    this.match = undefined;
  }

  getMatch(): Match | undefined {
    return this.match;
  }

  isAtMatch(): boolean {
    return this.match !== undefined;
  }

  setMatchEvents(mEvents: MatchEvent[]) {
    this.matchEvents = mEvents
  }

  endEditing() {
    this.matchEvents = undefined;
  }

  getMatchEvents() : MatchEvent[] | undefined {
    return this.matchEvents;
  }

  hasMatchEventsToEdit(): boolean {
    return this.matchEvents !== undefined;
  }

  currentPage() : ViewPage {
    if(!this.isLoggedIn())
      return ViewPage.None;
    if(!this.isAtMatch())
      return ViewPage.MatchSelection;
    if(!this.hasMatchEventsToEdit())
      return ViewPage.EventEntry;
    return ViewPage.EventEdit;
  }
}

export enum ViewPage {
  None,
  MatchSelection,
  EventEntry,
  EventEdit
}

export interface User {
  id: number
}

export interface Match {
  id: number;
  name: string;
}

export interface Fooble {
  id: number;
  name: string;
}

export interface MatchEvent {
  userId: number;
  matchId: number;
  eventTime: string;
  matchEventType: number;
}
