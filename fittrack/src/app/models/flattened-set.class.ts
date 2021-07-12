import { ExerciseType } from "../swagger/model/models";
import { Set } from "../swagger/model/models";

export interface FlattenedSet {
    exerciseType: ExerciseType;
    set: Set;
}