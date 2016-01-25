/// <reference path="C:\Users\pfahlen\Documents\geoAlive\geoLive\WebStreamedLayer\scripts/XSockets.latest.js" />
var socketMap = socketMap || {};

var map;

socketMap.init = function(){
	map = new ol.Map({
		target: "map",
		layers: [
			new ol.layer.Tile({
				source : new ol.source.OSM()
			}),
			new ol.layer.Vector({
				title: "Observations",
				source: new ol.source.Vector()
			})
		],
		view: new ol.View({
    		center: [0, 0],
    		zoom: 2
  		})
	});

	var conn = new XSockets.WebSocket("ws://localhost:4502", ["GeoCars"]);
	var controller = conn.controller("geocars");
	controller.on("pos",sock.carMove);
};

socketMap.carMove = function (data) {
    var json = JSON.parse(data);
    var id = json.id;
    var lat = json.geodata.Lat;
    var lon = json.geodata.Lng;

};

//coordinates in format[x, y], coordinates expects an array, attributes can be any object
socketMap.createFeature = function(coordinates, attributes){
	var point = new ol.geom.Point(coordinates);
	var feature = new ol.Feature({
		geometry: point,
		attributes: attributes
	});
	return feature;
};

//creates rules for styling
var createStyle = function(feature){
	var attributes = feature.get("attributes");
	var val = attributes.val;
	var fill = val === 1 ? "red" : 
		val === 2 ? "blue" : "green";
	
	var style = new ol.style.Style({
	    'LineString': new ol.style.Style({
	        stroke: new ol.style.Stroke({
	            color: 'green',
	            width: 2
	        })
	    }),
		image: new ol.style.Circle({
			fill: new ol.style.Fill({color: fill}),
			stroke: new ol.style.Stroke({
				color: "white",
				width: 1
			}),
			radius: 3
		})
	});
	return style;
	};


socketMap.setObservation = function(feature){
	var observationLayer = map.getLayers().getArray()[1];
	var source = observationLayer.getSource();
	var currentFeature = socketMap.findFeature(feature, source) || feature;

	var style = new createStyle(currentFeature);
	currentFeature.setStyle(style);
	source.addFeatures([currentFeature]);
};

socketMap.findFeature = function(currentFeature, source){
	var features = source.getFeatures();
	if (features.length === 0) return null;

	var currentAttributes = currentFeature.get("attributes");
	var currentId = currentAttributes.id;
	var findFeatures = features.filter(function(feature){
		var attributes = feature.get("attributes");
		return attributes.id === currentId;
	});
	var feature = findFeatures.length === 1 ? findFeatures[0] : null;
	return feature;
};

//var obs1 = new socketMap.createFeature([0, 0], {id: 1, val: 1});
//var obs2 = new socketMap.createFeature([5000000, 5000000], {id: 2, val: 2});

//var timerId = setTimeout(function(){
//	if (map === null) return;

//	socketMap.setObservation(obs1);
//	socketMap.setObservation(obs2);
//	clearInterval(timerId);
//}, 10);


//socketMap.changeColor = function(){
//	var val = Math.ceil((Math.random()*10000)%3);
//	var updateObs = new socketMap.createFeature([5000000, 5000000], {id: 2, val: val});
//	socketMap.setObservation(updateObs);
//};

socketMap.transformFeature = function (feature) {
    feature.getGeometry().transform("EPSG:4326", "EPSG:3857");
};