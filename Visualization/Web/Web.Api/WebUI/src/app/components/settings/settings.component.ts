import { Component, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { SimulationPreferences } from 'src/app/interfaces/simulation-preferences';
import { UnityService } from 'src/app/services/unity.service';
import { Observable } from 'rxjs';
import { SnackBarService } from 'src/app/services/snack-bar.service';
import { ScenePreferences } from 'src/app/interfaces/scene-preferences';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})
export class SettingsComponent implements OnInit {

  constructor(
    private unityService: UnityService,
    private snackBar: SnackBarService) { }

  preferences: ScenePreferences[];

  ngOnInit() {
    this.unityService.getPreferences().subscribe((p) => {
      this.preferences = p;
    });
  }

  savePreferences() {
    this.unityService.savePreferences(this.preferences).subscribe(
      response => { this.snackBar.open("Preferences saved!", 500) },
      error => { this.snackBar.open("Preferences can't be saved!", 1000) }); 
  }
}
