(function () {
    "use strict";

    angular.module("app-user", [
        "ngRoute",
        "ngSanitize",
        "ngResource",
        "ngAnimate",
        "services",
        "directives",
        "directivesTheme",
    ]).config(["$routeProvider",
        function ($routeProvider) {
            $routeProvider.
            when("/dashboard", {
                templateUrl: "Scripts/Application/User/Modules/Dashboard/Views/dashboard.html",
                controller: "DashboardCtrl"
            }).
            otherwise({
                redirectTo: "/dashboard"
            });
        }]);
}());