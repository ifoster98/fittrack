import { Injectable } from '@angular/core';
import { FlattenedSetList } from './models/flattened-set-list.model';
import { Workout } from './swagger/model/models';

@Injectable({
  providedIn: 'root'
})
export class WorkoutService {

  constructor() { }

  flattenWorkoutToList(workout: Workout) : Array<FlattenedSetList>{
    let resultSets = new Array<FlattenedSetList>();
    workout.exercises.forEach(exercise => {
      exercise.sets?.forEach(set => {
        if(exercise.exerciseType !== undefined){
          resultSets.push({exerciseType: exercise.exerciseType, set: set});
        }
      })
    })
    return resultSets;
  }

  getNextElement(flattenedList: Array<FlattenedSetList>, currentElement: FlattenedSetList) : FlattenedSetList | undefined{
    let currentPosition = flattenedList.indexOf(currentElement);
    if(currentPosition == -1) return undefined;
    return flattenedList[currentPosition + 1];
  }

  getPreviousElement(flattenedList: Array<FlattenedSetList>, currentElement: FlattenedSetList) : FlattenedSetList | undefined{
    let currentPosition = flattenedList.indexOf(currentElement);
    if(currentPosition == -1) return undefined;
    return flattenedList[currentPosition - 1];
  }

  hasPreviousElement(flattenedList: Array<FlattenedSetList>, currentElement: FlattenedSetList) : boolean {
    let currentPosition = flattenedList.indexOf(currentElement);
    if(currentPosition == -1) return false;
    return currentPosition > 0;
  }

  hasNextElement(flattenedList: Array<FlattenedSetList>, currentElement: FlattenedSetList) : boolean {
    let currentPosition = flattenedList.indexOf(currentElement);
    if(currentPosition == -1) return false;
    return currentPosition < flattenedList.length - 1;
  }
}
