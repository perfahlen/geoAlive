/// <reference path="C:\Users\pfahlen\Documents\geoAlive\geoLive\WebStreamedLayer\scripts/XSockets.latest.js" />
var socketMap = socketMap || {};

var map;

socketMap.carMove = function (data) {
    var json = JSON.parse(data);
    var id = json.id;
    var lat = json.geodata.Lat;
    var lon = json.geodata.Lng;
    console.log(id, lat, lon);
};

//coordinates in format[x, y], coordinates expects an array, attributes can be any object
socketMap.createFeature = function(coordinates, attributes){
    var point = new ol.geom.Point(coordinates);
    point.transform("EPSG:4326", "EPSG:3857");
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


socketMap.updating = false;
socketMap.setObservation = function (feature) {
    
    if (!socketMap.updating) {

        socketMap.updating = true;

        try{
            var observationLayer = map.getLayers().getArray()[1];
            var source = observationLayer.getSource();
            var currentFeatures = socketMap.findFeature(feature, source) || feature;
            var currentFeature = currentFeatures[0] || feature;

            var style = new createStyle(currentFeature);
            currentFeature.setStyle(style);

            if (currentFeatures.length > 1) {
                for (var i = 0; i < currentFeatures.length; i++) {
                    source.removeFeature(currentFeatures[i]);
                }
            }

            source.addFeatures([currentFeature]);
        }catch(x){
            socketMap.updating = false;
        }
        socketMap.updating = false;
    }
};
socketMap.findFeature = function (currentFeature, source) {
    //debugger;

	var features = source.getFeatures();
	if (features.length === 0) return null;

	var currentAttributes = currentFeature.get("attributes");
	var currentId = currentAttributes.id;
	var findFeatures = features.filter(function(feature){
		var attributes = feature.get("attributes");
		return attributes.id === currentId;
	});
    
	var feature = findFeatures.length > 0 ? findFeatures[0] : null;
	//if (findFeatures.length >= 1) {
    //    findFeatures
	//}
	return findFeatures;
};

socketMap.transformFeature = function (feature) {
    feature.getGeometry().transform("EPSG:4326", "EPSG:3857");
};

socketMap.init = function () {
    map = new ol.Map({
        target: "map",
        layers: [
			new ol.layer.Tile({
			    source: new ol.source.OSM()
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
    controller.on("pos", function (data) {
        var feature = socketMap.createFeature([data.geodata.Lng, data.geodata.Lat], { id: data.id });
        socketMap.setObservation(feature);
    });
};