import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { HttpClientModule } from '@angular/common/http';
import { MatchSelectionComponent } from './match-selection/match-selection.component';
import { EventEntryComponent } from './event-entry/event-entry.component';
import { EditEntriesComponent } from './edit-entries/edit-entries.component';

@NgModule({
  declarations: [
    AppComponent,
    MatchSelectionComponent,
    EventEntryComponent,
    EditEntriesComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
