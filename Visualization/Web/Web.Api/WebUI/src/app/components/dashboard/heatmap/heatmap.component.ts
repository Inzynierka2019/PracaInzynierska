import { Component, OnInit } from '@angular/core';
import 'leaflet.heat/dist/leaflet-heat.js'
import * as Leaflet from 'leaflet';
import { latLng } from 'leaflet';
import { icon, Map, point, marker, polyline } from 'leaflet';
import { VehiclePositionsService, GeoPosition } from 'src/app/services/vehicle-positions.service';
import { TileLayers } from './tileLayers';

@Component({
  selector: 'app-heatmap',
  templateUrl: './heatmap.component.html',
  styleUrls: ['./heatmap.component.css']
})
export class HeatmapComponent implements OnInit {
  constructor(
    private layers: TileLayers,
    private geoService: VehiclePositionsService) { }

  map: any;

  ngOnInit() {
    this.geoService.getAddressPoints().subscribe((geoPoints) => {
        let newAddressPoints = geoPoints.map(function (p: GeoPosition) { return [p.latitude, p.longitude]; });
        console.log(newAddressPoints);
        const heat = Leaflet.heatLayer(newAddressPoints).addTo(this.map);
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

  onMapReady(map) {
    this.map = map;
  }

  options = {
    layers: [ this.layers.mapBoxStreetsBasic ],
    zoom: 16,
    center: latLng([54.371764, 18.612528])
  };
}