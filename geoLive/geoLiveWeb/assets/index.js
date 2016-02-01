/// <reference path="bmapi.js" />

Microsoft.Maps.Pushpin.prototype.setPinId = function (id) { this.pushpinId = id };
Microsoft.Maps.Pushpin.prototype.getPinId = function () { return this.pushpinId; };


var LoadMap = function () {
    var loadMap = function () {
        var mapElem = document.querySelector('#map');
        Microsoft.Maps.registerModule("ConfigurationModule", "assets/ConfigurationModule.js");
        Microsoft.Maps.loadModule("ConfigurationModule", {
            callback: function () {
                getConfig(function (jsonConfig) {
                    var config = ConfigurationModule.getConfig(jsonConfig);
                    var map = new Microsoft.Maps.Map(mapElem, config);
                    initConnection(map);
                })
            }
        });
    };

    var getConfig = function (callback) {
        var request = new XMLHttpRequest();
        request.addEventListener("load", function () {
            var config = JSON.parse(this.responseText);
            callback(config);
        });
        request.open("GET", "/configuration.json");
        request.send();
    };

    var getPushpin = function (data) {
        var pushpinOptions = { icon: '/assets/greysquare.jpg', width: 8, height: 8 };
        var location = new Microsoft.Maps.Location(data.lat, data.lon);
        var pin = new Microsoft.Maps.Pushpin(location, pushpinOptions);
        pin.setPinId(data.id);
        return pin;
    };

    var initConnection = function (map) {
        var conn = new XSockets.WebSocket("ws://localhost:4502", ["GeoCars"]);
        var controller = conn.controller("geocars");
        controller.on("pos", function (data) {
            var location = {
                lat: data.geodata.Lat,
                lon: data.geodata.Lng,
                id: data.id
            };
            var pushPin = getPushpin(location);
            plotPin(map, pushPin);
        });
    };


    var plotPin = function (map, pushPin) {
        var found = false;
        var length = map.entities.getLength();
        for (var i = 0; i < length; i++) {
            var entity = map.entities.get(i);
            if (entity instanceof Microsoft.Maps.Pushpin) {
                var id = entity.getPinId();
                if (id === pushPin.getPinId()) {
                    entity.setLocation(pushPin.getLocation());
                    found = true;
                    break;
                }
            }
        }
        if (!found) {
            map.entities.push(pushPin);
        }
    };

    return loadMap;
}();