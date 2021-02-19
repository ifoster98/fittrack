import { Injectable } from '@angular/core';
import { Workout } from './swagger/model/models';

@Injectable({
  providedIn: 'root'
})
export class PseudongrxService {
  private currentWorkout: Workout | undefined = undefined;

  constructor() { }

  setWorkout(workout: Workout){
    this.currentWorkout = workout;
  }

  getCurrentWorkout(): Workout | undefined {
    return this.currentWorkout;
  }

  hasWorkout(): boolean {
    return this.currentWorkout !== undefined;
  }
}
