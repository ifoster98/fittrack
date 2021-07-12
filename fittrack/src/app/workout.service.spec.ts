import { TestBed } from '@angular/core/testing';
import { FlattenedSet } from './models/flattened-set.class';
import { ExerciseType, ProgramType } from './swagger/model/models';

import { WorkoutService } from './workout.service';

describe('WorkoutService', () => {
  let service: WorkoutService;

  let demoData = {
    "programName": "Workout1",
    "programType": ProgramType.NUMBER_2,
    "workoutTime": new Date(),
    "exercises": [
        {
            "exerciseType": 0,
            "sets": [
                {
                    "plannedReps": 12,
                    "plannedWeight": 57.5,
                    "actualReps": 12,
                    "actualWeight": 57.5,
                    "order": 1
                },
                {
                    "plannedReps": 10,
                    "plannedWeight": 57.5,
                    "actualReps": 10,
                    "actualWeight": 57.5,
                    "order": 2
                },
                {
                    "plannedReps": 8,
                    "plannedWeight": 57.5,
                    "actualReps": 8,
                    "actualWeight": 57.5,
                    "order": 3
                }
            ],
            "order": 1
        },
        {
            "exerciseType": 4,
            "sets": [
                {
                    "plannedReps": 12,
                    "plannedWeight": 52.5,
                    "actualReps": 12,
                    "actualWeight": 52.5,
                    "order": 1
                },
                {
                    "plannedReps": 10,
                    "plannedWeight": 52.5,
                    "actualReps": 10,
                    "actualWeight": 52.5,
                    "order": 2
                },
                {
                    "plannedReps": 8,
                    "plannedWeight": 52.5,
                    "actualReps": 8,
                    "actualWeight": 52.5,
                    "order": 3
                }
            ],
            "order": 2
        },
        {
            "exerciseType": 3,
            "sets": [
                {
                    "plannedReps": 12,
                    "plannedWeight": 35.0,
                    "actualReps": 12,
                    "actualWeight": 35.0,
                    "order": 1
                },
                {
                    "plannedReps": 10,
                    "plannedWeight": 35.0,
                    "actualReps": 10,
                    "actualWeight": 35.0,
                    "order": 2
                },
                {
                    "plannedReps": 8,
                    "plannedWeight": 35.0,
                    "actualReps": 8,
                    "actualWeight": 35.0,
                    "order": 3
                }
            ],
            "order": 3
        }
    ]
};

  let demoFlattenedList: FlattenedSet[] = [
    {exerciseType: ExerciseType.Squat, set: {plannedReps: 5, plannedWeight: 80, actualReps: 5, actualWeight: 80, order: 1}},
    {exerciseType: ExerciseType.BenchPress, set: {plannedReps: 5, plannedWeight: 50, actualReps: 5, actualWeight: 50, order: 2}},
    {exerciseType: ExerciseType.BentOverRow, set: {plannedReps: 5, plannedWeight: 40, actualReps: 5, actualWeight: 40, order: 3}},
  ]

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(WorkoutService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should return nine sets when workout is flattened', () => {
    let flattenedList = service.flattenWorkoutToList(demoData);
    expect(flattenedList.length).toBe(9);
  });

  it('should return the exercise type BenchPress as the exercise name for the first three sets in the list', () => {
    let flattenedList = service.flattenWorkoutToList(demoData);
    expect(flattenedList[0].exerciseType).toBe(ExerciseType.BenchPress);
    expect(flattenedList[1].exerciseType).toBe(ExerciseType.BenchPress);
    expect(flattenedList[2].exerciseType).toBe(ExerciseType.BenchPress);
  });

  it('should return the exercise type BentOverRow as the exercise name for the first three sets in the list', () => {
    let flattenedList = service.flattenWorkoutToList(demoData);
    expect(flattenedList[3].exerciseType).toBe(ExerciseType.BentOverRow);
    expect(flattenedList[4].exerciseType).toBe(ExerciseType.BentOverRow);
    expect(flattenedList[5].exerciseType).toBe(ExerciseType.BentOverRow);
  });

  it('should return the exercise type OverheadPress as the exercise name for the first three sets in the list', () => {
    let flattenedList = service.flattenWorkoutToList(demoData);
    expect(flattenedList[6].exerciseType).toBe(ExerciseType.OverheadPress);
    expect(flattenedList[7].exerciseType).toBe(ExerciseType.OverheadPress);
    expect(flattenedList[8].exerciseType).toBe(ExerciseType.OverheadPress);
  });

  it('should return the next set in the list', () => {
    let currentElement = demoFlattenedList[0];
    let nextElement = service.getNextElement(demoFlattenedList, currentElement);
    expect(nextElement).toBe(demoFlattenedList[1]);
  });

  it('should return undefined if the current element is at the end of the list', () => {
    let currentElement = demoFlattenedList[2];
    let nextElement = service.getNextElement(demoFlattenedList, currentElement);
    expect(nextElement).toBe(undefined);
  });

  it('should return undefined if the current element does not exist in the list', () => {
    let currentElement = demoFlattenedList[2];
    currentElement.exerciseType = ExerciseType.OverheadPress;
    let nextElement = service.getNextElement(demoFlattenedList, currentElement);
    expect(nextElement).toBe(undefined);
  });

  it('should return the previous set in the list', () => {
    let currentElement = demoFlattenedList[2];
    let previousElement = service.getPreviousElement(demoFlattenedList, currentElement);
    expect(previousElement).toBe(demoFlattenedList[1]);
  });

  it('should return undefined if the current element is at the beginning of the list', () => {
    let currentElement = demoFlattenedList[0];
    let previousElement = service.getPreviousElement(demoFlattenedList, currentElement);
    expect(previousElement).toBe(undefined);
  });

  it('should return undefined if the current element does not exist in the list', () => {
    let currentElement = {exerciseType: ExerciseType.OverheadPress, set: {plannedReps: 5, plannedWeight: 50, actualReps: 5, actualWeight: 50, order: 2}};
    let previousElement = service.getPreviousElement(demoFlattenedList, currentElement);
    expect(previousElement).toBe(undefined);
  });

  it('should return true if there is a previous element in the flattenedlist to the current element', () => {
    let currentElement = demoFlattenedList[1];
    let hasPreviousElement = service.hasPreviousElement(demoFlattenedList, currentElement);
    expect(hasPreviousElement).toBeTrue();
  });

  it('should return false if there is not a previous element in the flattenedlist to the current element', () => {
    let currentElement = demoFlattenedList[0];
    let hasPreviousElement = service.hasPreviousElement(demoFlattenedList, currentElement);
    expect(hasPreviousElement).toBeFalse();
  });

  it('should return false if the current element does not exist', () => {
    let currentElement = {exerciseType: ExerciseType.OverheadPress, set: {plannedReps: 5, plannedWeight: 50, actualReps: 5, actualWeight: 50, order: 2}};
    let hasPreviousElement = service.hasPreviousElement(demoFlattenedList, currentElement);
    expect(hasPreviousElement).toBeFalse();
  });
  
  it('should return true if there is a next element in the flattenedlist to the current element', () => {
    let currentElement = demoFlattenedList[1];
    let hasNextElement = service.hasNextElement(demoFlattenedList, currentElement);
    expect(hasNextElement).toBeTrue();
  });

  it('should return false if there is not a next element in the flattenedlist to the current element', () => {
    let currentElement = demoFlattenedList[2];
    let hasNextElement = service.hasNextElement(demoFlattenedList, currentElement);
    expect(hasNextElement).toBeFalse();
  });

  it('should return false if the current element does not exist', () => {
    let currentElement = {exerciseType: ExerciseType.OverheadPress, set: {plannedReps: 5, plannedWeight: 50, actualReps: 5, actualWeight: 50, order: 2}};
    let hasNextElement = service.hasNextElement(demoFlattenedList, currentElement);
    expect(hasNextElement).toBeFalse();
  });
  
});
