import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class FittrackService {
  private baseUrl = 'http://localhost:8080';

  constructor(private http: HttpClient) { }

  getWorkoutForDate(workoutDate: string) {
    return this.http.get(`${this.baseUrl}/Workout/${workoutDate}`, { observe: 'response' });
  }
}
