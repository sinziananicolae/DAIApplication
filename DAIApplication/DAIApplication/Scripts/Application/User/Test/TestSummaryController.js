(function () {
    "use strict";

    angular.module("app")
        .controller("TestSummaryCtrl", ["$scope", "toastr", "$routeParams", "$location", "TestResultService", testController]);

    function testController($scope, toastr, $routeParams, $location, testService) {

        testService.get({ testId: $routeParams.testId, resultId: $routeParams.resultId }, function (response) {
            $scope.lastResult = response.data.LastResult[0];
            $scope.allResults = response.data.AllResults;
            $scope.test = response.data.Test;
        });
    }
}());