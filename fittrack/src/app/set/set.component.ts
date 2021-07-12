import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FlattenedSet } from '../models/flattened-set.class';
import { PseudongrxService } from '../pseudongrx.service';
import { ExerciseType } from '../swagger/model/models';
import { WorkoutService } from '../workout.service';

@Component({
  selector: 'app-set',
  templateUrl: './set.component.html',
  styleUrls: ['./set.component.scss']
})
export class SetComponent implements OnInit {
  flattenedSet: FlattenedSet;
  setForm: FormGroup;

  constructor(private _ngrx: PseudongrxService, private _workoutService: WorkoutService, private fb: FormBuilder) { 
    let cfs = this._ngrx.getCurrentFlattenedSet();
    if(cfs !== undefined) {
      this.flattenedSet = cfs;
    }
    else {
      this.flattenedSet = {exerciseType: ExerciseType.BenchPress, set: {}};
    }
    this.setForm = this.fb.group({
      exerciseType: [{value: this.getNameOfExerciseType(this.flattenedSet.exerciseType), disabled: true}],
      plannedReps: [this.flattenedSet?.set.plannedReps, [ Validators.required ]],
      plannedWeight: [this.flattenedSet?.set.plannedWeight, [Validators.required]],
      actualReps: [this.flattenedSet?.set.actualReps, [Validators.required]],
      actualWeight: [this.flattenedSet?.set.actualWeight, [Validators.required]]
    });
  }

  ngOnInit(): void {
  }

  hasPrevious(): boolean {
    return this._workoutService.hasPreviousElement(this._ngrx.getFlattenedSetList(), this.flattenedSet);
  }

  hasNext(): boolean { 
    return this._workoutService.hasNextElement(this._ngrx.getFlattenedSetList(), this.flattenedSet);
  }

  previous(): void {
    let newFlattenedSet = this._workoutService.getPreviousElement(this._ngrx.getFlattenedSetList(), this.flattenedSet);
    if(newFlattenedSet !== undefined){
      this.updateCurrentFlattenedSet(newFlattenedSet);
    }
  }

  next(): void {
    let newFlattenedSet = this._workoutService.getNextElement(this._ngrx.getFlattenedSetList(), this.flattenedSet);
    if(newFlattenedSet !== undefined){
      this.updateCurrentFlattenedSet(newFlattenedSet);
    }
  }

  getNameOfExerciseType(exerciseType: ExerciseType) : string {
    return ExerciseType[exerciseType];
  }

  updateCurrentFlattenedSet(newFlattenedSet: FlattenedSet){
      this.flattenedSet = newFlattenedSet;
      this._ngrx.setCurrentFlattenedSet(newFlattenedSet);
      this.setForm.patchValue({exerciseType: this.getNameOfExerciseType(newFlattenedSet.exerciseType)});
      this.setForm.patchValue(this.flattenedSet.set);
  }
}
