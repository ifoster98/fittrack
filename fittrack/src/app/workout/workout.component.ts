import { Component, OnInit } from '@angular/core';
import { FittrackService } from '../fittrack.service';
import { PseudongrxService } from '../pseudongrx.service';
import { ExerciseType, Workout } from '../swagger/model/models';
import { WorkoutService } from '../workout.service';

@Component({
  selector: 'app-workout',
  templateUrl: './workout.component.html',
  styleUrls: ['./workout.component.scss']
})
export class WorkoutComponent implements OnInit {
  errorMessage: string = '';

  constructor(private _ngrx: PseudongrxService, private _fittrack: FittrackService, private _workoutService: WorkoutService) { }

  ngOnInit(): void {
    let currentDateTime = this.getCurrentDateWithTimeZone();
    this._fittrack.getWorkoutForDate(currentDateTime).subscribe(response => {
      if(response.body) {
        this.setWorkout(<Workout>response.body);
        this.setFlattenedList(<Workout>response.body);
      }
    });
  }

  setFlattenedList(workout: Workout) {
    let flattenedSetList = this._workoutService.flattenWorkoutToList(workout);
    this._ngrx.setFlattenedSetList(flattenedSetList);
    this._ngrx.setCurrentFlattenedSet(flattenedSetList[0]);
  }

  hasWorkout(): boolean {
    return this._ngrx.hasWorkout();
  }

  getWorkout(): Workout | undefined {
    return this._ngrx.getCurrentWorkout();
  }

  setWorkout(workout: Workout): void {
    this._ngrx.setWorkout(workout);
  }

  getWorkoutName(): string | undefined {
    return this._ngrx.getCurrentWorkout()?.programName;
  }

  getExerciseTypeName(exerciseType: ExerciseType | undefined): string | undefined {
    if(exerciseType === undefined) return ExerciseType[0];
    return ExerciseType[exerciseType];
  }

  getCurrentDateWithTimeZone(): string {
    let currentDateTime = new Date();
    return currentDateTime.toISOString();
  }
}
