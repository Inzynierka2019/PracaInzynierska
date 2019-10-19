import { Injectable } from '@angular/core';
import { SignalRService } from './signal-r.service';
import { SnackBarService } from './snack-bar.service';
import { UnityAppState } from '../interfaces/unity-app-state';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AppUnityConnectionStatusService {
  public isConnected: boolean;
  public appTimeSpan: string;
  public appState: UnityAppState;
  private timeSpanUrl = "/api/unity/timespan"

  constructor(
    private signalR: SignalRService,
    private snackBar: SnackBarService,
    private httpClient: HttpClient) {
      this.isConnected = false;
      this.registerSignalHandlers();
      this.appState = UnityAppState.NOT_CONNECTED;
  }

  connected() {
    this.isConnected = true;
    this.snackBar.open("Simulator app is now connected!");
  }

  notConnected() {
    this.isConnected = false;
  }

  disconnected() {
    this.snackBar.open("Simulator app has ended!");
    this.isConnected = false;
    this.httpClient.get(this.timeSpanUrl).subscribe(
      (timespan: any) => {
        this.appTimeSpan = timespan;
      },
      error => {
        console.log(error);
      }
    );
  }

  registerSignalHandlers(): void {
    this.signalR.registerHandler(
      'SignalForUnityAppConnectionStatus',
      (appState: UnityAppState) => {
        if (this.appState != appState)
          this.appState = appState;
        else return;

        switch (appState) {
          case UnityAppState.NOT_CONNECTED:
            this.notConnected();
            break;

          case UnityAppState.CONNECTED:
            this.connected();
            break;

          case UnityAppState.DISCONNECTED:
            this.disconnected();
            break;

          case UnityAppState.RUNNING:
            break;
        }
      }
    );
  }

}
