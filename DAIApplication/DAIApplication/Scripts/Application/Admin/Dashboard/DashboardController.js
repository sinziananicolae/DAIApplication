(function () {
    "use strict";

    angular.module("app")
        .controller("AdminDashboardCtrl", ["$scope", "TestService", homeController]);

    function homeController($scope, testService) {
        testService.get(function (response) {
            $scope.tests = response.data;

            $scope.totalVisitorsNo = 0;

            _.each($scope.tests, function (test) {
                $scope.totalVisitorsNo += test.Visits;
            });
        });

        $scope.currentDate = moment().format("DD MMMM YYYY");

        setInterval(function () {
            $scope.currentTime = moment().format("h:mm:ss A");
            $scope.$apply();
        }, 1000);
    }
}());