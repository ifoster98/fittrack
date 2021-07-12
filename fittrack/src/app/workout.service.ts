import { flatten } from '@angular/compiler';
import { Injectable } from '@angular/core';
import { FlattenedSet } from './models/flattened-set.class';
import { Workout } from './swagger/model/models';

@Injectable({
  providedIn: 'root'
})
export class WorkoutService {

  constructor() { }

  flattenWorkoutToList(workout: Workout) : Array<FlattenedSet>{
    let resultSets = new Array<FlattenedSet>();
    workout.exercises.forEach(exercise => {
      exercise.sets?.forEach(set => {
        if(exercise.exerciseType !== undefined){
          resultSets.push({exerciseType: exercise.exerciseType, set: set});
        }
      })
    })
    return resultSets;
  }

  getNextElement(flattenedList: Array<FlattenedSet>, currentElement: FlattenedSet) : FlattenedSet | undefined{
    let currentPosition = flattenedList.indexOf(currentElement);
    if(currentPosition == -1) return undefined;
    return flattenedList[currentPosition + 1];
  }

  getPreviousElement(flattenedList: Array<FlattenedSet>, currentElement: FlattenedSet) : FlattenedSet | undefined{
    let currentPosition = flattenedList.indexOf(currentElement);
    if(currentPosition == -1) return undefined;
    return flattenedList[currentPosition - 1];
  }

  hasPreviousElement(flattenedList: Array<FlattenedSet>, currentElement: FlattenedSet) : boolean {
    let currentPosition = flattenedList.indexOf(currentElement);
    if(currentPosition == -1) return false;
    return currentPosition > 0;
  }

  hasNextElement(flattenedList: Array<FlattenedSet>, currentElement: FlattenedSet) : boolean {
    let currentPosition = flattenedList.indexOf(currentElement);
    if(currentPosition == -1) return false;
    return currentPosition < flattenedList.length - 1;
  }
}
