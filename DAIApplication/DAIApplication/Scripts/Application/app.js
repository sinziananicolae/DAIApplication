(function () {
    "use strict";

    angular.module("app", [
        "ngRoute",
        "ngSanitize",
        "ngResource",
        "ngAnimate",
        "services",
        "directives",
        "filters",
        "directivesTheme",
        "toastr"
    ]).config(["$routeProvider",
        function ($routeProvider) {
            $routeProvider.
                when("/admin-dashboard", {
                    templateUrl: "Scripts/Application/Admin/Dashboard/Views/dashboard.html",
                    controller: "AdminDashboardCtrl",
                    resolve: {
                        function($location, StoreService) {
                            var interval = setInterval(function () {
                                if (StoreService.getUser()) {
                                    clearInterval(interval);
                                    if (StoreService.getUserRole() == 2) {
                                        $location.path("/user-dashboard");
                                    }
                                }
                            }, 200);
                        }
                    }
                }).
                when("/createTest", {
                    templateUrl: "Scripts/Application/Admin/Test/Views/test.html",
                    controller: "TestCtrl"
                }).
                when("/edit-test/:id", {
                    templateUrl: "Scripts/Application/Admin/Test/Views/test.html",
                    controller: "TestCtrl"
                }).
                when("/take-test/:id", {
                    templateUrl: "Scripts/Application/User/Test/Views/test.html",
                    controller: "TakeTestCtrl"
                }).
                when("/test-summary/:testId/:resultId?", {
                    templateUrl: "Scripts/Application/User/Test/Views/testSummary.html",
                    controller: "TestSummaryCtrl"
                }).
                when("/user-dashboard", {
                    templateUrl: "Scripts/Application/User/Dashboard/Views/dashboard.html",
                    controller: "UserDashboardCtrl",
                    resolve: {
                        "check": function ($location, UserService) {
                            UserService.get(function(response) {
                                if (response.data.UserRole == 1) {
                                    $location.path("/admin-dashboard");
                                }
                            });
                        }
                    }
                }).
            otherwise({
                redirectTo: "/user-dashboard"
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
        }]);
}());