import { Component, OnInit } from '@angular/core';
import { FittrackService } from '../fittrack.service';
import { PseudongrxService } from '../pseudongrx.service';

@Component({
  selector: 'app-workout',
  templateUrl: './workout.component.html',
  styleUrls: ['./workout.component.scss']
})
export class WorkoutComponent implements OnInit {

  constructor(private _ngrx: PseudongrxService, private _fittrack: FittrackService) { }

  ngOnInit(): void {
  }

}
