import { Injectable } from '@angular/core';
import { SnackBarService } from './snack-bar.service';
import * as signalR from "@aspnet/signalr";

@Injectable({
  providedIn: 'root'
})
export class SignalRService {

  timeout = 3000;
  private hubConnection: signalR.HubConnection

  public startConnection = () => {
    this.hubConnection = new signalR.HubConnectionBuilder()
                              .withUrl('https://localhost:5001/UIHub')
                              .build();

    this.hubConnection
      .start()
      .then(() => this.snackBar.open('SignalR started'))
      .catch(err => {
        setTimeout(() => {
          this.startConnection();
        }, this.timeout);
      });
  }

  public registerHandler(methodName: string, method: (...args: any[]) => void): void {
    this.hubConnection.on(methodName, method);
  }

  public removeHandler(methodName: string, method: (...args: any[]) => void): void {
    this.hubConnection.off(methodName, method);
  }

  constructor(private snackBar: SnackBarService) {
    this.startConnection();
  }


}
