(function () {
    "use strict";

    angular.module("app")
        .controller("AdminDashboardCtrl", ["$scope", "toastr", "TestService", homeController]);

    function homeController($scope, toastr, testService) {
        var getAllTests = function () {
            $scope.tests = [];
            testService.get(function (response) {
                $scope.tests = response.data;

                $scope.totalVisitorsNo = 0;

                _.each($scope.tests, function (test) {
                    $scope.totalVisitorsNo += test.Visits;
                });
            });
        };

        getAllTests();
        $scope.currentDate = moment().format("DD MMMM YYYY");

        setInterval(function () {
            $scope.currentTime = moment().format("h:mm:ss A");
            $scope.$apply();
        }, 1000);

        $scope.removeTest = function (test) {
            if (confirm("Are you sure you want to delete test " + test.Name + "?\n")) {
                testService.delete({ id: test.Id },
                    function (response) {
                        if (response.success)
                            toastr.success("Test successfully removed!");
                        else {
                            toastr.error(response.message);
                        }
                        getAllTests();
                    });
            }
        };
    }
}());