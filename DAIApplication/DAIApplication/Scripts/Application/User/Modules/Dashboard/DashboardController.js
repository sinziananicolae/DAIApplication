(function () {
    'use strict';

    angular.module('app')
        .controller('DashboardCtrl', ['$scope', '$location', '$interval', 'WeatherService', homeController]);
    
    function homeController($scope, $location, $interval, WeatherService) {
        $scope.getCurrentDateTime = function(){
            $scope.currentDateTime = moment().format('DD-MM-YYYY h:mm:ss a');
        };

        $scope.getCurrentWeather = function() {
            WeatherService.get({type: "current"}, function(response){
                $scope.currentWeather = response.data[0];
            });
        }

        $scope.getCurrentWeather();

        $scope.map = {
            center: {
                latitude: 44.5248078,
                longitude: 26.7019943
            },
            zoom: 10,
            marker: {
                id: 21,
                coords: {
                    latitude: 44.5248078,
                    longitude: 26.7019943
                },
                options: {
                    draggable: false,
                    icon: "css/app-images/home.png"
                }
            }
        };

        $scope.goToMapPage = function(){
            $location.path('/map');
        };

        $interval($scope.getCurrentDateTime, 100);
    }
}());