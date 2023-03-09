import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import {HttpClientModule} from '@angular/common/http';

import { AppComponent } from './app.component';
import { AirportComponent } from './components/airport/airport.component';
import { StationComponent } from './components/station/station.component';
import { HubService } from './services/hub.service';

@NgModule({
  declarations: [
    AppComponent,
    AirportComponent,
    StationComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [ HubService ],
  bootstrap: [AppComponent]
})
export class AppModule { }
