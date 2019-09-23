import { Injectable } from '@angular/core';
import { SnackBarService } from './snack-bar.service';
import * as signalR from "@aspnet/signalr";

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  timeout = 10000;
  private hubConnection: signalR.HubConnection

  constructor(private snackBar: SnackBarService) {
    this.startConnection();
  }

  public startConnection = () => {
    this.hubConnection = new signalR.HubConnectionBuilder()
                              .withUrl('https://localhost:5001/UIHub')
                              .build();

    this.hubConnection
      .start()
      .then(() => this.snackBar.open('Visualization App now started!'))  
      .catch(err => {
          this.snackBar.open("Can't connect with the Visualization App!");
      });
  }

  public registerHandler(methodName: string, method: (...args: any[]) => void): void {
    this.hubConnection.on(methodName, method);
  }

  public removeHandler(methodName: string, method: (...args: any[]) => void): void {
    this.hubConnection.off(methodName, method);
  }
}
