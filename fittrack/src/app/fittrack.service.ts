import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Fooble, Match, MatchEvent } from './psuedo-ngrx.service';

@Injectable({
  providedIn: 'root'
})
export class FittrackService {
  private baseUrl = 'http://localhost';

  constructor(private http: HttpClient) { }

  login(userId: number | undefined) {
    return this.http.get(`${this.baseUrl}/LoginWithUserId/${userId}`, { observe: 'response' });
  }

  getMatches() {
    return this.http.get<Match[]>(`${this.baseUrl}/Matches`, { observe: 'response' });
  }

  getEvents() {
    return this.http.get<Fooble[]>(`${this.baseUrl}/Events`, { observe: 'response' });
  }

  getMatchEvents(userId: number | undefined, matchId: number | undefined) {
    return this.http.get<MatchEvent[]>(`${this.baseUrl}/MatchEvent/${userId}/${matchId}`);
  }

  saveEvent(userId:number | undefined, matchId: number | undefined, eventTime: string, matchEventType: number){
    let content = {'userId': userId, 'matchId': matchId, 'eventTime': eventTime, 'matchEventType': matchEventType};
    return this.http.post(`${this.baseUrl}/MatchEvent`, content);
  }
}
