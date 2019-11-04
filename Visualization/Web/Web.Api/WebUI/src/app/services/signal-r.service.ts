import { Injectable } from '@angular/core';
import { SnackBarService } from './snack-bar.service';
import * as signalR from "@aspnet/signalr";

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  timeout = 5000;
  initialConnection = true;
  private hubConnection: signalR.HubConnection

  constructor(private snackBar: SnackBarService) {
    this.hubConnection = new signalR.HubConnectionBuilder()
                              .withUrl('https://localhost:5001/UIHub')
                              .build();

    this.startConnection();

    this.hubConnection.onclose(() => {
      this.snackBar.open("Refresh page to enable live reloading.", -1);
    });
  }

  public startConnection = () => {
    this.hubConnection
      .start()
      .then(() => this.snackBar.open('Visualization App now started!'))  
      .catch(err => {
        if(this.initialConnection) {
          this.initialConnection = false;
          this.snackBar.open("Can't connect with the Visualization App!", -1);
        }
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
}
