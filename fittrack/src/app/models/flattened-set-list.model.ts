import { ExerciseType } from "../swagger/model/models";
import { Set } from "../swagger/model/models";

export interface FlattenedSetList {
    exerciseType: ExerciseType;
    set: Set;
}
