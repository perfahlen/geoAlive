﻿var ConfigurationModule = new function () { var n = function (n) { var r = Object.assign(n); return r.bounds && (r.bounds = e(r.bounds), r.center && delete r.center), !r.bounds && r.center && (r.center = o(r.center)), r.centerOffset && !r.bounds && r.center && (r.centerOffset = t(r.centerOffset)), r.labelOverlay && (r.labelOverlay = Microsoft.Maps.LabelOverlay[r.labelOverlay]), r.mapTypeId && (r.mapTypeId = Microsoft.Maps.MapTypeId[r.mapTypeId]), r }, e = function (n) { var e = o(n.center), t = new Microsoft.Maps.LocationRect(e, n.height, n.width); return t }, t = function (n) { var e = new Microsoft.Maps.Point(n.point.x, n.point.y); return e }, o = function (n) { var e = new Microsoft.Maps.Location(n.latitude, n.longitude); return e }; this.getConfig = function (e) { if (!e) return void console.log("Missing config."); e.constructor === String && (e = JSON.parse(e)); var t = n(e); return t } }; "function" != typeof Object.assign && !function () { Object.assign = function (n) { "use strict"; if (void 0 === n || null === n) throw new TypeError("Cannot convert undefined or null to object"); for (var e = Object(n), t = 1; t < arguments.length; t++) { var o = arguments[t]; if (void 0 !== o && null !== o) for (var r in o) o.hasOwnProperty(r) && (e[r] = o[r]) } return e } }(), Microsoft.Maps.moduleLoaded("ConfigurationModule");