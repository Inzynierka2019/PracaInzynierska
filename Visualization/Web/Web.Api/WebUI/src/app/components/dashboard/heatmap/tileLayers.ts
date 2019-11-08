import { tileLayer } from 'leaflet';
import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root'
})
export class TileLayers {
    mapBoxToken = "pk.eyJ1IjoiamFjZWthcmQiLCJhIjoiY2l6NHMzdDN5MDAzaDJ3cGdiZzFta3k0byJ9.taOtgWr3RKQJblAaSOJYMA";

    streetMaps = tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        detectRetina: true,
    });
    wMaps = tileLayer('http://maps.wikimedia.org/osm-intl/{z}/{x}/{y}.png', {
        detectRetina: true,
    });
    mapBoxSattelite = tileLayer('https://api.tiles.mapbox.com/v4/{id}/{z}/{x}/{y}.png?access_token={accessToken}', {
        detectRetina: true,
        maxZoom: 20,
        id: 'mapbox.streets-satellite',
        accessToken: this.mapBoxToken
    });
    mapBoxStreets = tileLayer('https://api.tiles.mapbox.com/v4/{id}/{z}/{x}/{y}.png?access_token={accessToken}', {
        detectRetina: true,
        maxZoom: 20,
        id: 'mapbox.streets',
        accessToken: this.mapBoxToken
    });
    mapBoxComic = tileLayer('https://api.tiles.mapbox.com/v4/{id}/{z}/{x}/{y}.png?access_token={accessToken}', {
        detectRetina: true,
        maxZoom: 20,
        id: 'mapbox.comic',
        accessToken: this.mapBoxToken
    });
    mapBoxStreetsBasic = tileLayer('https://api.tiles.mapbox.com/v4/{id}/{z}/{x}/{y}.png?access_token={accessToken}', {
        detectRetina: true,
        maxZoom: 20,
        id: 'mapbox.streets-basic',
        accessToken: this.mapBoxToken
    });
    mapBoxLight = tileLayer('https://api.tiles.mapbox.com/v4/{id}/{z}/{x}/{y}.png?access_token={accessToken}', {
        detectRetina: true,
        maxZoom: 20,
        id: 'mapbox.light',
        accessToken: this.mapBoxToken
    });
    mapBoxPencil = tileLayer('https://api.tiles.mapbox.com/v4/{id}/{z}/{x}/{y}.png?access_token={accessToken}', {
        detectRetina: true,
        maxZoom: 20,
        id: 'mapbox.pencil',
        accessToken: this.mapBoxToken
    });
    mapBoxOutdoors = tileLayer('https://api.tiles.mapbox.com/v4/{id}/{z}/{x}/{y}.png?access_token={accessToken}', {
        detectRetina: true,
        maxZoom: 20,
        id: 'mapbox.outdoors',
        accessToken: this.mapBoxToken
    });
    mapBoxPirates = tileLayer('https://api.tiles.mapbox.com/v4/{id}/{z}/{x}/{y}.png?access_token={accessToken}', {
        detectRetina: true,
        maxZoom: 20,
        id: 'mapbox.pirates',
        accessToken: this.mapBoxToken
    });   
}