import { Component, OnInit } from '@angular/core';
import { UnityService } from 'src/app/services/unity.service';
import { SnackBarService } from 'src/app/services/snack-bar.service';
import { SimulationPreferences } from 'src/app/interfaces/scene-preferences';
import { GeoPosition } from 'src/app/interfaces/chart-models';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})
export class SettingsComponent implements OnInit {

  constructor(
    private unityService: UnityService,
    private snackBar: SnackBarService) { }

  settings: SimulationPreferences = new SimulationPreferences();

  ngOnInit() {

    this.unityService.getPreferences().subscribe((settings) => {
      this.settings = settings;
    });
  }

  savePreferences() {
    this.unityService.savePreferences(this.settings).subscribe(
      response => { this.snackBar.open("Preferences saved!", 500) },
      error => { this.snackBar.open("Preferences can't be saved!", 1000) }); 
  }
}
