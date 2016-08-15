(function(){
    "use strict";

    angular.module("app", [
        "ngRoute",
        "ngSanitize",
        "ngResource",
        "ngAnimate",
        "services",
        "directives",
        "directivesTheme",
    ]).config(["$routeProvider",
        function($routeProvider) {
            $routeProvider.
            when("/dashboard", {
                templateUrl: "js/app/Modules/Dashboard/Views/dashboard.html",
                controller: "DashboardCtrl"
            }).
            otherwise({
                redirectTo: "/dashboard"
            });
        }]);
}());