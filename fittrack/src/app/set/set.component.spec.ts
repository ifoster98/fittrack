import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FlattenedSet } from '../models/flattened-set.class';
import { PseudongrxService } from '../pseudongrx.service';
import { ExerciseType } from '../swagger/model/models';

import { SetComponent } from './set.component';

describe('SetComponent', () => {
  let component: SetComponent;
  let fixture: ComponentFixture<SetComponent>;
  let pseudongrxServiceStub: Partial<PseudongrxService>;
  let demoFlattenedSetList: FlattenedSet[] = [
    {exerciseType: ExerciseType.Squat, set: {plannedReps: 5, plannedWeight: 80, actualReps: 5, actualWeight: 80, order: 1}},
    {exerciseType: ExerciseType.BenchPress, set: {plannedReps: 5, plannedWeight: 50, actualReps: 5, actualWeight: 50, order: 2}},
    {exerciseType: ExerciseType.BentOverRow, set: {plannedReps: 5, plannedWeight: 40, actualReps: 5, actualWeight: 40, order: 3}},
  ]

  pseudongrxServiceStub = {
    getCurrentFlattenedSet : () => demoFlattenedSetList[1],
    getFlattenedSetList: () => demoFlattenedSetList,
    setCurrentFlattenedSet: () => undefined
  }

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SetComponent ],
      providers: [{provide:PseudongrxService, useValue: pseudongrxServiceStub}],
      imports: [ReactiveFormsModule, FormsModule]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should move to the next set in the list', () => {
    expect(component.flattenedSet).toBe(demoFlattenedSetList[1]);
    component.next();
    expect(component.flattenedSet).toBe(demoFlattenedSetList[2]);
  });

  it('should move to the previous set in the list', () => {
    expect(component.flattenedSet).toBe(demoFlattenedSetList[1]);
    component.previous();
    expect(component.flattenedSet).toBe(demoFlattenedSetList[0]);
  });
});
