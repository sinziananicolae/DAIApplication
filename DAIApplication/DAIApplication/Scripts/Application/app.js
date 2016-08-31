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
            when("/admin-dashboard", {
                templateUrl: "Scripts/Application/Admin/Dashboard/Views/dashboard.html",
                controller: "AdminDashboardCtrl"
            }).
            when("/createTest", {
                templateUrl: "Scripts/Application/Admin/Test/Views/test.html",
                controller: "TestCtrl"
            }).
                when("/edit-test/:id", {
                    templateUrl: "Scripts/Application/Admin/Test/Views/test.html",
                    controller: "TestCtrl"
                }).
            otherwise({
                redirectTo: "/admin-dashboard"
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

            $scope.logOff = function () {
                window.location.href = "/Account/LogOff";
            }
        }]);;
}());