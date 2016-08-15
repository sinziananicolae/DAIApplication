(function(){
    'use strict';

    angular.module('app', [
        'ngRoute',
        'ngSanitize',
        'ngResource',
        'ngAnimate',
        'services',
        'helpers',
        'directives',
        'directivesTheme',
        'uiGmapgoogle-maps'
    ]).config(['$routeProvider',
        function($routeProvider) {
            $routeProvider.
            when('/dashboard', {
                templateUrl: 'js/app/Modules/Dashboard/Views/dashboard.html',
                controller: 'DashboardCtrl'
            }).
            when('/login', {
                templateUrl: 'js/app/Modules/Login/Views/login.html',
                controller: 'LoginCtrl'
            }).
             when('/select-hive-file', {
                templateUrl: 'js/app/Modules/HiveFile/Views/select-hive-file.html',
                controller: 'HiveFileCtrl'
            }).
            when('/hive-file/:hiveId', {
                templateUrl: 'js/app/Modules/HiveFile/Views/hive-file.html',
                controller: 'HiveFileCtrl'
            }).
            when('/hives-info', {
                templateUrl: 'js/app/Modules/HivesInfo/Views/all-hives-info.html',
                controller: 'HivesInfoCtrl'
            }).
            when('/hives-info/:hiveId', {
                templateUrl: 'js/app/Modules/HivesInfo/Views/hive-info.html',
                controller: 'HivesInfoCtrl'
            }).
            when('/map', {
                templateUrl: 'js/app/Modules/Map/Views/map.html',
                controller: 'MapCtrl'
            }).
            when('/weather-report', {
                templateUrl: 'js/app/Modules/WeatherReport/Views/weather-report.html',
                controller: 'WeatherReportCtrl'
            }).
            otherwise({
                redirectTo: '/login'
            });
        }]);
}());