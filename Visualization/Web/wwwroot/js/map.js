let myMap;
let canvas;
const mappa = new Mappa('Leaflet');
const options = {
    lat: 0,
    lng: 0,
    zoom: 4,
    studio: true, // false to use non studio styles
    //style: 'mapbox.dark' //streets, outdoors, light, dark, satellite (for nonstudio)
    style: 'mapbox://styles/mapbox/traffic-night-v2'
};

let sketch = function (p) {
    p.setup = function () {
        canvas = p.createCanvas(840, 500);
        p.background(0);
        myMap = mappa.tileMap(options);
        myMap.overlay(canvas);
    };
};
new p5(sketch, window.document.getElementById('heatmap'));