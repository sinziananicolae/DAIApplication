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
        "toastr"
    ]).config(["$routeProvider",
        function ($routeProvider) {
            $routeProvider.
            when("/dashboard", {
                templateUrl: "Scripts/Application/Admin/Dashboard/Views/dashboard.html",
                controller: "DashboardCtrl"
            }).
            when("/createTest", {
                templateUrl: "Scripts/Application/Admin/Test/Views/test.html",
                controller: "TestCtrl"
            }).
            otherwise({
                redirectTo: "/dashboard"
            });
        }]).controller("GreetingController", ["$scope", "StoreService", "UserService", "toastr", function ($scope, storeService, userService, toastr) {
            $scope.loading = true;

            userService.get(function (response) {
                if (response.success) {
                    storeService.save(response.data);
                    $scope.user = storeService.getUser();
                } else {
                    toastr.error(response.message);
                    window.location.href = "/Account/LogIn";
                }
                $scope.loading = false;
            });

            $scope.logOff = function() {
                window.location.href = "/Account/LogOff";
            }
    }]);;
}());