import { Component, OnInit } from '@angular/core';
import { FittrackService } from '../fittrack.service';
import { PseudongrxService } from '../pseudongrx.service';
import * as moment from 'moment';

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
        this._ngrx.setWorkout(response.body);
      }
      else {
        this.errorMessage = 'Unable to retrieve workout from server.';
      }
    }, error => {
      this.errorMessage = `Unable to retrieve workout from server - ${error}`;
    })
  }
}
