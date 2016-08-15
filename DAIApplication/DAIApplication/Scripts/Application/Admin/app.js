(function(){
    "use strict";

    angular.module("app-admin", [
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
                templateUrl: "Scripts/Application/Admin/Modules/Dashboard/Views/dashboard.html",
                controller: "DashboardCtrl"
            }).
            otherwise({
                redirectTo: "/dashboard"
            });
        }]);
}());