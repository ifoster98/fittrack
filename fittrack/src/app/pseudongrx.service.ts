import { Injectable } from '@angular/core';
import { FlattenedSet } from './models/flattened-set.class';
import { Workout } from './swagger/model/models';

@Injectable({
  providedIn: 'root'
})
export class PseudongrxService {
  private currentWorkout: Workout | undefined;
  private flattenedSetList: FlattenedSet[] | undefined;
  private currentFlattenedSet: FlattenedSet | undefined;

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

  setFlattenedSetList(flattenedSetList: FlattenedSet[]) {
    this.flattenedSetList = flattenedSetList;
  }

  getFlattenedSetList(): FlattenedSet[] {
    if(this.flattenedSetList !== undefined)
      return this.flattenedSetList;
    return new Array<FlattenedSet>();
  }

  setCurrentFlattenedSet(currentFlattenedSet: FlattenedSet) {
    this.currentFlattenedSet = currentFlattenedSet;
  }

  getCurrentFlattenedSet(): FlattenedSet | undefined {
    return this.currentFlattenedSet;
  }
}
