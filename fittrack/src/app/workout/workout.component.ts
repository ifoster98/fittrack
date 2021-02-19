import { Component, OnInit } from '@angular/core';
import { FittrackService } from '../fittrack.service';
import { PseudongrxService } from '../pseudongrx.service';
import * as moment from 'moment';
import { ExerciseType, ProgramType, Workout } from '../swagger/model/models';

@Component({
  selector: 'app-workout',
  templateUrl: './workout.component.html',
  styleUrls: ['./workout.component.scss']
})
export class WorkoutComponent implements OnInit {
  errorMessage: string = '';

  constructor(private _ngrx: PseudongrxService, private _fittrack: FittrackService) { }

  ngOnInit(): void {
    let currentDateTime = (moment(Date())).format('YYYY-MM-DD HH:mm:ss');
    this._fittrack.getWorkoutForDate(currentDateTime).subscribe(response => {
      if(response.body) {
        this.setWorkout(<Workout>response.body);
      }
    });
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
}
