import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { ChartsModule } from 'ng2-charts';
import { NgxSpinnerModule } from "ngx-spinner";
import { OverlayModule } from "@angular/cdk/overlay";
import { MatSnackBarModule } from '@angular/material';
import { MatExpansionModule } from '@angular/material/expansion';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatCardModule } from '@angular/material/card';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ConsoleComponent } from './components/console/console.component';
import { UnityConnectorComponent } from './components/unity-connector/unity-connector.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { AboutComponent } from './components/about/about.component';
import { LogsComponent } from './components/console/logs/logs.component';
import { VehiclePopulationComponent } from './components/dashboard/charts/vehicle-population/vehicle-population.component';

@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent,
    ConsoleComponent,
    UnityConnectorComponent,
    AboutComponent,
    LogsComponent,
    VehiclePopulationComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ChartsModule,
    NgxSpinnerModule,
    OverlayModule,
    MatSnackBarModule,
    MatExpansionModule,
    BrowserAnimationsModule,
    MatCardModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }