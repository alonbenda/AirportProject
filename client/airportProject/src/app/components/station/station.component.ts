import { Component, Input, OnInit } from '@angular/core';
import Station from 'src/app/models/station.model';

@Component({
  selector: 'app-station',
  templateUrl: './station.component.html',
  styleUrls: ['./station.component.css']
})
export class StationComponent implements OnInit {

  @Input() id?: number;
  @Input() role?: string;
  @Input() station?: Station;


  airplane: string = "https://businessaircraft.bombardier.com/sites/default/files/styles/retina_1430x1430/public/2017-09/Challenger650_specs-render-top.png?itok=-YP8O66C";

  constructor() { }

  ngOnInit(): void {
  }

}
