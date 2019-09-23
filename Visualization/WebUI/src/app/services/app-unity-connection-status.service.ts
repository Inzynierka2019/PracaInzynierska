import { Injectable } from '@angular/core';
import { SignalRService } from './signal-r.service';
import { SnackBarService } from './snack-bar.service';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AppUnityConnectionStatusService {

  private _isConnected = new BehaviorSubject<boolean>(false);
  public status$ = this._isConnected.asObservable();
  public isConnected: boolean;

  constructor(
    private signalR: SignalRService,
    private snackBar: SnackBarService) {
    this.isConnected = false;
    this.registerSignalHandlers();
    this.snackBar.open("Simulator app is disconnected.");
   }

  registerSignalHandlers(): void {
    this.signalR.registerHandler(
      'SignalForUnityAppConnectionStatus',
      (isConnected: boolean) => {
        this.isConnected = isConnected;
        this._isConnected.next(isConnected);
      }
    );
  }
  
}
