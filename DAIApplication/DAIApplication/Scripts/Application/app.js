(function () {
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
        function ($routeProvider) {
            $routeProvider.
            when("/dashboard", {
                templateUrl: "Scripts/Application/Admin/Dashboard/Views/dashboard.html",
                controller: "DashboardCtrl"
            }).
            otherwise({
                redirectTo: "/dashboard"
            });
        }]).controller("GreetingController", ["StoreService", "UserService", function (StoreService, UserService) {
            UserService.get(function (response) {
                console.log(response);
            });
        }]);;
}());