import { Component, OnInit, Input } from '@angular/core';
import 'leaflet.heat/dist/leaflet-heat.js'
import * as Leaflet from 'leaflet';
import { latLng } from 'leaflet';
import { icon, Map, point, marker, polyline } from 'leaflet';
import { VehicleStatus, GeoPosition } from 'src/app/interfaces/chart-models';
import { VehiclePositionsService } from 'src/app/services/vehicle-positions.service';
import { TileLayers } from './tileLayers';
import { UnityService } from 'src/app/services/unity.service';
import { SimulationPreferences } from 'src/app/interfaces/scene-preferences';
import { AppUnityConnectionStatusService } from 'src/app/services/app-unity-connection-status.service';
declare var L;

@Component({
  selector: 'app-heatmap',
  templateUrl: './heatmap.component.html',
  styleUrls: ['./heatmap.component.css']
})
export class HeatmapComponent implements OnInit {
  geoReference: GeoPosition;
  map: any;
  zoom: number = 12;
  vehicleMaxCount = 1000;
  preferences: SimulationPreferences;
  options: any;
  defaultLatLon = { lat: 52.0, lng: 20.0 }; // Poland

  constructor(
    private layers: TileLayers,
    private geoService: VehiclePositionsService,
    private appStatus: AppUnityConnectionStatusService,
    private unityService: UnityService) {

    this.appStatus.isConnectedEvent.subscribe((isConnected: boolean) => {
      this.loadGeoReference();
    });
  }

  public ngOnInit() {
    this.loadGeoReference();

    this.options = {
      layers: [this.layers.mapBoxStreetsBasic],
      zoom: 6,
      center: latLng(this.defaultLatLon)
    };
  }

  loadGeoReference() {
    this.unityService.getGeoPositionReference().subscribe((geoRef) => {
      this.geoReference = geoRef;
      setTimeout(() =>
        this.map.flyTo([this.geoReference.latitude, this.geoReference.longitude], this.zoom + 1),
        3000);
    });
  }

  layersControl = {
    baseLayers: {
      'OpenStreetMaps': this.layers.streetMaps,
      'Wikimedia Maps': this.layers.wMaps,
      'MapBox Streets': this.layers.mapBoxStreets,
      'MapBox Basic': this.layers.mapBoxStreetsBasic,
      'MapBox Sattelite': this.layers.mapBoxSattelite,
      'MapBox Light': this.layers.mapBoxLight,
      'MapBox Pencil': this.layers.mapBoxPencil,
      'MapBox Comic': this.layers.mapBoxComic,
      'MapBox Outdoors': this.layers.mapBoxOutdoors,
      'MapBox Pirates': this.layers.mapBoxPirates
    }
  };

  onMapReady(map: any) {
    this.map = map;

    const heat = L.heatLayer([]).addTo(this.map);

    this.geoService.getAddressPoints().subscribe((geoPoints) => {

      let newAddressPoints = geoPoints.map(function (p: VehicleStatus) { return L.latLng(p.latitude, p.longitude); });
      var a = L.latLng(50.5, 30.5);
      heat.setLatLngs(newAddressPoints);
    });
  }
}