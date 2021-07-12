import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdminComponent } from './admin/admin.component';
import { SetComponent } from './set/set.component';
import { WorkoutComponent } from './workout/workout.component';

const routes: Routes = [
  {path: 'workout', component: WorkoutComponent },
  {path: 'workout/set', component: SetComponent },
  {path: 'admin', component: AdminComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
