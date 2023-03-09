import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import Station from '../models/station.model';

@Injectable()
export class HubService {

  constructor() { 
    const connection = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Information)
      .withUrl(environment.airportUrl, {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
      })
      .build();

    connection.start().then(() => {
      console.log('SignalR Connected');
    }).catch(function(err) {
      return console.error(err.toString());
    });

    connection.on("UpdateAirport", (stations) => {
      console.log("connection on")
      this.updateAirport(stations);
    })
  }

  public firstStation?: Station;
  public secondStation?: Station;
  public thirdStation?: Station;
  public forthStation?: Station;
  public fifthStation?: Station;
  public sixthStation?: Station;
  public seventhStation?: Station;
  public eighthStation?: Station;
  public ninthStation?: Station;

  public emergancyStation?: Station;

  private updateAirport(stations: Station[]) {
    stations.forEach(station => {
      if (station !== null) {
        switch (station.id) {
          case 0:
            this.emergancyStation = station;
            break;
          case 1:
            this.firstStation = station;
            break;
          case 2:
            this.secondStation = station;
            break;
          case 3:
            this.thirdStation = station;
            break;
          case 4:
            this.forthStation = station;
            break;
          case 5:
            this.fifthStation = station;
            break;
          case 6:
            this.sixthStation = station;
            break;
          case 7:
            this.seventhStation = station;
            break;
          case 8:
            this.eighthStation = station;
            break;
          case 9:
            this.ninthStation = station;
            break;
          default:
            break;
        }
      }
    });    
  }
}
