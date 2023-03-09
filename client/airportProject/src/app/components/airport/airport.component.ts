import { Component, OnInit } from '@angular/core';
import Station from 'src/app/models/station.model';
import { HubService } from 'src/app/services/hub.service';

@Component({
  selector: 'app-airport',
  templateUrl: './airport.component.html',
  styleUrls: ['./airport.component.css']
})
export class AirportComponent implements OnInit {

  takeOff: string = "Take off";
  lane: string = "Lane";
  landing: string = "Landing";
  terminal: string = "Terminal";
  emergency: string = "Emergancy";

  isStarted: boolean = false;

  get firstStation(): Station | undefined { return this.hubService.firstStation; }
  get secondStation(): Station | undefined { return this.hubService.secondStation; }
  get thirdStation(): Station | undefined { return this.hubService.thirdStation; }
  get forthStation(): Station | undefined { return this.hubService.forthStation; }
  get fifthStation(): Station | undefined { return this.hubService.fifthStation; }
  get sixthStation(): Station | undefined { return this.hubService.sixthStation; }
  get seventhStation(): Station | undefined { return this.hubService.seventhStation; }
  get eighthStation(): Station | undefined { return this.hubService.eighthStation; }
  get ninthStation(): Station | undefined { return this.hubService.ninthStation; }

  get emergancyStation(): Station | undefined { return this.hubService.emergancyStation; }

  hubService: HubService;
  constructor(hubService: HubService) {
    this.hubService = hubService;
  }

  ngOnInit(): void {
  }

}
