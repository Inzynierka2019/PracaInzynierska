import { Injectable } from '@angular/core';
import { OverlayModule } from "@angular/cdk/overlay";
import {
  MatSnackBar,
  MatSnackBarConfig,
  MatSnackBarHorizontalPosition,
  MatSnackBarVerticalPosition,
} from '@angular/material';

@Injectable({
  providedIn: 'root'
})
export class SnackBarService {
  snackBarConfig: MatSnackBarConfig;
  horizontalPosition: MatSnackBarHorizontalPosition = 'right';
  verticalPosition: MatSnackBarVerticalPosition = 'bottom';
  snackBarAutoHide = '2000';

  constructor(private snackBar: MatSnackBar) { }

  open(message) {
    this.snackBarConfig = new MatSnackBarConfig();
    this.snackBarConfig.horizontalPosition = this.horizontalPosition;
    this.snackBarConfig.verticalPosition = this.verticalPosition;
    this.snackBarConfig.duration = parseInt(this.snackBarAutoHide, 0);
    this.snackBarConfig.panelClass = 'snackbar';
    this.snackBar.open(message, 'Close', this.snackBarConfig);
  }
}
