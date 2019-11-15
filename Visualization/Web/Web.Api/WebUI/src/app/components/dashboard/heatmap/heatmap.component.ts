import { Component, OnInit } from '@angular/core';
import 'leaflet.heat/dist/leaflet-heat.js'
import * as Leaflet from 'leaflet';
import { latLng } from 'leaflet';
import { icon, Map, point, marker, polyline } from 'leaflet';
import { VehicleStatus } from 'src/app/interfaces/chart-models';
import { VehiclePositionsService } from 'src/app/services/vehicle-positions.service';
import { TileLayers } from './tileLayers';
import { UnityService } from 'src/app/services/unity.service';
import { SimulationPreferences } from 'src/app/interfaces/scene-preferences';
declare var L;

@Component({
  selector: 'app-heatmap',
  templateUrl: './heatmap.component.html',
  styleUrls: ['./heatmap.component.css']
})
export class HeatmapComponent implements OnInit {
  map: any;
  vehicleMaxCount = 1000;
  preferences: SimulationPreferences;
  latRef: number;
  lonRef: number;
  options: any;

  constructor(
    private layers: TileLayers,
    private geoService: VehiclePositionsService,
    private unityService: UnityService) {
      this.unityService.getGeoPositionReference().subscribe(
        (ref) => {
          this.latRef = ref.latitude;
          this.lonRef = ref.longitude;
        });
        
        this.options = {
          layers: [ this.layers.mapBoxStreetsBasic ],
          zoom: 16,
          center: latLng([this.latRef, this.lonRef])
        };
    }

  ngOnInit() {}

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