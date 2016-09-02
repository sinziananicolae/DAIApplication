(function () {
    "use strict";

    angular.module("app")
        .controller("UserHistoryCtrl", ["$scope", "toastr", "$location", "UserHistoryService", historyController]);

    function historyController($scope, toastr, $location, userHistoryService) {
        var getUserHistory = function() {
            $scope.tests = [];
            userHistoryService.get(function (response) {
                $scope.userHistory = {};
                _.each(response.data, function(history) {
                    if (!$scope.userHistory[history.TestId]) {
                        $scope.userHistory[history.TestId] = history;
                        $scope.userHistory[history.TestId].Tests = [];
                    }
                    $scope.userHistory[history.TestId].Tests.push(history);

                });
            });
        };

        getUserHistory();

        $scope.takeTest = function (test) {
            $location.path("/take-test/" + test.TestId);
        }
    }
}());