var key = 'AuQ2kqDNtkNtNd0FU1nPWQFPBo14nJiZ3VkaukVwnzB9DGy6lCrfSSX6cHAEjxHE';

var mapState = { navigate: true, to: false, from: false };
var SearchServiceCallback = function (resp) {
    console.log(resp);
};



var loadMap = function () {
    var mapElem = document.querySelector('#map');
    var map = new Microsoft.Maps.Map(mapElem, { credentials: key });
    //Microsoft.Maps.Events.addHandler(map, 'click', function (e) {
    //    if (mapState.to || mapState.from) {
    //        console.log("click");
    //    }
    //});

    //var findElem = document.querySelector("#cities");
    //findElem.addEventListener("click", function (evt) {
    //    var routesToCalc = getRoutesToCalc();
    //});
    //var routes = [];
    //var getRoutesToCalc = function () {
        
    //    for (var i = 0; i < cities.length; i++) {
    //        var from = cities[i];
    //        for (var j = 0; j < cities.length; j++) {
    //            if (j === i) continue;
    //            var route = { from: cities[i], to: cities[j] };
    //            var registred = isRegistered(routes, route);
    //            if (!registred) {
    //                routes.push(route);
    //            }
    //        }
    //    }
    //    calcRoutes();
    //};


    //var calcRoutes = function () {

    //    var calcRoute = routes.pop();
    //    callRouteService(calcRoute.from.name, calcRoute.to.name);
    //}


    //var isRegistered = function (routes, route) {
    //    for (var i = 0; i < routes.length; i++) {
    //        if (routes[i].from === route.to && route.from === routes[i].to)
    //            return true;
    //    }
    //    return false;
    //}

    //function callRouteService(start, end) {
    //    var credentials = key;
    //    routeName = start.split(",")[0].trim() + end.split(",")[0].trim();
    //    var routeRequest = 'https://dev.virtualearth.net/REST/v1/Routes?wp.0=' + start + '&wp.1=' + end + '&routePathOutput=Points&output=json&jsonp=routeCallback&key=' + credentials;
    //    var mapscript = document.createElement('script');
    //    mapscript.type = 'text/javascript';
    //    mapscript.src = routeRequest;
    //    document.getElementById('map').appendChild(mapscript);
    //}


    //var geoCodeCities = function () {
    //    var city = cities.pop();
    //    fetchfromService(city, geoCodeCities);
    //};

    //var fetchfromService = function (city, callback) {
    //    var mapscript = document.createElement('script');
    //    mapscript.type = 'text/javascript';
    //    mapscript.src = city.url;
    //    document.querySelector('#map').appendChild(mapscript);
    //};

    //var toggleMapstate = function (newState) {
    //    for (var state in mapState) {
    //        state = false;
    //    }

    //    state = newState;
    //};

    //return { calcRoutes: calcRoutes };
}();

//var routeName;

//var routeCallback = function (resp) {
//    try{
//        if (resp.resourceSets) {
//            var payload = resp.resourceSets[0].resources[0].routePath.line.coordinates;
//            var formData = new FormData();
//            formData.append("json", JSON.stringify(payload));
//            var url = "services/SaveRoute.ashx?routeName=" + routeName;
//            fetch(url, {
//                method: "POST",
//                body: formData
//            }).then(function () {
//                debugger;
//                loadMap.calcRoutes();
//            });
//        }
//    } catch (x) {
//        loadMap.calcRoutes();
//    }
//};
