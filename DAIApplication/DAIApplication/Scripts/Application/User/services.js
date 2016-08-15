(function () {
    'use strict';

    var serviceMetods = {
        get: { method: 'GET',
            headers: { 'id': 1} 
        },
        create: { method: 'POST',
            headers: { 'id': 1} 
        },
        update: { method: 'PUT' },
        patch: { method: 'PATCH' },
        remove: { method: 'DELETE' }
    };

    var APIurl = "http://beenocular-v1.azurewebsites.net/api";

    angular.module('services', ['ngResource'])
    .factory('HiveFileService', ['$resource', '$http', function ($resource, $http) {
        $http.defaults.useXDomain = true;
        return $resource(APIurl + '/hivefiles/:hiveId/:type', { hiveId: "@hiveId", type: "@type" }, serviceMetods);
    }])

    .factory('MapService', ['$resource', '$http', function ($resource, $http) {
        $http.defaults.useXDomain = true;
        return $resource(APIurl + '/map-markers', { }, serviceMetods);
    }])

    .factory('WeatherService', ['$resource', '$http', function ($resource, $http) {
        $http.defaults.useXDomain = true;
        return $resource(APIurl + '/weather-params/:type', {type: "@type" }, serviceMetods);
    }])

    .factory('HivesService', ['$resource', '$http', function ($resource, $http) {
        $http.defaults.useXDomain = true;
        return $resource(APIurl + '/hives-number', { }, serviceMetods);
    }]);

}());